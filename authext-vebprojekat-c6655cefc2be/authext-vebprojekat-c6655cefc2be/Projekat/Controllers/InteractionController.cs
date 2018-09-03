using Projekat.Baza;
using Projekat.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;


namespace Projekat.Controllers
{
    public class InteractionController : Controller
    {

       
       

        [Route("AddDriver")]
        public ActionResult AddDriver()
        {
            if (LoggedIn == null)
                return View("NotLoggedIn");

            if (Baza.GetUser(LoggedIn).Uloga_Korisnika != Models.Korisnik.Uloga.Dispecer)
                return View("NotAuthorized");

            if (Request.HttpMethod == "GET")
                return View();
            
            
            var user = new Korisnik()
            {
                Korisnicko_Ime = Request.Params["username"],
                Lozinka = Request.Params["password"],
                Ime = Request.Params["first-name"],
                Prezime = Request.Params["last-name"],
                Telefon = Request.Params["phone-no"],
                Pol = Request.Params["pol"],
                
                JMBG = Request.Params["jmbg"],
                
                Email = Request.Params["email"],
                Uloga_Korisnika = Models.Korisnik.Uloga.Vozac

            };

            if (Baza.Register(user))
                ViewBag.Title = "Vozac Dodat";
            else
                ViewBag.Title = "Vozac nije dodat";

            return View("DriverResult");
        }

       
       
        [Route("EditUser")]
        public ActionResult EditUser()
        {
            if (LoggedIn == null)
                return View("NotLoggedIn");

            

            

            if (Request.HttpMethod == "GET")
                return View();

            string Korisnicko_Ime = LoggedIn;
            string Ime = Request.Params["first-name"];
            string Prezime = Request.Params["last-name"];
            string Telefon = Request.Params["phone-no"];
            string Pol = Request.Params["pol"];

            string JMBG = Request.Params["jmbg"];

            string Email = Request.Params["email"];


           

            if (Baza.EditUser(Korisnicko_Ime, Ime, Prezime, Telefon, Pol, JMBG,Email))
                ViewBag.Title = "Editing Successful";
            else
                ViewBag.Title = "Editing Failed";

            return View("EditUserResult");
        }

        [Route("AddLocationHelp")]
        public ActionResult AddLocationHelp()
        {
            if (LoggedIn == null)
                return View("NotLoggedIn");

            if (Baza.GetUser(LoggedIn).Uloga_Korisnika != Models.Korisnik.Uloga.Vozac)
                return View("NotAuthorized");

            if (Request.HttpMethod == "GET")
                return View();

            return View("AddLocation");

        }


        [Route("AddLocation")]

        public ActionResult AddLocation()
        {
            if (LoggedIn == null)
                return View("NotLoggedIn");

            if (Baza.GetUser(LoggedIn).Uloga_Korisnika != Models.Korisnik.Uloga.Vozac)
                return View("NotAuthorized");

            if (Request.HttpMethod == "GET")
                return View();

            var lokacija = new Models.Lokacija()
            {
                Adresa = Request.Params["adresa"],
                X_kordinata = Request.Params["x"],
                Y_kordinata = Request.Params["y"],
            };
            if (Baza.AddLocation(lokacija))
            {
                
                if (Baza.AddAdress(lokacija.Adresa))
                {
                    
                    if (Baza.UpdateLocationInKorisnik(LoggedIn, lokacija.Adresa))
                    {
                        ViewBag.Title = "Lokacija je promenjena";
                        return View("LocationResult");

                    }
                    else
                    {
                        ViewBag.Title = "Lokacija nije promenjena";
                        return View("LocationResult");
                    }
                }
                else
                {
                    ViewBag.Title = "Lokacija nije promenjena";
                    return View("LocationResult");
                }
            }
            else
            {
                ViewBag.Title = "Lokacija nije promenjena";
                return View("LocationResult");
            }

        }

