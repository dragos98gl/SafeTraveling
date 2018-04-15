using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Safe_Traveling_SERVER.Scripts.Server;
using System.Data.SqlClient;
using Safe_Traveling_SERVER.Scripts.Aplication_Server;
using System.IO;
using System.Threading;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server;
using Safe_Traveling_SERVER.Scripts.Website;

namespace Safe_Traveling_SERVER
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AplicationServer ApplicationServer = new AplicationServer();
            ApplicationServer.Start();

            button1.Text = "Started";
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new Settings()).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Website Website = new Website();
            Website.Start();

            button2.Text = "Started";
            /*
            var client = new TcpClient();
            var result = client.BeginConnect("192.168.21.32", 2000, null, null);

            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));

            if (!success)
            {
                throw new Exception("Failed to connect.");
            }

            // we have connected
            client.EndConnect(result);*/
        }
    }
}
