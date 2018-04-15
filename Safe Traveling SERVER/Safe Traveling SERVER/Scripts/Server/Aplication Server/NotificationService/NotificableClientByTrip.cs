using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS
{
    class NotificableClientByTrip
    {
        List<Client> Clients;
        List<string> NumePrenume;
        string TripId;

        public void SetTripId(string TripId)
        {
            this.TripId = TripId;

            Clients = new List<Client>();
            NumePrenume = new List<string>();
        }

        public string GetTripId()
        {
            return TripId;
        }

        public void AddNewClient(Client c,string np)
        {
            Clients.Add(c);
            NumePrenume.Add(np);
        }

        public Client GetClientByNs(NetworkStream ns)
        {
            Client WantedClient = null;

            foreach (Client c in Clients)
            {
                if (c.ns.Equals(ns))
                {
                    WantedClient = c;
                    break;
                }
            }

            return WantedClient;
        }
    }
}
