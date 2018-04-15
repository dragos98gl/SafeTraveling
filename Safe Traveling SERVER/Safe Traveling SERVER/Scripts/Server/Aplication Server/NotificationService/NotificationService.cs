using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS
{
    class NotificationService
    {
        public List<Excursie> Excursii;
        const int SleepTime = 300;
        TcpListener host;

        public void Start(TcpListener host)
        {
            this.host = host;
            Excursii = new List<Excursie>();

            new Thread(new ThreadStart(LoadClients)).Start();
        }

        private void LoadClients()
        {
            while (true)
            {
                if (host.Pending())
                    new Thread(new ParameterizedThreadStart(ManageClients)).Start(host.AcceptTcpClient());
                Thread.Sleep(SleepTime);
            }
        }

        private void ManageClients(object newClient)
        {
            TcpClient client = (TcpClient)newClient;
            NetworkStream ns = client.GetStream();

            string[] Messages = CryptDecryptData.DecryptData (_Utils.ReadStreamString(ns));

            //1.TripId
            //2.NumePrenume

            string TripId = Messages[0];
            string NumarTelfon = Messages[1];

            foreach (Excursie e in Excursii)
                if (e.UniqueId.Equals(TripId))
                {
                    bool IfExist = false;
                    foreach (NotificableClient nc in e.NotificableClients)
                        if (nc.NumarTelefon == NumarTelfon)
                            IfExist = true;

                    if (!IfExist)
                    {
                        Client nClient = new Client(client, ns);
                        NotificableClient nNotificableClient = new NotificableClient(nClient, NumarTelfon, e,TripId);
                        e.NotificableClients.Add(nNotificableClient);
                    }
                    else
                    {
                        client.Close();
                    }
                }
        }
    }
}
