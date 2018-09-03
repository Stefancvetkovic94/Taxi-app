using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Lokacija
    {
        public int Id { get; set; }

        public string X_kordinata { get; set; }
        public string Y_kordinata { get; set; }
        public string Adresa { get; set; }


    }
}