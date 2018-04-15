using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LS
{
    class LocationableClient
    {
        public string NumarTelefon;
        public Thread GettingLocationThread;
        public LatitudeLongitude Position;
        public string PositionString;
        public LocationableClientData Data;

        Client Client;
        Excursie e;

        public LocationableClient(Client c, string NrTel, Excursie e)
        {
            this.Client = c;
            this.NumarTelefon = NrTel;
            this.e = e;

            Data = new LocationableClientData(NrTel);
            GettingLocationThread = new Thread(new ThreadStart(Listener));
            GettingLocationThread.Start();
        }

        private void Listener()
        {
            while (true)
            {
                string[] Messages = this.Receive();

                try
                {
                    Position = new LatitudeLongitude(Messages[0],Messages[1]);
                    PositionString = Position.GetCoord();
                    Console.WriteLine(NumarTelefon + ":" + PositionString + "======>>>>>" + System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second);
                    UpdateLocation(NumarTelefon,Position);

                    if (this.Data.TipCont == "Organizator")
                    {
                        List<string> Response = new List<string>();
                        Response.Add("1");

                        foreach (LocationableClient a in e.LocationableClients)
                            if (a.NumarTelefon != this.NumarTelefon)
                            {
                                Response.Add(NumarTelefon);
                                Response.Add(Position.GetDistance(a.Position));
                                break;
                            }
                        
                        _Utils.WriteStreamString(Client.ns, CryptDecryptData.CryptData(Response.ToArray()));
                    }
                    else
                        _Utils.WriteStreamString(Client.ns, CryptDecryptData.CryptData(new string[] { "0" }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    foreach (LocationableClient a in e.LocationableClients)
                        if (a.NumarTelefon == this.NumarTelefon)
                        {
                            e.LocationableClients.Remove(a);

                            foreach (NotificableClient nc in e.NotificableClients)
                                if (nc.NumarTelefon == this.NumarTelefon)
                                {
                                    e.NotificableClients.Remove(nc);
                                    nc.Dispose();
                                    break;
                                }

                            e.OnClientEnterExit();
                            break;
                        }
                    break;
                }
            }
        }

        private void UpdateLocation(string NrTel,LatitudeLongitude Positon)
        {
            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("UPDATE Utilizatori SET [LastKnowPosition]='" + Position.GetCoord() + "' WHERE [NumarTelefon]='" + NrTel + "'", Database);
                cmd.ExecuteNonQuery();
            }
        }

        public void Send(string Message)
        {

        }

        public string[] Receive()
        {
            return CryptDecryptData.DecryptData(_Utils.ReadStreamString(Client.ns));
        }

        public void Dispose()
        {
            GettingLocationThread.Abort();
        }
    }

    public class LocationableClientData
    {
        public string Nume;
        public string Prenume;
        public string TipCont;
        public string NumarTelefon;
        public string Photo;
        public string[] StreamFile;

        public LocationableClientData(string NumarTelefon)
        {
            this.NumarTelefon = NumarTelefon;

            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT [Nume],[Prenume],[TipCont],[LowSizePhoto] FROM Utilizatori WHERE [NumarTelefon]='" + NumarTelefon + "'", Database);
                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    Nume = (string)Reader[0];
                    Prenume = (string)Reader[1];
                    TipCont = (string)Reader[2];
                    Photo = (string)Reader[3];
                }

                string UniqueId = GetPhotoUniqueId();

                StreamFile = new string[] { "ClientEnterExit", Nume, Prenume, TipCont, NumarTelefon,UniqueId, Photo };
            }
        }

        private string GetPhotoUniqueId()
        {
            int[] NumList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string[] WordList = new string[] { "x", "y", "z", "a", "b", "c" };
            string UniqueId=string.Empty;

            Random rand = new Random();
            for(int i=0;i<3;i++)
                if (rand.Next(0,2).Equals(0))
                    UniqueId = UniqueId + NumList[rand.Next(0,NumList.Length)];
                else
                    UniqueId = UniqueId + WordList[rand.Next(0,WordList.Length)];

            return UniqueId;
        }
    }
}
