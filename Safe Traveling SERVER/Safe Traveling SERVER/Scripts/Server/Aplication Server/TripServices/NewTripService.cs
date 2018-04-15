using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LS;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NewTrip
{
    class NewTripService
    {
        public static void Handle(TcpClient Client, NetworkStream ns, string[] Messages,LocationService LocationService,NotificationService NotificationService,List<Excursie> Excursii)
        {
            //1.Destinatia principala
            //2.Tipul participantilor
            //3.Data plecare
            //4.Data intoarcere
            //5.Locatie plecare
            //6.Ora plecare
            //7.Observatii
            //8.Nume organizator
            //9.Numar telefon

            string
                DestinatiePrincipala = Messages[1],
                TipulParticipantilor = Messages[2],
                DataPlecare = Messages[3],
                DataIntoarcere = Messages[4],
                LocatiePlecare = Messages[5],
                OraPlecare = Messages[6],
                Observatii = Messages[7],
                NumeOrganizator = Messages[8],
                NumarTelefon = Messages[9];

            string UniqueId = GenerateUniqueTripId();
            string URL = CreateTripURL(NumeOrganizator, NumarTelefon, UniqueId);

            string Response = (0).ToString();
            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT [Nume],[Prenume] FROM Utilizatori WHERE [NumarTelefon]='" + NumarTelefon + "'", Database);
                {
                    using (SqlDataReader Reader = cmd.ExecuteReader())
                        if (Reader.Read())
                            NumeOrganizator = (string)Reader[0] + " " + (string)Reader[1];
                }

                cmd = new SqlCommand(@"INSERT INTO Excursii ([Destinatie Principala],[Tipul Participantilor],[Data Plecare],[Data Intoarcere],[Locatie Plecare],[Ora Plecare],[Observatii],[Nume Organizator],[Numar Telefon],[UniqueId],[URL]) " +
                "VALUES ('" + DestinatiePrincipala + "','" +
                TipulParticipantilor + "','" +
                DataPlecare + "','" +
                DataIntoarcere + "','" +
                LocatiePlecare + "','" +
                OraPlecare + "','" +
                Observatii + "','" +
                NumeOrganizator + "','" +
                NumarTelefon + "','" +
                UniqueId + "','" +
                URL + "')", Database);

                cmd.ExecuteNonQuery();

                Excursie Excursie = new Excursie(LocationService, NotificationService, DestinatiePrincipala, TipulParticipantilor, DataPlecare, DataIntoarcere, LocatiePlecare, OraPlecare, Observatii, NumeOrganizator, NumarTelefon, UniqueId, URL);
                Excursii.Add(Excursie);

                Response = (1).ToString();
            }
            _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { Response }));

            ns.Close();
            Client.Close();
        }

        private static string CreateTripURL(string NumeUtilizator, string NumarTelefon, string UniqueId)
        {
            string URL = string.Empty;
            string ExecutionURL = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string Suffix = "/SafeTraveling/Trips/" + NumeUtilizator + NumarTelefon + "_" + UniqueId;

            URL = ExecutionURL + Suffix;

            Directory.CreateDirectory(URL);
            Directory.CreateDirectory(URL+"/Galerie");

            return URL;
        }

        private static string GenerateUniqueTripId()
        {
            int[] NumList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string[] WordList = new string[] { "x", "y", "z", "a", "b", "c" };
            string UniqueId = string.Empty;

            Random rand = new Random();

            for (int i = 0; i < 10; i++)
            {
                switch (rand.Next(0, 2))
                {
                    case 0:
                        {
                            UniqueId = UniqueId + NumList[rand.Next(0, NumList.Length)].ToString();
                        } break;
                    case 1:
                        {
                            UniqueId = UniqueId + WordList[rand.Next(0, WordList.Length)];
                        } break;
                }
            }

            return UniqueId;
        }
    }
}