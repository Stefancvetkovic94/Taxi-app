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
                    @"INSERT INTO Lokacije(Id, Adresa, X_kordinata, Y_kordinata)
                      VALUES (NULL, @adresa, @x, @y);";
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
                    @"INSERT INTO Adrese(Id, Adresa_Ulice)
                      VALUES (NULL, @adresa);";
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

        public bool AddVoznjaMusterija(Voznja voznja)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"INSERT INTO Voznje(Id, Datum_Vreme, Lokacija, Tip, Musterija, Status_Voznje)
                      VALUES (NULL, @datum, @lokacija, @tip, @musterija ,@status);";
                cmd.Parameters.AddWithValue("@datum", voznja.Datum_Vreme.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@lokacija", voznja.Lokacija);
                if (voznja.Tip == "Putnicki")
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

        public bool OtkaziVoznju(int id)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Voznje
                      SET Status_Voznje = @status
                      WHERE Id = @id";
                cmd.Parameters.AddWithValue("@status", (int)Voznja.Status.Otkazana);
                cmd.Parameters.AddWithValue("@id", id);


                return Execute(cmd);
            }
        }

        public bool AddComment(Komentar komentar)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"INSERT INTO Komentari(Id, Opis, Datum, Korisnik, Voznja, Ocena)
                      VALUES (NULL, @opis, @datum, @korisnik, @voznja, @ocena);";
                cmd.Parameters.AddWithValue("@opis", komentar.Opis);
                cmd.Parameters.AddWithValue("@datum", komentar.Datum);
                cmd.Parameters.AddWithValue("@voznja", komentar.Voznja);
                cmd.Parameters.AddWithValue("@ocena", komentar.Ocena);

                cmd.Parameters.AddWithValue("@korisnik", komentar.Korisnik);


                return Execute(cmd);
            }
        }

        public bool AddCommentToVoznja(int id, string komentar)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Voznje
                      SET Komentar = @komentar
                      WHERE Id = @id";
                cmd.Parameters.AddWithValue("@komentar", komentar);
                cmd.Parameters.AddWithValue("@id", id);


                return Execute(cmd);
            }
        }

        public bool IzmeniVoznju(string lokacija, string tip, string id)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Voznje
                      SET Lokacija = @lokacija, Tip = @tip
                      WHERE Id = @id";
                cmd.Parameters.AddWithValue("@lokacija", lokacija);
                if (tip == "Putnicki")
                {
                    cmd.Parameters.AddWithValue("@tip", (int)Models.Automobil.Tip.Putnicki);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@tip", (int)Models.Automobil.Tip.Kombi);

                }
                cmd.Parameters.AddWithValue("@id", id);


                return Execute(cmd);
            }
        }

        public IEnumerable<string> GetVozaci()
        {
            var list = new List<string>();

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT Korisnicko_Ime
                      FROM Korisnici
                      WHERE Uloga_Korisnika = @uloga AND Slobodan= 1;";
                cmd.Parameters.AddWithValue("@uloga", 2);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string vozac = string.Empty;
                        vozac = reader.GetString(0);

                        list.Add(vozac);
                    }
                }
            }

            return list;
        }

        public bool AddVoznjaDispecer(Voznja voznja)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"INSERT INTO Voznje(Id, Datum_Vreme, Lokacija, Tip, Dispecer, Status_Voznje, Vozac)
                      VALUES (NULL, @datum, @lokacija, @tip, @dispecer,@status, @vozac);";
                cmd.Parameters.AddWithValue("@datum", voznja.Datum_Vreme.ToString());
                cmd.Parameters.AddWithValue("@lokacija", voznja.Lokacija);
                if (voznja.Tip == "Putnicki")
                {
                    cmd.Parameters.AddWithValue("@tip", (int)Models.Automobil.Tip.Putnicki);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@tip", (int)Models.Automobil.Tip.Kombi);

                }

                cmd.Parameters.AddWithValue("@dispecer", voznja.Dispecer);
                cmd.Parameters.AddWithValue("@status", (int)voznja.Status_Voznje);
                cmd.Parameters.AddWithValue("@vozac", voznja.Vozac);



                return Execute(cmd);
            }
        }

        public List<Komentar> GetKomentareZaVoznje()
        {
            var list = new List<Komentar>();

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT Opis, Datum, Korisnik, Ocena, Voznja
                      FROM Komentari
                      WHERE Id>=0";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {


                        list.Add(new Komentar()
                        {
                            Opis = reader.GetString(0),
                            Datum = reader.GetDateTime(1),
                            Korisnik = reader.GetString(2),
                            Ocena = reader.GetInt32(3),
                            Voznja = reader.GetInt32(4)

                        });
                    }
                }
            }

            return list;
        }

        public bool VozacZauzet(string ime)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Korisnici
                      SET Slobodan=@slobodan
                      WHERE Korisnicko_Ime = @korisnickoime";
                cmd.Parameters.AddWithValue("@korisnickoime", ime);
                cmd.Parameters.AddWithValue("@slobodan", 0);

                return Execute(cmd);
            }
        }

        public bool VozacSlobodan(string ime)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Korisnici
                      SET Slobodan=@slobodan
                      WHERE Korisnicko_Ime = @korisnickoime";
                cmd.Parameters.AddWithValue("@korisnickoime", ime);
                cmd.Parameters.AddWithValue("@slobodan", 1);

                return Execute(cmd);
            }
        }

        public IEnumerable<Voznja> GetVoznjeDispecer(string musterija)
        {
            var list = new List<Voznja>();

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT Id, Datum_Vreme, Lokacija, Tip, Musterija, Odrediste, Dispecer, Vozac, Iznos, Komentar, Status_Voznje
                      FROM Voznje
                      WHERE Dispecer = @name;";
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
                            Datum_Vreme = DateTime.Now,
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

        public List<Voznja> GetVoznjeSve()
        {
            var list = new List<Voznja>();

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT Id, Datum_Vreme, Lokacija, Tip, Musterija, Odrediste, Dispecer, Vozac, Iznos, Komentar, Status_Voznje
                      FROM Voznje
                      WHERE Id>=0;";

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
                            Datum_Vreme = DateTime.Now,
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

        public bool DodeliVozacaVoznji(string id, string vozac, string dispecer)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Voznje
                      SET Vozac = @vozac, Dispecer=@dispecer, Status_Voznje = @status
                      WHERE Id = @id";
                cmd.Parameters.AddWithValue("@vozac", vozac);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@dispecer", dispecer);
                cmd.Parameters.AddWithValue("@status", (int)Models.Voznja.Status.Obradjena);

                
                return Execute(cmd);
            }
        }

        public IEnumerable<Voznja> GetVoznjeVozac(string musterija)
        {
            var list = new List<Voznja>();

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT Id, Datum_Vreme, Lokacija, Tip, Musterija, Odrediste, Dispecer, Vozac, Iznos, Komentar, Status_Voznje
                      FROM Voznje
                      WHERE Vozac = @name;";
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
                            Datum_Vreme = DateTime.Now,
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

        public List<Voznja> GetVoznjeSveKreirane()
        {
            var list = new List<Voznja>();

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT Id, Datum_Vreme, Lokacija, Tip, Musterija, Odrediste, Dispecer, Vozac, Iznos, Komentar, Status_Voznje
                      FROM Voznje
                      WHERE Status_Voznje=0;";

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
                            Datum_Vreme = DateTime.Now,
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

        public bool PrihvatiVoznju(int id, string vozac)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Voznje
                      SET Vozac = @vozac, Status_Voznje = @status
                      WHERE Id = @id";
                cmd.Parameters.AddWithValue("@vozac", vozac);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@status", (int)Models.Voznja.Status.Prihvacena);


                return Execute(cmd);
            }
        }

        public bool VoznjaNeuspesna(int id)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Voznje
                      SET Status_Voznje = @status
                      WHERE Id = @id";
                cmd.Parameters.AddWithValue("@status", (int)Voznja.Status.Neuspesna);
                cmd.Parameters.AddWithValue("@id", id);


                return Execute(cmd);
            }
        }

        public bool VoznjaUspesna(int id, string odrediste, int iznos)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    @"UPDATE Voznje
                      SET Status_Voznje = @status, Odrediste= @odrediste, Iznos=@iznos
                      WHERE Id = @id";
                cmd.Parameters.AddWithValue("@status", (int)Voznja.Status.Uspesna);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@odrediste", odrediste);
                cmd.Parameters.AddWithValue("@iznos", iznos);


                return Execute(cmd);
            }
        }
    }
}
    
