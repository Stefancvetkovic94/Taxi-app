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

        private string LoggedIn => Request.Cookies[CookieKeys.Login]?.Value;
        private IBaza Baza => (IBaza)HttpContext.Application[ApplicationKeys.Baza];
    }
}