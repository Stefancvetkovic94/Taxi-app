using System;
using Projekat.Models;
using System.Collections.Generic;

namespace Projekat.Baza
{
    public interface IBaza
    {
     
        bool CanLogIn(string username, string password);
        bool Register(Korisnik user);

        bool AddLocation(Lokacija lokacija);
        bool AddAdress(string adresa);
        bool AddVoznjaMusterija(Voznja voznja);
        bool AddVoznjaDispecer(Voznja voznja);
        bool AddComment(Komentar komentar);
        bool AddCommentToVoznja(int id, string komentar);

        bool UpdateLocationInKorisnik(string korisnik, string lokacija);
        bool EditUser(string korisnickoime, string ime, string prezime, string telefon, string pol, string jmbg, string email);
        bool IzmeniVoznju(string lokacija, string tip, string id);
        bool DodeliVozacaVoznji(string id, string vozac, string dispecer);

        bool OtkaziVoznju(int id);


        Korisnik GetUser(string username);
        IEnumerable<Voznja> GetVoznjeMusterija(string musterija);
        IEnumerable<Voznja> GetVoznjeDispecer(string musterija);
        List<Voznja> GetVoznjeSve();
        IEnumerable<string> GetVozaci();
        List<Komentar> GetKomentareZaVoznje();

        bool VozacZauzet(string ime);
        bool VozacSlobodan(string ime);


    }
}