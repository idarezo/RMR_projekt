using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_projek.Models
{
    public class Podnajemnik : Uporabnik
    {
        public Podnajemnik(string adr, string email, int id, string ime, string telefon, string geslo) : base(adr, email, id, ime, telefon, geslo)
        {
        }
    }
}
