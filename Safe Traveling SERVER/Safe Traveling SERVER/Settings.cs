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

namespace Safe_Traveling_SERVER
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            label3.Text = LoadingIntro.PublicIp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tablessControl1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tablessControl1.SelectedIndex = 1;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton1.Checked)
                label3.Text = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }
    }

    public class TablessControl : TabControl
    {
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1328 && !DesignMode)
                m.Result = (IntPtr)1;
            else
                base.WndProc(ref m);
        }
    }
}
