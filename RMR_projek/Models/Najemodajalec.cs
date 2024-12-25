using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_projek.Models
{
    public class Najemodajalec : Uporabnik
    {
        public int steviloNepremicnin { get; set; }
        public Najemodajalec(string adr, string email, int id, string ime, string telefon, string geslo, int steviloNepremicnin) : base(adr, email, id, ime, telefon, geslo)
        {
            this.steviloNepremicnin = steviloNepremicnin;
        }
    }
}