        [Route("VoznjaMusterija")]
        public ActionResult VoznjaMusterija()
        {
            if (LoggedIn == null)
                return View("NotLoggedIn");

            if (Baza.GetUser(LoggedIn).Uloga_Korisnika != Models.Korisnik.Uloga.Musterija)
                return View("NotAuthorized");

            if (Request.HttpMethod == "GET")
                return View();

            string adresa = Request.Params["adresa"];
            string x = Request.Params["x"];
            string y = Request.Params["y"];
            string tip = Request.Params["tip"];

            Lokacija lokacija = new Lokacija();
            lokacija.X_kordinata = x;
            lokacija.Y_kordinata = y;
            lokacija.Adresa = adresa;
            Baza.AddLocation(lokacija);

            Baza.AddAdress(adresa);

            Voznja voznja = new Voznja();

            voznja.Datum_Vreme = DateTime.Now;
            voznja.Lokacija = adresa;
            voznja.Tip = tip;
            voznja.Musterija = LoggedIn;
            voznja.Status_Voznje = Voznja.Status.Kreirana;

            if (Baza.AddVoznjaMusterija(voznja))
            {
                ViewBag.Title = "Voznja je kreirana i na cekanju";
                return View("VoznjaResult");
            }
            else
            {
                ViewBag.Title = "Voznja nije kreirana";
                return View("VoznjaResult");
            }
                


                  
        }

        [Route("AddComent")]
        public ActionResult AddComent()
        {
            if (LoggedIn == null)
                return View("NotLoggedIn");

           
            if (Request.HttpMethod == "GET")
                return View();

            var komentar = new Komentar()
            {
                Opis = Request.Params["opis"],
                Datum = DateTime.Now,
                Korisnik = Request.Params["korisnik"],
                Voznja = Int32.Parse( Request.Params["voznja"]),
                Ocena = Int32.Parse(Request.Params["ocena"]),
                

            };

            if(Baza.AddComment(komentar))
            {
                if(Baza.AddCommentToVoznja(komentar.Voznja, komentar.Opis))
                {
                    ViewBag.Title = "dodat je komentar";
                    return View("ComentResult");

                }
                else
                {
                    ViewBag.Title = "nije dodat komentar";
                    return View("ComentResult");
                }
            }
            else
            {
                ViewBag.Title = "nije dodat komentar";
                return View("ComentResult");

            }




            
        }

        [Route("AddVoznjaDispecer")]
        public ActionResult AddVoznjaDispecer()
        {

            if (LoggedIn == null)
                return View("NotLoggedIn");


            ViewBag.vozaci = Baza.GetVozaci();



            return View("AddVoznjaDispecer");

        }

        [Route("AddVoznjaDispecer2")]
        public ActionResult AddVoznjaDispecer2()
        {

            if (LoggedIn == null)
                return View("NotLoggedIn");

            string adresa = Request.Params["adresa"];
            string x = Request.Params["x"];
            string y = Request.Params["y"];
            string tip = Request.Params["tip"];
            string vozac = Request.Params["vozac"];

            Lokacija lokacija = new Lokacija();
            lokacija.X_kordinata = x;
            lokacija.Y_kordinata = y;
            lokacija.Adresa = adresa;

            Baza.AddLocation(lokacija);
            Baza.AddAdress(adresa);

            Voznja voznja = new Voznja();

            voznja.Datum_Vreme = DateTime.Now;
            voznja.Lokacija = adresa;
            voznja.Tip = tip;
            voznja.Dispecer = LoggedIn;
            voznja.Status_Voznje = Voznja.Status.Formirana;
            voznja.Vozac = vozac;

            Baza.VozacZauzet(vozac);


            //Baza.UpdateLocationInKorisnik(vozac, adresa);
            if (Baza.AddVoznjaDispecer(voznja))
            {
                ViewBag.Title = "Voznja je kreirana i na cekanju";
                return View("VoznjaResult");
            }
            else
            {
                ViewBag.Title = "Voznja nije kreirana";
                return View("VoznjaResult");
            }

        }


        private string LoggedIn => Request.Cookies[CookieKeys.Login]?.Value;
        private IBaza Baza => (IBaza)HttpContext.Application[ApplicationKeys.Baza];
    }
}