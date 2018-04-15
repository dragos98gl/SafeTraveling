using Safe_Traveling_SERVER.Scripts.Server;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.Games;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LargeFilesTransfer;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.Login_SignUp_NewTrip_Services;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LS;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NewTrip;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.TripServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Aplication_Server
{
    class AplicationServer
    {
        OpenPort OP = new OpenPort();

        public void Start()
        {
            OP.Start(_Details.LoginSignUpNewTripPort);
            OP.Start(_Details.NotificationServicePort);
            OP.Start(_Details.LocationServicePort);
            OP.Start(_Details.TripPort_INPUT);
            OP.Start(_Details.TripPort_OUTPUT);
            OP.Start(_Details.LargeFilesPort);
            OP.Start(_Details.GamesHostPort);

            List<Excursie> Excursii = new List<Excursie>();

            NotificationService Notification = new NotificationService();
            LocationService Location = new LocationService();
            Login_SignUp_NewTripServices LoginSignUp = new Login_SignUp_NewTripServices();
            NewTrip_TripEnterServices TripListener = new NewTrip_TripEnterServices();
            LargeFilesTransfer LargeFilesTransfer = new LargeFilesTransfer();
            GamesHost GamesHost = new GamesHost();

            Notification.Start(OP.Use(_Details.NotificationServicePort));
            Location.Start(OP.Use(_Details.LocationServicePort));
            LoginSignUp.Start(OP.Use(_Details.LoginSignUpNewTripPort));
            TripListener.Start(OP.Use(_Details.TripPort_INPUT), OP.Use(_Details.TripPort_OUTPUT), Location, Notification, Excursii);
            LargeFilesTransfer.Start(OP.Use(_Details.LargeFilesPort),Excursii);
            GamesHost.Start(OP.Use(_Details.GamesHostPort),Excursii);
        }
    }
}
