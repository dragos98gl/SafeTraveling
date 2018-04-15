using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Safe_Traveling_SERVER.Scripts.Visual_Scripts;
using System.Net;
using System.IO;

namespace Safe_Traveling_SERVER
{
    public partial class LoadingIntro : Form
    {
        private RotateImage VG = new RotateImage();

        public static string PublicIp="Cannot reach internet!";
        

        public LoadingIntro()
        {
            InitializeComponent();

            new Thread(new ThreadStart(() => {
               // GetPublicIp();
                VerifyData();

                VG.Stop();

                Application.Exit();
                (new Form1()).ShowDialog();
            })).Start();
    
            new Thread(new ThreadStart(() => {
                VG.Start(pictureBox1, 100, 60);
            })).Start();
        }

        private void VerifyData()
        {
            string Path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (System.IO.File.Exists(Path + @"\cfg.txt"))
            {
            }
            else {
                File.CreateText(Path + @"\cfg.txt");


            }
        }

        private void CreateDefaultCfg()
        {
            string u = "";
        }

        private void GetPublicIp()
        {
            PublicIp = new WebClient().DownloadString("http://ifconfig.me/ip");
        }
    }
}
