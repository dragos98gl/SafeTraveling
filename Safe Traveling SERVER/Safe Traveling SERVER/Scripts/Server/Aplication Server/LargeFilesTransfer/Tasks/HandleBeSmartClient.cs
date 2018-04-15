using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LargeFilesTransfer.Tasks
{
    static class HandleBeSmartClient
    {
        public static void Handle(string[] Response, NetworkStream ns)
        {
            string Produs = Response[1];
            int Distanta = int.Parse(Response[2]);
            string Username = Response[3];

            string UserPosition = string.Empty;

            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT [LastKnowPosition] FROM Utilizatori WHERE [NumarTelefon]=@UserPhone", Database);
                cmd.Parameters.AddWithValue("UserPhone", Username);

                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.Read())
                    UserPosition = Reader[0] as string;

                Reader.Dispose();

                cmd = new SqlCommand("SELECT [Locatie],[Id_Magazin] FROM BeSmart_Magazine", Database);
                Reader = cmd.ExecuteReader();

                LatitudeLongitude DistanceCalculator = new LatitudeLongitude(UserPosition.Split(',')[0].Replace('.', ','), UserPosition.Split(',')[1].Replace('.', ','));

                List<string> ListaProduse = new List<string>();
                List<string> PozitieProduse = new List<string>();
                List<string> DistantaProduse = new List<string>();
                List<string> PretProduse = new List<string>();

                while (Reader.Read())
                {
                    string PozitieMagazin = Reader[0] as string;
                    string dist = DistanceCalculator.GetDistance(new LatitudeLongitude(PozitieMagazin.Split(',')[0].Replace('.', ','), PozitieMagazin.Split(',')[1].Replace('.', ',')));

                    if (int.Parse(dist) < Distanta)
                    {
                        using (SqlConnection Database2 = new SqlConnection(_Details.DatabaseConnectionString))
                        {
                            Database2.Open();

                            SqlCommand cmd2 = new SqlCommand("SELECT * FROM BeSmart_Produse WHERE [Id_Magazin]=@IdMagazin", Database2);
                            cmd2.Parameters.AddWithValue("IdMagazin", Reader[1] as int?);
                            SqlDataReader Reader2 = cmd2.ExecuteReader();

                            while (Reader2.Read())
                            {
                                string ActualProd = Reader2[4] as string;

                                if (ActualProd.Contains(Produs))
                                {
                                    ListaProduse.Add(ActualProd);
                                    PretProduse.Add(Reader2[5] as string + " lei");
                                    DistantaProduse.Add(dist+"m");
                                    PozitieProduse.Add(PozitieMagazin);
                                }
                            }
                        }
                    }
                }

                _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(ListaProduse.ToArray()));
                _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(PretProduse.ToArray()));
                _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(DistantaProduse.ToArray()));
                _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(PozitieProduse.ToArray()));
            }
        }
    }
}
