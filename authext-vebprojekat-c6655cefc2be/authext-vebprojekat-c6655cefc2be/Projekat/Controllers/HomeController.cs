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
                ViewBag.User = Baza.GetUser(LoggedIn);
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