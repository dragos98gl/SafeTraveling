using Safe_Traveling_SERVER.Scripts.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Website
{
    class WebService
    {
        const int SleepTime = 300;
        TcpListener host;

        public void Start(TcpListener host)
        {
            this.host = host;

            new Thread(new ThreadStart(LoadClients)).Start();
        }

        private void LoadClients()
        {
            while (true)
            {
                if (host.Pending())
                    new Thread(new ParameterizedThreadStart(ResolveRequest)).Start(host.AcceptTcpClient());
                Thread.Sleep(SleepTime);
            }
        }

        private void ResolveRequest(object newClient)
        {
            TcpClient client = (TcpClient)newClient;
            NetworkStream ns = client.GetStream();

            StreamReader r = new StreamReader(new BufferedStream(ns));
            StreamWriter w = new StreamWriter(new BufferedStream(ns));
            BinaryWriter bw = new BinaryWriter(new BufferedStream(ns));
            string URL = string.Empty;

            string line = r.ReadLine();
            try
            {
                URL = line.Substring(5, line.IndexOf("HTTP") - 6);

                if ((URL.Contains("?username=")) && (URL.Contains("&password=")))
                {
                    string Username = URL.Substring(URL.IndexOf("?username=") + "?username=".Length, URL.IndexOf("&password=") - "&password".Length - 1);
                    string Password = URL.Substring(URL.IndexOf("&password=") + "&password=".Length, URL.Length - URL.IndexOf("&password=") - "&password=".Length);

                    using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                    {
                        Database.Open();

                        SqlCommand cmd = new SqlCommand("SELECT [LastKnowPosition] FROM Utilizatori WHERE [NumarTelefon]='" + Username + "' AND [Parola]='" + Password + "'", Database);
                        SqlDataReader Reader = cmd.ExecuteReader();

                        if (Reader.Read())
                        {
                            w.Write(Location((string)Reader[0]));
                            w.Close();
                        }
                        else
                        {
                            w.Write(Home());
                            w.Close();
                        }
                    }
                }
                else if (URL.Contains("?merch_usr=") && URL.Contains("&merch_pw="))
                {
                    string Password = URL.Substring(URL.IndexOf("&merch_pw=") + "&merch_pw=".Length, URL.Length - URL.IndexOf("&merch_pw=") - "&merch_pw=".Length);
                    string Username = URL.Substring(URL.IndexOf("?merch_usr=") + "?merch_usr=".Length, URL.IndexOf("&merch_pw=") - (URL.IndexOf("?merch_usr=") + "?merch_usr=".Length));
                    Username = Username.Replace("%40", "@");

                    using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                    {
                        Database.Open();

                        SqlCommand cmd = new SqlCommand("SELECT * FROM BeSmart_Magazine WHERE [Email]='" + Username + "' AND [Parola]='" + Password + "'", Database);
                        SqlDataReader Reader = cmd.ExecuteReader();

                        if (Reader.Read())
                        {
                            w.Write(MerchandAddProduct());
                            w.Close();
                        }
                        else
                        {
                            w.Write(MerchantMain());
                            w.Close();
                        }
                    }
                }
                else if (URL.Contains("?nume=") && URL.Contains("&locatie="))
                {
                    string Nume = URL.Substring(URL.IndexOf("?nume=") + "?nume=".Length, URL.IndexOf("&nume_firma=")-(URL.IndexOf("?nume=") + "?nume=".Length));
                    string NumeFirma = URL.Substring(URL.IndexOf("&nume_firma=") + "&nume_firma=".Length, URL.IndexOf("&prenume=") - (URL.IndexOf("&nume_firma=") + "&nume_firma=".Length));
                    string Prenume = URL.Substring(URL.IndexOf("&prenume=") + "&prenume=".Length, URL.IndexOf("&tipfirma=") - (URL.IndexOf("&prenume=") + "&prenume=".Length));
                    string TipFirma = URL.Substring(URL.IndexOf("&tipfirma=") + "&tipfirma=".Length, URL.IndexOf("&parola=") - (URL.IndexOf("&tipfirma=") + "&tipfirma=".Length));
                    string Parola = URL.Substring(URL.IndexOf("&parola=") + "&parola=".Length, URL.IndexOf("&email=") - (URL.IndexOf("&parola=") + "&parola=".Length));
                    string Email = URL.Substring(URL.IndexOf("&email=") + "&email=".Length, URL.IndexOf("&locatie=") - (URL.IndexOf("&email=") + "&email=".Length));
                    string Locatie = URL.Substring(URL.IndexOf("&locatie=") + "&locatie=".Length, URL.Length - URL.IndexOf("&locatie=") - "&locatie=".Length);

                    using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                    {
                        Database.Open();

                        SqlCommand cmd = new SqlCommand("INSERT INTO BeSmart_Magazine ([NumePatron],[PrenumePatron],[NumeFirma],[Email],[TipFirma],[Locatie],[Parola]) VALUES (@nume,@prenume,@numefirma,@email,@tipfirma,@locatie,@parola)", Database);
                        cmd.Parameters.AddWithValue("nume", Nume.Replace('+',' '));
                        cmd.Parameters.AddWithValue("prenume", Prenume.Replace('+', ' '));
                        cmd.Parameters.AddWithValue("numefirma", NumeFirma.Replace('+', ' '));
                        cmd.Parameters.AddWithValue("email", Email.Replace('+', ' ').Replace("%40","@"));
                        cmd.Parameters.AddWithValue("tipfirma", TipFirma.Replace('+', ' '));
                        cmd.Parameters.AddWithValue("locatie", Locatie.Replace('+', ' '));
                        cmd.Parameters.AddWithValue("parola", Parola.Replace('+', ' '));

                        cmd.ExecuteNonQuery();

                        w.Write(Home());
                        w.Close();
                    }
                }
                else
                {
                    switch (URL)
                    {
                        case "merchantmain":
                            {
                                w.Write(MerchantMain());
                                w.Close();
                            } break;
                        case "":
                            {
                                w.Write(Home());
                                w.Close();
                            } break;
                    }
                }
            }
            catch
            {
                w.Write("err,please refresh");
                w.Close();
            }
        }

        private string MerchandAddProduct()
        {
            string Response = "Error";

            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT [Merchant_AddProduct] FROM WebsiteFiles", Database);
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.Read())
                    Response = (string)Reader[0];
            }

            return Response;
        }

        private string MerchantMain()
        {
            string Response = "Error";

            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT [MerchantPage] FROM WebsiteFiles", Database);
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.Read())
                    Response = (string)Reader[0];
            }

            return Response;
        }

        private string Home()
        {
            string Response = "Error";

            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT [LoginPage] FROM WebsiteFiles", Database);
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.Read())
                    Response = (string)Reader[0];
            }

            return Response;
        }

        private string Location(string Location)
        {
            string Response = "Error";

            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT [ShowPostion] FROM WebsiteFiles", Database);
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.Read())
                    Response = (string)Reader[0] + Location;

                if (Reader.Read())
                    Response += (string)Reader[0];
            }

            return Response;
        }
    }
}
