using Microcharts;
using Newtonsoft.Json;
using RMR_projek.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_projek.ViewModels
{
    public class MojProfilViewPage
    {
        public Uporabnik uporabnik { get; set; }
        List<Nepremicnina> nepremicnine { get; set; }

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
                HoleRadius = 0.4f // Optional, creates a donut chart effect
            };

            // Assuming you have a ChartView named pieChartView in your XAML
          //  pieChartView.Chart = pieChart;
        }

    }
}
