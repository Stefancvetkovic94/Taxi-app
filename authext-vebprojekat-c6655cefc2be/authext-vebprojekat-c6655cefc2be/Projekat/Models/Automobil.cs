using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Automobil
    {

        public enum Tip
        {
            Putnicki,
            Kombi
        }

        public string Vozac { get; set; }
        public int Godiste { get; set; }
        public int Br_Registracije { get; set; }
        public int Broj_Vozila { get; set; }
        public Tip Tip_Automobila { get; set; }





    }
}