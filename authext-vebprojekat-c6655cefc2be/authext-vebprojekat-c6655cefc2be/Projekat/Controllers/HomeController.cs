using Projekat.Baza;
using Projekat.Models;
using System;
using System.Linq;
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

                if(korisnik.Uloga_Korisnika == Models.Korisnik.Uloga.Musterija)
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
                    List<Models.Voznja> voznjesvee = new List<Models.Voznja>();

                    voznjesve = Baza.GetVoznjeSve();
                   // voznjesvee = voznjesve.OrderByDescending(x => x.Datum_Vreme).ToList();
                    ViewBag.VoznjeSve = voznjesve;

                    List<Models.Komentar> komentari = new List<Models.Komentar>();
                    komentari = Baza.GetKomentareZaVoznje();
                    ViewBag.Komentari = komentari;

                    

                    //var newList = people.OrderBy(x => x.LastName)
                }

                if (korisnik.Uloga_Korisnika == Models.Korisnik.Uloga.Vozac)
                {
                    IEnumerable<Models.Voznja> voznje = new List<Models.Voznja>();
                    voznje = Baza.GetVoznjeVozac(LoggedIn);
                    ViewBag.Voznje = voznje;

                    List<Models.Voznja> voznjesve = new List<Models.Voznja>();
                    voznjesve = Baza.GetVoznjeSveKreirane();
                    ViewBag.VoznjeSve = voznjesve;

                    List<Models.Komentar> komentari = new List<Models.Komentar>();
                    komentari = Baza.GetKomentareZaVoznje();
                    ViewBag.Komentari = komentari;

                    

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


        [Route("PrihvatiVoznju/{id}")]
        public ActionResult PrihvatiVoznju(int id)
        {
            int idvoznje = id;

            Baza.VozacZauzet(LoggedIn);

            if(Baza.PrihvatiVoznju(idvoznje, LoggedIn))
            {
                ViewBag.Title = "Voznja Prihvacena";
                return View("PrihvatiVoznjuResult");

            }
            else
            {
                ViewBag.Title = "Voznja nije prihvacena";
                return View("PrihvatiVoznjuResult");

            }

        }

        [Route("Neuspesna/{id}")]
        public ActionResult Neuspesna(int id)
        {
            int idvoznje = id;

            Baza.VozacSlobodan(LoggedIn);
            ViewBag.Korisnik = LoggedIn;
            ViewBag.Id = idvoznje;


            if (Baza.VoznjaNeuspesna(idvoznje))
            {
                return View("AddComentNeuspesna");

            }
            else
            {
                return View("AddComentNeuspesna");

            }

        }

        [Route("Uspesna/{id}")]
        public ActionResult Uspesna(int id)
        {
            int idvoznje = id;

            Baza.VozacSlobodan(LoggedIn);
            ViewBag.Korisnik = LoggedIn;
            ViewBag.Id = idvoznje;


         
                return View("VoznjaUspesna");

            

        }

        [Route("VoznjaUspesna")]
        public ActionResult VoznjaUspesna()
        {
            if (LoggedIn == null)
                return View("NotLoggedIn");


            if (Request.HttpMethod == "GET")
                return View();

            string id = Request.Params["voznja"];
            string odrediste = Request.Params["adresa"];
            string x = Request.Params["x"];
            string y = Request.Params["y"];
            string iznos = Request.Params["iznos"];

            Baza.UpdateLocationInKorisnik(LoggedIn, odrediste);
            Baza.AddAdress(odrediste);

            Lokacija lok = new Lokacija();
            lok.Adresa = odrediste;
            lok.X_kordinata = x;
            lok.Y_kordinata = y;
            Baza.AddLocation(lok);


            if (Baza.VoznjaUspesna(Int32.Parse(id), odrediste, Int32.Parse(iznos)))
            {
                ViewBag.Title = "Voznja Uspesna";

                return View("UspesnaResult");

            }
            else
            {
                ViewBag.Title = "Voznja nije uspesna";

                return View("UspesnaResult");

            }

        }

        [Route("KomentarUspesna/{id}")]
        public ActionResult KomentarUspesna(int id)
        {
            int idvoznje = id;

            ViewBag.Korisnik = LoggedIn;
            ViewBag.Id = idvoznje;



            return View("AddComent");



        }

        [Route("Filtriraj")]
        public ActionResult Filtriraj()
        {
            Projekat.Models.Korisnik korisnik = new Models.Korisnik();
            korisnik = Baza.GetUser(LoggedIn);
            ViewBag.User = korisnik;

            int status = Int32.Parse(Request.Params["status"]);


            if (korisnik.Uloga_Korisnika == Models.Korisnik.Uloga.Vozac)
            {
                IEnumerable<Models.Voznja> voznje = new List<Models.Voznja>();
                voznje = Baza.GetVoznjeVozac(LoggedIn);
                List<Models.Voznja> voznje2 = new List<Models.Voznja>();

                foreach(Voznja v in voznje)
                {
                    if((int)v.Status_Voznje == status)
                    {
                        voznje2.Add(v);
                    }

                }

                ViewBag.Voznje = voznje2;
                

                List<Models.Komentar> komentari = new List<Models.Komentar>();
                komentari = Baza.GetKomentareZaVoznje();
                ViewBag.Komentari = komentari;

                List<Models.Voznja> voznjesve = new List<Models.Voznja>();
                voznjesve = Baza.GetVoznjeSveKreirane();
                ViewBag.VoznjeSve = voznjesve;

            }
            return View("Index");



        }

        [Route("Filtriraj2")]
        public ActionResult Filtriraj2()
        {
            Projekat.Models.Korisnik korisnik = new Models.Korisnik();
            korisnik = Baza.GetUser(LoggedIn);
            ViewBag.User = korisnik;

            int status = Int32.Parse(Request.Params["status"]);


            if (korisnik.Uloga_Korisnika == Models.Korisnik.Uloga.Musterija)
            {
                IEnumerable<Models.Voznja> voznje = new List<Models.Voznja>();
                voznje = Baza.GetVoznjeMusterija(korisnik.Korisnicko_Ime);
                List<Models.Voznja> voznje2 = new List<Models.Voznja>();

                foreach (Voznja v in voznje)
                {
                    if ((int)v.Status_Voznje == status)
                    {
                        voznje2.Add(v);
                    }

                }

                ViewBag.Voznje = voznje2;


                List<Models.Komentar> komentari = new List<Models.Komentar>();
                komentari = Baza.GetKomentareZaVoznje();
                ViewBag.Komentari = komentari;

               

            }
            return View("Index");



        }

        [Route("Filtriraj3")]
        public ActionResult Filtriraj3()
        {
            Projekat.Models.Korisnik korisnik = new Models.Korisnik();
            korisnik = Baza.GetUser(LoggedIn);
            ViewBag.User = korisnik;

            int status = Int32.Parse(Request.Params["status"]);


            if (korisnik.Uloga_Korisnika == Models.Korisnik.Uloga.Dispecer)
            {
                IEnumerable<Models.Voznja> voznje = new List<Models.Voznja>();
                voznje = Baza.GetVoznjeDispecer(korisnik.Korisnicko_Ime);
                List<Models.Voznja> voznje2 = new List<Models.Voznja>();

                foreach (Voznja v in voznje)
                {
                    if ((int)v.Status_Voznje == status)
                    {
                        voznje2.Add(v);
                    }

                }

                ViewBag.Voznje = voznje2;


                List<Models.Voznja> voznjesve = new List<Models.Voznja>();
                voznjesve = Baza.GetVoznjeSve();
                ViewBag.VoznjeSve = voznjesve;

                List<Models.Komentar> komentari = new List<Models.Komentar>();
                komentari = Baza.GetKomentareZaVoznje();
                ViewBag.Komentari = komentari;


            }
            return View("Index");



        }

        [Route("Filtriraj4")]
        public ActionResult Filtriraj4()
        {
            Projekat.Models.Korisnik korisnik = new Models.Korisnik();
            korisnik = Baza.GetUser(LoggedIn);
            ViewBag.User = korisnik;

            int status = Int32.Parse(Request.Params["status"]);


            if (korisnik.Uloga_Korisnika == Models.Korisnik.Uloga.Dispecer)
            {
                IEnumerable<Models.Voznja> voznje = new List<Models.Voznja>();
                voznje = Baza.GetVoznjeSve();
                List<Models.Voznja> voznje2 = new List<Models.Voznja>();

               // var newList = people.OrderBy(x => x.LastName)
                foreach (Voznja v in voznje)
                {
                    if ((int)v.Status_Voznje == status)
                    {
                        voznje2.Add(v);
                    }

                }

                ViewBag.VoznjeSve = voznje2;


                IEnumerable<Models.Voznja> voznjee = new List<Models.Voznja>();
                voznjee = Baza.GetVoznjeDispecer(LoggedIn);
                ViewBag.Voznje = voznjee;

                List<Models.Komentar> komentari = new List<Models.Komentar>();
                komentari = Baza.GetKomentareZaVoznje();
                ViewBag.Komentari = komentari;


            }
            return View("Index");



        }

        [Route("Sort1")]
        public ActionResult Sort1()
        {
            Projekat.Models.Korisnik korisnik = new Models.Korisnik();
            korisnik = Baza.GetUser(LoggedIn);
            ViewBag.User = korisnik;

            int status = Int32.Parse( Request.Params["sort"]);
            
            if (korisnik.Uloga_Korisnika == Models.Korisnik.Uloga.Dispecer)
            {
                if (status == 1)
                {
                    IEnumerable<Models.Voznja> voznje = new List<Models.Voznja>();
                    IEnumerable<Models.Voznja> help = new List<Models.Voznja>();

                    voznje = Baza.GetVoznjeDispecer(LoggedIn);
                    help = voznje.OrderByDescending(x => x.Datum_Vreme).ToList();

                    ViewBag.Voznje = help;

                    List<Models.Voznja> voznjesve = new List<Models.Voznja>();
                    List<Models.Voznja> help2 = new List<Models.Voznja>();

                    voznjesve = Baza.GetVoznjeSve();
                    help2 = voznjesve.OrderByDescending(x => x.Datum_Vreme).ToList();
                    ViewBag.VoznjeSve = help2;

                    List<Models.Komentar> komentari = new List<Models.Komentar>();
                    komentari = Baza.GetKomentareZaVoznje();
                    ViewBag.Komentari = komentari;

                    return View("Index");
                }

            }
            return View("Index");



        }

        [Route("Sort2")]
        public ActionResult Sort2()
        {
            Projekat.Models.Korisnik korisnik = new Models.Korisnik();
            korisnik = Baza.GetUser(LoggedIn);
            ViewBag.User = korisnik;

            int status = Int32.Parse(Request.Params["sort"]);

            if (korisnik.Uloga_Korisnika == Models.Korisnik.Uloga.Musterija)
            {
                if (status == 1)
                {
                    IEnumerable<Models.Voznja> voznje = new List<Models.Voznja>();
                    IEnumerable<Models.Voznja> help = new List<Models.Voznja>();

                    voznje = Baza.GetVoznjeMusterija(LoggedIn);
                    help = voznje.OrderByDescending(x => x.Datum_Vreme).ToList();

                    ViewBag.Voznje = help;

                    

                    List<Models.Komentar> komentari = new List<Models.Komentar>();
                    komentari = Baza.GetKomentareZaVoznje();
                    ViewBag.Komentari = komentari;

                    return View("Index");
                }

            }
            return View("Index");



        }

        [Route("Sort3")]
        public ActionResult Sort3()
        {
            Projekat.Models.Korisnik korisnik = new Models.Korisnik();
            korisnik = Baza.GetUser(LoggedIn);
            ViewBag.User = korisnik;

            int status = Int32.Parse(Request.Params["sort"]);

            if (korisnik.Uloga_Korisnika == Models.Korisnik.Uloga.Vozac)
            {
                if (status == 1)
                {
                    IEnumerable<Models.Voznja> voznje = new List<Models.Voznja>();
                    IEnumerable<Models.Voznja> help = new List<Models.Voznja>();

                    voznje = Baza.GetVoznjeVozac(LoggedIn);
                    help = voznje.OrderByDescending(x => x.Datum_Vreme).ToList();

                    ViewBag.Voznje = help;

                    List<Models.Voznja> voznjesve = new List<Models.Voznja>();
                    List<Models.Voznja> help2 = new List<Models.Voznja>();

                    voznjesve = Baza.GetVoznjeSveKreirane();
                    help2 = voznjesve.OrderByDescending(x => x.Datum_Vreme).ToList();
                    ViewBag.VoznjeSve = help2;

                    List<Models.Komentar> komentari = new List<Models.Komentar>();
                    komentari = Baza.GetKomentareZaVoznje();
                    ViewBag.Komentari = komentari;

                    return View("Index");
                }

            }
            return View("Index");



        }
        private string LoggedIn => Request.Cookies[CookieKeys.Login]?.Value;
        private IBaza Baza => (IBaza)HttpContext.Application[ApplicationKeys.Baza];
    }
}