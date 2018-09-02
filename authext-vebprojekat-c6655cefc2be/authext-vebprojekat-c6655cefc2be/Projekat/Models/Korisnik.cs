using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Korisnik
    {
        public enum Uloga
        {
            Musterija,
            Dispecer,
            Vozac
        }

        public string Korisnicko_Ime { get; set; }
        public string Lozinka { get; set; }

        public string Ime { get; set; }
        public string Prezime { get; set; }

        public Uloga Uloga_Korisnika { get; set; }

        public string Pol { get; set; }
        public string JMBG { get; set; }

        public string Telefon { get; set; }
        public string Email { get; set; }



        public List<Voznja> Voznje { get; set; }

        public string Lokacija { get; set; }
        public string Automobil { get; set; }
        public int Slobodan { get; set; }



    }
}