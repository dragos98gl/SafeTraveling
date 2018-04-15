using Safe_Traveling_SERVER.Scripts.Aplication_Server;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LS;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NewTrip;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.Login_SignUp_NewTrip_Services
{
    class Login_SignUp_NewTripServices
    {
        const int SleepTime = 300;
        TcpListener host;

        public void Start(TcpListener host)
        {
            this.host = host;
            new Thread(new ThreadStart(SearchForClients)).Start();
        }

        private void SearchForClients()
        {
            while (true)
            {
                if (host.Pending())
                    new Thread(new ParameterizedThreadStart(ClientHandle)).Start(host.AcceptTcpClient());
                Thread.Sleep(SleepTime);
            }
        }

        private void ClientHandle(object c)
        {
            TcpClient Client = (TcpClient)c;
            NetworkStream ns = Client.GetStream();

            string[] Messages = CryptDecryptData.DecryptData(_Utils.ReadStreamString(ns));

            switch (Messages[0])
            {
                case "LOGIN":
                    LoginService.Handle(Client, ns, Messages);
                    break;
                case "SIGNUP":
                    SignUpService.Handle(Client, ns, Messages);
                    break;
            }
        }
    }
}
