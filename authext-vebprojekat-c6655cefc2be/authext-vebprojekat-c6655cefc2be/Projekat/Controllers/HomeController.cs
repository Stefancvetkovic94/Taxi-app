using Projekat.Baza;
using System.Web.Mvc;
using System.Collections.Generic;


namespace Projekat.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            if (LoggedIn != null)
            {
                Projekat.Models.Korisnik korisnik = new Models.Korisnik();
                korisnik = Baza.GetUser(LoggedIn);
                ViewBag.User = korisnik;

                if(korisnik.Uloga_Korisnika== Models.Korisnik.Uloga.Musterija)
                {
                    IEnumerable<Models.Voznja> voznje = new List<Models.Voznja>();
                    voznje = Baza.GetVoznjeMusterija(LoggedIn);
                    ViewBag.Voznje = voznje;
                    List<Models.Komentar> komentari = new List<Models.Komentar>();
                    komentari = Baza.GetKomentareZaVoznje();
                    ViewBag.Komentari = komentari;
                    
                }

                if (korisnik.Uloga_Korisnika == Models.Korisnik.Uloga.Dispecer)
                {
                    IEnumerable<Models.Voznja> voznje = new List<Models.Voznja>();
                    voznje = Baza.GetVoznjeDispecer(LoggedIn);
                    ViewBag.Voznje = voznje;

                    List<Models.Voznja> voznjesve = new List<Models.Voznja>();
                    voznjesve = Baza.GetVoznjeSve();
                    ViewBag.VoznjeSve = voznjesve;

                    List<Models.Komentar> komentari = new List<Models.Komentar>();
                    komentari = Baza.GetKomentareZaVoznje();
                    ViewBag.Komentari = komentari;

                    IEnumerable<string> vozaci = new List<string>();
                    vozaci = Baza.GetVozaci();
                    ViewBag.Vozaci = vozaci;

                }
            }
            


            return View();
        }

        [Route("User/{user}")]
        public ActionResult ShowUser(string user)
        {
            ViewBag.User = Baza.GetUser(user);
            if (ViewBag.User == null)
                return HttpNotFound();

            if (user == LoggedIn)
            {
                
               
            }

            return View();
        }

        [Route("OtkaziVoznju/{id}")]
        public ActionResult OtkaziVoznju(int id)
        {

            if(Baza.OtkaziVoznju(id))
            {

            }
            ViewBag.Id = id;
            ViewBag.Korisnik = LoggedIn;
            return View("AddComent");
        }

        [Route("IzmeniVoznju/{id}")]
        public ActionResult IzmeniVoznju(int id)
        {


            ViewBag.Id = id;
            ViewBag.Korisnik = LoggedIn;
            return View("IzmeniVoznju");
        }

        [Route("IzmeniVoznju2")]
        public ActionResult IzmeniVoznju2()
        {
            if (LoggedIn == null)
                return View("NotLoggedIn");





            if (Request.HttpMethod == "GET")
                return View();

            string Lokacija = Request.Params["Lokacija"];
            string Tip = Request.Params["Tip"];
            string Id = Request.Params["Id"];
            




            if (Baza.IzmeniVoznju(Lokacija, Tip, Id))
                ViewBag.Title = "Editing Successful";
            else
                ViewBag.Title = "Editing Failed";

            return View("EditVoznjaResult");
        }

        [Route("DodeliVozaca/{id}")]
        public ActionResult DodeliVozaca(int id)
        {
            IEnumerable<string> vozaci = new List<string>();
            vozaci = Baza.GetVozaci();
            ViewBag.Vozaci = vozaci;

            ViewBag.Id = id;
            ViewBag.Korisnik = LoggedIn;
            return View("DodeliVozaca");
        }

        [Route("DodeliVozaca2")]
        public ActionResult DodeliVozaca2()
        {
            if (LoggedIn == null)
                return View("NotLoggedIn");





            if (Request.HttpMethod == "GET")
                return View();

            string id = Request.Params["id"];
            string vozac = Request.Params["vozac"];
            string dispecer = LoggedIn;


            Baza.VozacZauzet(vozac);

            if (Baza.DodeliVozacaVoznji(id, vozac, dispecer))
                ViewBag.Title = "Dodeljen vozac";
            else
                ViewBag.Title = "Vozac nije dodeljen";

            return View("DodeliVozacaResult");
        }


        private string LoggedIn => Request.Cookies[CookieKeys.Login]?.Value;
        private IBaza Baza => (IBaza)HttpContext.Application[ApplicationKeys.Baza];
    }
}