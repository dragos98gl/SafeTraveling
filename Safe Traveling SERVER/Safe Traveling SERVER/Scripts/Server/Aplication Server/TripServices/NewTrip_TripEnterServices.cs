using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LS;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NewTrip;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS;
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
    class NewTrip_TripEnterServices
    {
        TcpListener host_INPUT;
        TcpListener host_OUTPUT;
        const int SleepTime = 300;
        LocationService LocationService;
        NotificationService NotificationService;
        List<Excursie> Excursii;

        public void Start (TcpListener host_INPUT,TcpListener host_OUTPUT,LocationService LocationService,NotificationService NotificationService,List<Excursie> Excursii)
        {
            this.host_INPUT = host_INPUT;
            this.host_OUTPUT = host_OUTPUT;
            this.LocationService = LocationService;
            this.NotificationService = NotificationService;
            this.Excursii = Excursii;
            AddActualTrips();
            
            new Thread(new ThreadStart(SearchForRequests)).Start();
            new Thread(new ThreadStart(SearchForOUTPUT)).Start();
        }

        private void SearchForOUTPUT()
        {
            while (true)
            {
                if (host_OUTPUT.Pending())
                    new Thread(new ParameterizedThreadStart(JoinOUTPUT)).Start(host_OUTPUT.AcceptTcpClient());
                Thread.Sleep(SleepTime);
            }
        }
        private void JoinOUTPUT(object nClient)
        {
            TcpClient Client = (TcpClient)nClient;
            NetworkStream ns = Client.GetStream();
            
            string[] Messages = CryptDecryptData.DecryptData(_Utils.ReadStreamString(ns));
           
            string NumarTelefon = Messages[1];
            string TripId = Messages[2];

            foreach (Excursie e in Excursii)
                if (e.UniqueId == TripId)
                    foreach (TripClient t in e.ClientsList)
                    {
                        if (t.NumarTelefon == NumarTelefon)
                        {
                            t.Client_OUTPUT = new Client(Client, ns);
                        }
                    }
        }

        private void SearchForRequests()
        {
            while(true)
            {
                if (host_INPUT.Pending())
                    new Thread(new ParameterizedThreadStart(GestionareUtilizatori)).Start(host_INPUT.AcceptTcpClient());
                Thread.Sleep(SleepTime);
            }
        }

        private void GestionareUtilizatori(object newClient)
        {
            TcpClient Client = (TcpClient)newClient;
            NetworkStream ns = Client.GetStream();
            
            string[] Messages = CryptDecryptData.DecryptData(_Utils.ReadStreamString(ns));

            switch (Messages[0])
            {
                case "TRIPENTER": TripEnter.Handle(Client, ns, Messages, Excursii);
                    break;
                case "NEWTRIP": NewTripService.Handle(Client, ns, Messages, LocationService, NotificationService, Excursii);
                    break;
            }
        }

        private void AddActualTrips()
        {
            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Excursii", Database);
                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    Excursie Excursie = new Excursie(
                        LocationService, NotificationService,
                        (string)Reader[0], (string)Reader[1],
                        (string)Reader[2], (string)Reader[3],
                        (string)Reader[4], (string)Reader[5],
                        (string)Reader[6], (string)Reader[7],
                        (string)Reader[8], (string)Reader[9],
                        (string)Reader[10]);
                    Excursii.Add(Excursie);
                }
            }
        }
    }
}
