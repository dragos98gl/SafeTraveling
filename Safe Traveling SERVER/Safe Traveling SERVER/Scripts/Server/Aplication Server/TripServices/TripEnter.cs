using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.TripServices
{
    class TripEnter
    {
        public static void Handle(TcpClient Client, NetworkStream ns, string[] Messages, List<Excursie> Excursii)
        {
            //1.NumePrenume
            //2.NumarTelefon
            //3.TripId
            
            string NumePrenume = Messages[1];
            string NumarTelefon = Messages[2];
            string TripId = Messages[3];
            string DestinatiePrincipala = string.Empty;
            string TipulParticipantilor = string.Empty;
            string DataPlecare = string.Empty;
            string DataIntoarcere = string.Empty;
            string LocatiePlecare = string.Empty;
            string OraPlecare = string.Empty;
            string Observatii = string.Empty;

            string Response = (0).ToString();
            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Excursii WHERE [UniqueId] = '" + TripId + "'",Database);
                SqlDataReader Reader = cmd.ExecuteReader();
                
                if (Reader.Read())
                {
                    DestinatiePrincipala = (string)Reader[0];
                    TipulParticipantilor = (string)Reader[1];
                    DataPlecare = (string)Reader[2];
                    DataIntoarcere = (string)Reader[3];
                    LocatiePlecare = (string)Reader[4];
                    OraPlecare = (string)Reader[5];
                    Observatii = (string)Reader[6];

                    Response = (1).ToString();
                }
            }
            
            if (Response.Equals((1).ToString()))
                _Utils.WriteStreamString(ns,CryptDecryptData.CryptData(new string[] { Response, DestinatiePrincipala, TipulParticipantilor, DataPlecare, DataIntoarcere, LocatiePlecare, OraPlecare, Observatii }));
            else
                _Utils.WriteStreamString(ns,CryptDecryptData.CryptData(new string[] {Response}));

            foreach (Excursie e in Excursii)
                if (e.UniqueId == TripId)
                {
                    e.AdaugareUtilizatorNou(new Client(Client, ns), NumarTelefon);
                }
        }
    }
}
