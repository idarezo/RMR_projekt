using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_projek.Models
{
    public class Nepremicnina
    {
        public int id { get; set; }
        public string naslov { get; set; }
        public int stPrebivalcev { get; set; }

        public Najemodajalec lastnik { get; set; }
        public List<Podnajemnik> podnajemniki { get; set; }

        public List<Placila> placila { get; set; }

    }
}
