using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Komentar
    {
        public uint Id { get; set; }
        public string Opis { get; set; }
        public DateTime Datum { get; set; }
        public string Korisnik { get; set; }
        public int Voznja { get; set; }
        public int Ocena { get; set; }




    }
}