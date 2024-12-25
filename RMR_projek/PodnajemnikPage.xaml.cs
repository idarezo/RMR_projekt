using Firebase.Auth;
using Microcharts;
using Newtonsoft.Json;
using RMR_projek.Models;
using SkiaSharp;

namespace RMR_projek;

public partial class PodnajemnikPage : ContentPage
{
    public Uporabnik uporabnik { get; set; }
    public string FullName => $"{uporabnik.ime} {uporabnik.priimek}";
    List<Nepremicnina> nepremicnine { get; set; }
    public PodnajemnikPage(Uporabnik uporabnik)
	{
		InitializeComponent();
        this.uporabnik = uporabnik;
        BindingContext = this;
        PridobiNepremicnine();


    }

    private async void prikaziNevem(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistracijaPage());

    }

    private async void prikaziMojProfil(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MojProfilPage(uporabnik));

    }

    private async void prikaziStatistiko(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StatistikaPage());

    }

    private async void prikaziRacune(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RacuniPage(uporabnik));

    }



    async public void PridobiNepremicnine()
    {
        var databaseUrl = "https://rmr-projektnovo-default-rtdb.europe-west1.firebasedatabase.app/";
        var userRef = $"Nepremicnina";
        var response = await new HttpClient().GetStringAsync(databaseUrl + userRef + ".json");


        if (!string.IsNullOrEmpty(response) && response != "null")
        {

            var nepremicnineDict = JsonConvert.DeserializeObject<List<Nepremicnina>>(response);
            nepremicnine = nepremicnineDict.ToList();

            var iskaneNepremicnine = nepremicnineDict
                       .Where(nepremicnina => nepremicnina.podnajemniki.Any(u => u.email == uporabnik.email))
                       .ToList();


            foreach (var nepremicnina in iskaneNepremicnine)
            {
                var uporabnikData = nepremicnina.podnajemniki.FirstOrDefault(u => u.email == uporabnik.email);
                if (uporabnikData != null)
                {
                    Console.WriteLine($"Naslov: {nepremicnina.naslov}, Ime uporabnika: {uporabnikData.ime} {uporabnikData.priimek}");
                }
            }

        }

        PridobiStatistiko(nepremicnine);

    }


    async public void PridobiStatistiko(List<Nepremicnina> nepremicnine)
    {
        double voda = 0.00;
        double elektrika = 0.00;
        double najemnina = 0.00;
        double skupnaVsota = 0.00;

        foreach (var nepremicnina in nepremicnine)
        {
            voda += nepremicnina.placila.Sum(p => p.voda.poraba * p.voda.tarifa);
            elektrika += nepremicnina.placila.Sum(p => p.elektrika.poraba * p.elektrika.tarifa);
            najemnina += nepremicnina.placila.Sum(p => p.najemnina.visina);
        }


        skupnaVsota = voda + elektrika + najemnina;
        voda = (voda / skupnaVsota) * 100;
        elektrika = (elektrika / skupnaVsota) * 100;
        najemnina = (najemnina / skupnaVsota) * 100;
        DisplayPieChart(voda,elektrika,najemnina);
    }

    async public void DisplayPieChart(double voda, double elektrika, double najemnina)
    {
        var entries = new[]
        {
        new ChartEntry((float)voda)
        {
            Label = "Water",
            ValueLabel = voda.ToString("0.0") + "%",
            Color = SKColor.Parse("#66ccff"),
        },
        new ChartEntry((float)elektrika)
        {
            Label = "Electricity",
            ValueLabel = elektrika.ToString("0.0") + "%",
            Color = SKColor.Parse("#ffcc00"),
        },
        new ChartEntry((float)najemnina)
        {
            Label = "Rent",
            ValueLabel = najemnina.ToString("0.0") + "%",
            Color = SKColor.Parse("#ff6666"),
        }
    };

        var pieChart = new DonutChart
        {
            Entries = entries,
            LabelTextSize = 40f,
            BackgroundColor = SKColor.Parse("#ffffff"),
            HoleRadius = 0.4f 
        };

       
        pieChartView.Chart = pieChart;
    }
}