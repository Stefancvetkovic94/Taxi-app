using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Voznja
    {

        public enum Status
        {
            Kreirana,
            Formirana,
            Obradjena,
            Prihvacena,
            Otkazana,
            Neuspesna,
            Uspesna
        }
        public int Id { get; set; }

        public DateTime Datum_Vreme { get; set; }
        public string Lokacija { get; set; }
        public string Tip { get; set; }
        public string Musterija { get; set; }
        public string Odrediste { get; set; }
        public string Dispecer { get; set; }
        public string Vozac { get; set; }
        public int Iznos { get; set; }
        public string Komentar { get; set; }
        public Status Status_Voznje { get; set; }



    }
}