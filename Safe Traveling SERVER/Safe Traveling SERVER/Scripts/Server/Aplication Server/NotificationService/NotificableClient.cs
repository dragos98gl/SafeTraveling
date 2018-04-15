using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LS;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.TripServices;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS
{
    class NotificableClient
    {
        public string NumarTelefon;
        public string TripId;
        public Thread SendRealtimeLocation;
        public string TipCont;
        LocationableClient LocationableMe;

        Client Client;
        Excursie e;

        public NotificableClient(Client c, string NrTel, Excursie e,string TripId)
        {
            this.Client = c;
            this.NumarTelefon = NrTel;
            this.TripId = TripId;
            this.e = e;

            Thread.Sleep(1000);
            while (LocationableMe == null)
            {
                foreach (LocationableClient lc in e.LocationableClients)
                    if (lc.NumarTelefon == NrTel)
                    {
                        LocationableMe = lc;
                        break;
                    }
                Thread.Sleep(1000);
            }

            TipCont = GetAccountType(NrTel);

            if (TipCont == "Organizator")
            {
                SendRealtimeLocation = new Thread(new ThreadStart(Sender));
                SendRealtimeLocation.Start();
            }
        }

        private void Sender()
        {
            while(true)
            {
                for (int i = 0; i < e.LocationableClients.Count; i++)
                {
                    if (i != e.LocationableClients.Count - 1)
                    {
                        string Distanta = "not rdy";
                        string Position = string.Empty;

                        try
                        {
                            Distanta = LocationableMe.Position.GetDistance(e.LocationableClients[i].Position.Latitude, e.LocationableClients[i].Position.Longitude);
                            Position = e.LocationableClients[i].Position.GetCoord();
                        }
                        catch { }

                        string[] lcData = e.LocationableClients[i].Data.StreamFile;
                        string[] RegularData = new string[] { _Details.UpdateLocation, "1", lcData[1], lcData[2], lcData[3], lcData[4], lcData[5], Distanta, Position };

                        Send(RegularData);
                    }
                    else
                    {
                        string Distanta = "not rdy";
                        string Position = string.Empty;

                        try
                        {
                        Distanta = LocationableMe.Position.GetDistance(e.LocationableClients[i].Position.Latitude, e.LocationableClients[i].Position.Longitude);
                        Position = e.LocationableClients[i].Position.GetCoord();
                        }
                        catch { }

                        string[] lcData = e.LocationableClients[i].Data.StreamFile;
                        string[] RegularData = new string[] { _Details.UpdateLocation, "0", lcData[1], lcData[2], lcData[3], lcData[4], lcData[5], Distanta, Position};

                        Send(RegularData);
                    }
                }

                Thread.Sleep(3000);
            }
        }

        public void Send(string[] Messages)
        {
            _Utils.WriteStreamString(Client.ns, CryptDecryptData.CryptData(Messages));
        }

        public string[] Receive()
        {
            return CryptDecryptData.DecryptData(_Utils.ReadStreamString(Client.ns));
        }

        public void Dispose()
        {
            if (SendRealtimeLocation != null)
                SendRealtimeLocation.Abort();
        }

        private string GetAccountType(string nrTel)
        {
            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT [TipCont] FROM Utilizatori WHERE [NumarTelefon]='"+nrTel+"'",Database);
                SqlDataReader Reader = cmd.ExecuteReader();

                Reader.Read();

                return (string)Reader[0];
            }
        }
    }
}
