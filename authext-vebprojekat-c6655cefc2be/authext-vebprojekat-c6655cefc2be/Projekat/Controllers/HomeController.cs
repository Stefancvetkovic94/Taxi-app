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

       




        private string LoggedIn => Request.Cookies[CookieKeys.Login]?.Value;
        private IBaza Baza => (IBaza)HttpContext.Application[ApplicationKeys.Baza];
    }
}