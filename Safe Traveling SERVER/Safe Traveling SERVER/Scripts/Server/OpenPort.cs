using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server
{
    class OpenPort
    {
        List<TcpListener> Listeners = new List<TcpListener>();

        public OpenPort()
        { 
        }

        public void Start(int Port)
        {
            TcpListener Listener = new TcpListener(System.Net.IPAddress.Any,Port);
            Listener.Start();

            Listeners.Add(Listener);
        }

        public TcpListener Use (int Port)
        {
            TcpListener L = null;

            foreach (TcpListener Listener in Listeners)
                if (((IPEndPoint)Listener.LocalEndpoint).Port.Equals(Port))
                {
                    L = Listener;
                    break;
                }

            return L;
        }

        public void Stop(int Port)
        {
            foreach (TcpListener Listener in Listeners)
                if (((IPEndPoint)Listener.LocalEndpoint).Port.Equals(Port))
                    Listener.Stop();
        }

        public void StopAll()
        {
            foreach (TcpListener Listener in Listeners)
                Listener.Stop();
        }
    }
}
