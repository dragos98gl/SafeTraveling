using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LS;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.TripServices;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.TripServices.QuestionPool;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.TripServices.Tasks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server
{
    class GetMessageEventArgs : EventArgs { public string[] Messages; }
    class Excursie 
    {
        public delegate void ClientGetMessageHandler(object sender,GetMessageEventArgs e);
        public event ClientGetMessageHandler ClientGetMessage;

        public virtual void OnClientGetMessage(string[] Msgs)
        {
            if (ClientGetMessage!=null)
            {
                ClientGetMessage(this, new GetMessageEventArgs() { Messages = Msgs});
            } 
        }

        public delegate void ClientEnterExitHandler(object sender,EventArgs e);
        public event ClientEnterExitHandler ClientEnterExit;

        public virtual void OnClientEnterExit()
        {
            if (ClientEnterExit != null)
                ClientEnterExit(this,EventArgs.Empty);
        }

        public List<TripClient> ClientsList;
        public List<LocationableClient> LocationableClients;
        public List<NotificableClient> NotificableClients;
        public List<QuestionPool> QuestionPools;

        LocationService LocationService;
        NotificationService NotificationService;
        public string DestinatiePrincipala;
        public string TipulParticipantilor;
        public string DataPlecare;
        public string DataIntoarcere;
        public string LocatiePlecare;
        public string OraPlecare;
        public string Observatii;
        public string NumeOrganizator;
        public string NumarTelefon;
        public string UniqueId;
        public string URL;

        public Excursie(LocationService LocationService, NotificationService NotificationService, string DestinatiePrincipala, string TipulParticipantilor, string DataPlecare, string DataIntoarcere, string LocatiePlecare, string OraPlecare, string Observatii, string NumeOrganizator, string NumarTelefon, string UniqueId, string URL)
        {
            this.LocationService = LocationService;
            this.NotificationService = NotificationService;
            this.DestinatiePrincipala = DestinatiePrincipala;
            this.TipulParticipantilor = TipulParticipantilor;
            this.DataPlecare = DataPlecare;
            this.DataIntoarcere = DataIntoarcere;
            this.LocatiePlecare = LocatiePlecare;
            this.OraPlecare = OraPlecare;
            this.Observatii = Observatii;
            this.NumeOrganizator = NumeOrganizator;
            this.NumarTelefon = NumarTelefon;
            this.UniqueId = UniqueId;
            this.URL = URL;

            ClientsList = new List<TripClient>();
            LocationableClients = new List<LocationableClient>();
            NotificableClients = new List<NotificableClient>();
            QuestionPools = new List<QuestionPool>();

            LocationService.Excursii.Add(this);
            NotificationService.Excursii.Add(this);

            ClientGetMessage += Excursie_ClientGetMessage;
            ClientEnterExit += Excursie_ClientEnterExit;

            new Thread(SendOnlineClients).Start();
        }

        void SendOnlineClients()
        {
            while(true)
            {
                OnClientEnterExit();
                Thread.Sleep(1000);
            }
        }

        public void Excursie_ClientEnterExit(object sender, EventArgs e)
        {
            foreach (TripClient t in ClientsList.ToList())
            {
                for (int i = 0; i < LocationableClients.Count; i++)
                {
                    if (i != LocationableClients.Count - 1)
                    {
                        string[] lcData = LocationableClients[i].Data.StreamFile;
                        string[] RegularData = new string[] { lcData[0], "1", lcData[1], lcData[2], lcData[3], lcData[4], lcData[5],LocationableClients[i].PositionString };

                        t.OUTPUT_SEND(RegularData);
                    }
                    else
                    {
                        string[] lcData = LocationableClients[i].Data.StreamFile;
                        string[] RegularData = new string[] { lcData[0], "0", lcData[1], lcData[2], lcData[3], lcData[4], lcData[5], LocationableClients[i].PositionString };

                        t.OUTPUT_SEND(RegularData);
                    }
                }
            }
        }
        
        void Excursie_ClientGetMessage(object sender, GetMessageEventArgs e)
        {
                switch (e.Messages[0])
                {
                    case _Details.GetUserDataByPhone:
                        {
                            string UserPhone = e.Messages[1];

                            foreach (TripClient tClient in ClientsList)
                                if (tClient.TClientData.NumarTelefon.Equals(UserPhone))
                                    using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
                                    {
                                        Database.Open();

                                        string t = "SELECT * FROM Utilizatori WHERE [NumarTelefon]='" + tClient.NumarTelefon + "'";

                                        SqlCommand cmd = new SqlCommand(t, Database);
                                        SqlDataReader Reader = cmd.ExecuteReader();

                                        if (Reader.Read())
                                        {
                                            string UserNume = (string)Reader[0];
                                            string UserPrenume = (string)Reader[1];
                                            string UserVarsta = (string)Reader[2];
                                            string UserSex = (string)Reader[3];
                                            string UserEmail = (string)Reader[4];
                                            string[] RegularData = new string[] { _Details.GetUserDataByPhone, UserNume, UserPrenume, UserVarsta, UserSex, UserEmail };

                                            tClient.OUTPUT_SEND(RegularData);
                                        }
                                    }
                        } break;
                    case _Details.EditUserInfo:
                        {
                            EditUserInfo.Handle(e.Messages, ClientsList);
                        }
                        break;
                    default:
                        {
                            foreach (TripClient t in ClientsList.ToList())
                            {
                                t.OUTPUT_SEND(e.Messages);
                            }
                        } break;
                }
        }

        public void AdaugareUtilizatorNou(Client Client, string NumarTelefon)
        {
            TripClient TClient = new TripClient(Client, null, NumarTelefon, this);
            ClientsList.Add(TClient);
        }
    }
}
