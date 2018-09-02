using Projekat.Baza;
using Projekat.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace Projekat.Controllers
{
    public class GateController : Controller
    {
        [Route("Login")]
        public ActionResult Login()
        {
            if (Request.HttpMethod == "GET")
                return View();

            var username = Request.Params["username"];
            var password = Request.Params["password"];
            if (Baza.CanLogIn(username, password))
            {
                Response.Cookies.Add(new HttpCookie(CookieKeys.Login)
                {
                    Value = username,
                    Expires = DateTime.Now.AddHours(1)
                });
                ViewBag.Title = "Login Successful";
                ViewBag.Theme = "tema";
            }
            else
            {
                ViewBag.Title = "Login Failed";
            }
            
            return View("LoginResult");
        }

        [Route("Register")]
        public ActionResult Register()
        {
            if (Request.Cookies[CookieKeys.Login] != null)
                return View("AlreadyLoggedIn");

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
                Uloga_Korisnika = Models.Korisnik.Uloga.Musterija
                
            };

            

            if (Baza.Register(user))
                ViewBag.Title = "Registration Successful";
            else
                ViewBag.Title = "Registration Failure";

            return View("RegisterResult");
        }

        [Route("Logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Add(new HttpCookie(CookieKeys.Login)
            {
                Value = null,
                Expires = DateTime.Now.AddDays(-1)
            });
            
            return View();
        }



        private IBaza Baza => (IBaza) HttpContext.Application[ApplicationKeys.Baza];
    }
}