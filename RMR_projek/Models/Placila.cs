using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_projek.Models
{
    public class Placila
    {
        public int id { get; set; }

        public Najemnina najemnina { get; set; }
      
        public VodaPoraba voda { get; set; }
        public ElektrikaPoraba elektrika { get; set; }
        public bool placano { get; set; }

        public bool placanoVse => voda.placano && elektrika.placano && najemnina.placano;

    }
}
