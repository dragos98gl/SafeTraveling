﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server
{
    class Client
    {
        public TcpClient client;
        public NetworkStream ns;

        public Client(TcpClient client,NetworkStream ns)
        {
            this.client = client;
            this.ns = ns;
        }
    }
}
