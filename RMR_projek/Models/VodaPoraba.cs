using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_projek.Models
{
    public class VodaPoraba
    {
        public int id { get; set; }
        public double poraba { get; set; }
        public double tarifa { get; set; }

        public DateTime casovniZig { get; set; }
        public bool placano { get; set; }
        public DateTime DatumZapadlosti { get; set; }
        public double SkupniStrosek => poraba * tarifa;
    }
}
