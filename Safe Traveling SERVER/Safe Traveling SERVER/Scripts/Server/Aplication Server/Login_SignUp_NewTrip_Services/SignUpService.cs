using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Safe_Traveling_SERVER.Scripts.Server;
using Safe_Traveling_SERVER.Scripts.Aplication_Server;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Drawing;
using Safe_Traveling_SERVER.Scripts.Visual_Scripts;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server
{
    class SignUpService
    {
        public static void Handle(TcpClient Client, NetworkStream ns, string[] Messages)
        {
            //1.Nume
            //2.Prenume
            //3.Varsta
            //4.Sex
            //5.Email
            //6.Numar Telefon
            //7.Tip Cont
            //8.Parola

            string
                Nume = Messages[1],
                Prenume = Messages[2],
                Varsta = Messages[3],
                Sex = Messages[4],
                Email = Messages[5],
                NumarTelefon = Messages[6],
                TipCont = Messages[7],
                Parola = Messages[8];
            string URL;
            string LowSizePhoto;

            string Response = string.Empty;
            using (SqlConnection Database = new SqlConnection(@"Data Source=.\SQLEXPRESS;User id=Mihawai;" + "Password=11234567;" + "Database = SafeTraveling"))
            {
                Database.Open();

                URL = CreateUserURL(Nume+Prenume,NumarTelefon);
                AddUserPhoto(URL);
                LowSizePhoto = CreateLowSizePhoto.GetLowSizePhotoBase64(URL + @"\test.png"); 

                SqlCommand cmd = new SqlCommand(@"INSERT INTO Utilizatori ([Nume],[Prenume],[Varsta],[Sex],[Email],[NumarTelefon],[TipCont],[Parola],[LowSizePhoto],[URL]) VALUES 
                ('" + Nume + "'," +
                    "'" + Prenume + "'," +
                    "'" + Varsta + "'," +
                    "'" + Sex + "'," +
                    "'" + Email + "'," +
                    "'" + NumarTelefon + "'," +
                    "'" + TipCont + "'," +
                    "'" + Parola + "'," +
                    "'" + LowSizePhoto + "'," +
                    "'" + URL + "')", Database);
                cmd.ExecuteNonQuery();

                Response = "ok";
            }

            /*using (SmtpClient EmailSender = new SmtpClient("smtp.gmail.com", 587))
            {
                EmailSender.Credentials = new NetworkCredential("","");
                EmailSender.EnableSsl = true;

                try
                {
                    EmailSender.Send("from","to","subject","content");
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Unexpected error has happend! (" + exception.Message + ")");
                }
            }*/

            _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { Response }));

            Client.Close();
            ns.Dispose();
        }

        private static void AddUserPhoto(string URL)
        {
            Bitmap b = new Bitmap(Safe_Traveling_SERVER.Properties.Resources.DefaultUserPhoto);
            b.Save(URL + @"\test.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private static string CreateUserURL(string NumeUtilizator, string NumarTelefon)
        {
            string URL = string.Empty;
            string ExecutionURL = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string Suffix = @"\SafeTraveling\Users\" + NumeUtilizator + "_" + NumarTelefon;

            URL = ExecutionURL + Suffix;

            Directory.CreateDirectory(URL);

            return URL;
        }
    }
}
