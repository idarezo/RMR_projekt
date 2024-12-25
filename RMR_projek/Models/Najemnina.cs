using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_projek.Models
{
    public class Najemnina
    {
        public int id { get; set; }
        public double visina { get; set; }
        public DateTime zacetek { get; set; }
        public DateTime konec { get; set; }
        public bool placano { get; set; }
        public DateTime DatumZapadlosti { get; set; }
    }
}
