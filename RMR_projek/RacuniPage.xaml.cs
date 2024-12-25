using Newtonsoft.Json;
using RMR_projek.Models;
using Microcharts.Maui;
using SkiaSharp;
using Microcharts;
using System.Collections.Generic;

namespace RMR_projek;

public partial class RacuniPage : ContentPage
{
	public Uporabnik uporabnik { get; set; }
    List<Nepremicnina> nepremicnine { get; set; }

  
	public RacuniPage(Uporabnik uporabnik)
	{
		this.uporabnik= uporabnik;
		InitializeComponent();
        PridobiNepremicnine();




    }
    
	async public  void PridobiNepremicnine()
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

            PridobiStatistiko(nepremicnine);


        }

      

    }
    async public void PridobiStatistiko(List<Nepremicnina> nepremicnine)
    {
        double voda = 0.00;
        double elektrika = 0.00;
        double najemnina = 0.00;

        foreach (var nepremicnina in nepremicnine)
        {
            voda += nepremicnina.placila.Sum(p=>p.voda.poraba * p.voda.tarifa);
            elektrika += nepremicnina.placila.Sum(p=>p.elektrika.poraba * p.elektrika.tarifa);
            najemnina += nepremicnina.placila.Sum(p=> p.najemnina.visina);
        }

      DisplayChart(voda, elektrika, najemnina);
      DisplayLineChart(nepremicnine);

    }

    async public void DisplayChart(double voda, double elektrika, double najemnina)
    {

        var entries = new[]
     {
        new ChartEntry((float)voda)
        {
            Label = "Voda",
            ValueLabel = voda.ToString("C"),
            Color = SKColor.Parse("#66ccff"),
            TextColor = SKColor.Parse("#000000"),
            ValueLabelColor = SKColor.Parse("#000000")
        },
        new ChartEntry((float)elektrika)
        {
            Label = "Elektrika",
            ValueLabel = elektrika.ToString("C"),
            Color = SKColor.Parse("#ffcc00"),
            TextColor = SKColor.Parse("#000000"),
            ValueLabelColor = SKColor.Parse("#000000")
        },
        new ChartEntry((float)najemnina)
        {
            Label = "Najemnina",
            ValueLabel = najemnina.ToString("C"),
            Color = SKColor.Parse("#ff6666"),
            TextColor = SKColor.Parse("#000000"),
            ValueLabelColor = SKColor.Parse("#000000")
        }
    };



        var chart = new BarChart
        {
            Entries = entries,
            LabelTextSize = 40f,
            Margin = 20,
            BackgroundColor = SKColor.Parse("#ffffff") 
        };
     

       
        chartView.Chart = chart;  
    }

    async public void DisplayLineChart(List<Nepremicnina> nepremicnine)
    {
        var waterEntries = new List<ChartEntry>();
        var elektrikaEntries = new List<ChartEntry>();

        foreach (var nepremicnina in nepremicnine)
        {
            foreach (var placilo in nepremicnina.placila)
            {
               
                waterEntries.Add(new ChartEntry((float)placilo.voda.poraba)
                {
                    Label = placilo.voda.casovniZig.ToString("MMM dd"), // Format the timestamp
                    ValueLabel = placilo.voda.poraba.ToString(),
                    Color = SKColor.Parse("#3498db") // Blue for water
                });

              
                elektrikaEntries.Add(new ChartEntry((float)placilo.elektrika.poraba)
                {
                    Label = placilo.elektrika.casovniZig.ToString("MMM dd"),
                    ValueLabel = placilo.elektrika.poraba.ToString(),
                    Color = SKColor.Parse("#e74c3c") // Red for electricity
                });
            }
        }

       
        var waterChart = new LineChart
        {
            Entries = waterEntries,
            LineMode = LineMode.Straight, 
            LineSize = 4,
            PointMode = PointMode.Circle,
            PointSize = 8,
            LabelTextSize = 30
        };

        var elektrikaChart = new LineChart
        {
            Entries = elektrikaEntries,
            LineMode = LineMode.Straight,
            LineSize = 4,
            PointMode = PointMode.Circle,
            PointSize = 8,
            LabelTextSize = 30
        };


        waterChartView.Chart = waterChart;
        elektrikaChartView.Chart = elektrikaChart;

    }


}