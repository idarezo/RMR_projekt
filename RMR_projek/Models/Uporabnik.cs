using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_projek.Models
{
   public class Uporabnik
    {
        [JsonProperty("drzava", NullValueHandling = NullValueHandling.Ignore)]
        public string naslov { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string email { get; set; }

        [JsonProperty("geslo", NullValueHandling = NullValueHandling.Ignore)]
        public string geslo { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }

        [JsonProperty("ime", NullValueHandling = NullValueHandling.Ignore)]
        public string ime { get; set; }


        [JsonProperty("priimek", NullValueHandling = NullValueHandling.Ignore)]
        public string priimek { get; set; }

       

        [JsonProperty("telefon", NullValueHandling = NullValueHandling.Ignore)]
        public string telefon { get; set; }




        public Uporabnik(string adr, string email, int id, string ime, string telefon, string geslo)
        {
            this.naslov = adr;
            this.email = email;
            this.id = id;
            this.ime = ime;
            this.telefon = telefon;
            this.geslo = geslo;
        }
    }
}
