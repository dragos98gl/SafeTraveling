using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.TripServices
{
    class TripClient:TripClientStream
    {
        public TripClientData TClientData;
        public string NumarTelefon;
        public Client Client_OUTPUT;
        Excursie e;
        bool isReady = false;

        public TripClient(Client Client_INPUT, Client Client_OUTPUT, string NumarTelefon, Excursie e)
            : base(Client_INPUT, Client_INPUT)
        {
            this.NumarTelefon = NumarTelefon;
            this.Client_OUTPUT = Client_OUTPUT;
            this.e = e;
            GetUserData();

            Thread WaitForOUTPUT = new Thread(() =>
            {
                while (this.Client_OUTPUT == null)
                {
                    Thread.Sleep(1000);
                }

                OUTPUT = this.Client_OUTPUT;
                isReady = true;
                INPUT_GET(e);

               // e.Excursie_ClientEnterExit(this, new EventArgs());
            });
            WaitForOUTPUT.Start();

            new Thread(()=>{
                if (!WaitForOUTPUT.Join(TimeSpan.FromSeconds(10)))
                {
                    WaitForOUTPUT.Abort();
                    e.ClientsList.Remove(this);
                }
            }).Start();
        }

        private void GetUserData()
        {
            TClientData = new TripClientData(NumarTelefon);
        }

        public override void OUTPUT_SEND(string[] Messages)
        {
            if (isReady)
 	            base.OUTPUT_SEND(Messages);
        }

        public void Dispose()
        {
            foreach (TripClient tClient in e.ClientsList)
                if (tClient == this)
                    e.ClientsList.Remove(this);
        }
    }

    class TripClientData
    {
        public string Nume;
        public string Prenume;
        public string Varsta;
        public string Sex;
        public string Email;
        public string NumarTelefon;
        public string TipCont;
        public string Parola;
        public byte[] LowSizePhoto;
        public string URL;

        public TripClientData(string NumarTelefon)
        {
            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM Utilizatori WHERE [NumarTelefon]='" + NumarTelefon + "'", Database);
                SqlDataReader Reader = cmd.ExecuteReader();

                //1.Nume
                //2.Prenume
                //3.Varsta
                //4.Sex
                //5.Email
                //6.NumarTelefon
                //7.TipCont
                //8.Parola
                //9.LowSizePhoto
                //10.URL

                string LowPhotoBase64String = string.Empty;
                while (Reader.Read())
                {
                    Nume = (string)Reader[0];
                    Prenume = (string)Reader[1];
                    Varsta = (string)Reader[2];
                    Sex = (string)Reader[3];
                    Email = (string)Reader[4];
                    NumarTelefon = (string)Reader[5];
                    TipCont = (string)Reader[6];
                    Parola = (string)Reader[7];
                    LowPhotoBase64String = (string)Reader[8];
                    URL = (string)Reader[9];
                    this.NumarTelefon = NumarTelefon;
                }
                LowSizePhoto = Convert.FromBase64String(LowPhotoBase64String);
            }
        }
    }
    
    class TripClientStream
    {
        Client INPUT;
        protected Client OUTPUT;
        TripClient tClient;

        public TripClientStream(Client INPUT, Client OUTPUT)
        {
            this.INPUT = INPUT;  //get
            this.OUTPUT = OUTPUT;//send
            this.tClient = tClient;
        }

        public void INPUT_GET(Excursie e)
        {
            new Thread(new ThreadStart(() => {
                while (true)
                {
                    try
                    {
                        string[] Messages = CryptDecryptData.DecryptData(_Utils.ReadStreamString(INPUT.ns));
                        e.OnClientGetMessage(Messages);
                    }
                    catch
                    {
                        Thread.CurrentThread.Abort();
                        break;
                    }
                }
            })).Start();
        }

        public virtual void OUTPUT_SEND(string[] Messages)
        {
            _Utils.WriteStreamString(OUTPUT.ns, CryptDecryptData.CryptData(Messages));
        }
    }
}
