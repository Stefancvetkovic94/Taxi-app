using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.Ajax.Utilities;

namespace Projekat.Baza
{
    public class Baza : IBaza
    {
        public Baza(SQLiteConnection conn)
        {
            _conn = conn;
        }

        // User related



        public bool CanLogIn(string username, string password)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT COUNT()
                      FROM Korisnici
                      WHERE Korisnicko_Ime = @name AND Lozinka = @password
                      LIMIT 1;";
                cmd.Parameters.AddWithValue("@name", username);
                cmd.Parameters.AddWithValue("@password", password);

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();

                    return reader.GetInt32(0) == 1;
                }
            }
        }

        public bool Register(Korisnik user)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"INSERT INTO Korisnici(Korisnicko_Ime, Lozinka, Ime, Prezime, Uloga_Korisnika, Pol, JMBG, Telefon, Email, Lokacija, Automobil)
                      VALUES (@name, @password, @firstname, @lastname, @role ,@pol, @jmbg, @phoneno, @email, NULL, NULL);";
                cmd.Parameters.AddWithValue("@name", user.Korisnicko_Ime);
                cmd.Parameters.AddWithValue("@password", user.Lozinka);
                cmd.Parameters.AddWithValue("@firstname", user.Ime);
                cmd.Parameters.AddWithValue("@lastname", user.Prezime);
                cmd.Parameters.AddWithValue("@role", (int)user.Uloga_Korisnika);
                cmd.Parameters.AddWithValue("@pol", user.Pol);
                cmd.Parameters.AddWithValue("@jmbg", user.JMBG);
                cmd.Parameters.AddWithValue("@phoneno", user.Telefon);
                cmd.Parameters.AddWithValue("@email", user.Email);

                return Execute(cmd);
            }
        }


        public Korisnik GetUser(string username)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT Ime, Prezime, Uloga_Korisnika, Pol, Email, JMBG, Telefon, Lokacija, Automobil
                      FROM Korisnici
                      WHERE Korisnicko_Ime = @name
                      LIMIT 1;";
                cmd.Parameters.AddWithValue("@name", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    Korisnik k = new Korisnik();
                    k.Korisnicko_Ime = username;
                    k.Ime = reader.GetString(0);
                    k.Prezime = reader.GetString(1);
                    k.Uloga_Korisnika = (Korisnik.Uloga)reader.GetInt32(2);
                    k.Telefon = reader.GetString(6);
                    k.Email = reader.GetString(4);
                    if (reader.GetString(7) != "0")
                        k.Lokacija = reader.GetString(7);
                    if (reader.GetString(8) != "0")
                        k.Automobil = reader.GetString(8);

                    return k;

                  /*  return new Korisnik()
                    {
                        Korisnicko_Ime = username,
                        Ime = reader.GetString(0),
                        Prezime = reader.GetString(1),
                        Uloga_Korisnika = (Korisnik.Uloga)reader.GetInt32(2),
                        Telefon = reader.GetString(6),
                        Email = reader.GetString(4),
                        Pol = reader.GetString(3),
                        JMBG = reader.GetString(5),
                        
                        Lokacija = reader.GetString(7)
                        //Automobil= reader.GetString(8)


                    };
                    */
                }
            }
        }




        private readonly SQLiteConnection _conn;

        private static bool Execute(SQLiteCommand cmd)
        {
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException)
            {
                return false;
            }

            return true;
        }

        bool IBaza.EditUser(string korisnickoime, string ime, string prezime, string telefon, string pol, string jmbg, string email)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Korisnici
                      SET Ime = @ime, Prezime = @prezime, Pol = @pol, JMBG = @jmbg, Telefon = @telefon, Email = @email
                      WHERE Korisnicko_Ime = @korisnickoime";
                cmd.Parameters.AddWithValue("@ime", ime);
                cmd.Parameters.AddWithValue("@prezime", prezime);
                cmd.Parameters.AddWithValue("@telefon", telefon);
                cmd.Parameters.AddWithValue("@pol", pol);
                cmd.Parameters.AddWithValue("@jmbg", jmbg);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@korisnickoime", korisnickoime);

                return Execute(cmd);
            }
        }

        public bool AddLocation(Lokacija lokacija)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"INSERT INTO Lokacije(Adresa, X_kordinata, Y_kordinata)
                      VALUES (@adresa, @x, @y);";
                cmd.Parameters.AddWithValue("@adresa", lokacija.Adresa);
                cmd.Parameters.AddWithValue("@x", lokacija.X_kordinata);
                cmd.Parameters.AddWithValue("@y", lokacija.Y_kordinata);
                

                return Execute(cmd);
            }
        }

        public bool AddAdress(string adresa)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"INSERT INTO Adrese(Adresa_Ulice)
                      VALUES (@adresa);";
                cmd.Parameters.AddWithValue("@adresa", adresa);
                


                return Execute(cmd);
            }
        }

        public bool UpdateLocationInKorisnik(string korisnik, string lokacija)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Korisnici
                      SET Lokacija = @lokacija
                      WHERE Korisnicko_Ime = @korisnik";
                cmd.Parameters.AddWithValue("@lokacija", lokacija);
                cmd.Parameters.AddWithValue("@korisnik", korisnik);
                

                return Execute(cmd);
            }
        }

        public bool AddVoznja(Voznja voznja)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"INSERT INTO Voznje(Id, Datum_Vreme, Lokacija, Tip, Musterija, Status_Voznje)
                      VALUES (NULL, @datum, @lokacija, @tip, @musterija ,@status);";
                cmd.Parameters.AddWithValue("@datum", voznja.Datum_Vreme.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@lokacija", voznja.Lokacija);
                if(voznja.Tip== "Putnicki")
                {
                    cmd.Parameters.AddWithValue("@tip", (int)Models.Automobil.Tip.Putnicki);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@tip", (int)Models.Automobil.Tip.Kombi);

                }

                cmd.Parameters.AddWithValue("@musterija", voznja.Musterija);
                cmd.Parameters.AddWithValue("@status", (int)voznja.Status_Voznje);
                

                return Execute(cmd);
            }
        }

        public IEnumerable<Voznja> GetVoznjeMusterija(string musterija)
        {
            var list = new List<Voznja>();

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT Id, Datum_Vreme, Lokacija, Tip, Musterija, Odrediste, Dispecer, Vozac, Iznos, Komentar, Status_Voznje
                      FROM Voznje
                      WHERE Musterija = @name;";
                cmd.Parameters.AddWithValue("@name", musterija);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tip = string.Empty;
                        if (reader.GetInt32(3) == 0)
                        {
                             tip = "Putnicki";
                        }
                        else
                        {
                             tip = "Kombi";
                        }

                        list.Add(new Voznja()
                        {
                            Id = reader.GetInt32(0),
                            Datum_Vreme = reader.GetDateTime(1),
                            Lokacija = reader.GetString(2),
                            Musterija = reader.GetString(4),
                            Odrediste = reader.GetString(5),
                            Dispecer = reader.GetString(6),
                            Vozac = reader.GetString(7),
                            Iznos = reader.GetInt32(8),
                            Komentar = reader.GetString(9),
                            Status_Voznje = (Voznja.Status)reader.GetInt32(10),
                            Tip = tip

                        });
                    }
                }
            }

            return list;
        }
    }
}