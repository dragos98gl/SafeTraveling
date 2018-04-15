using Safe_Traveling_SERVER.Scripts.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Aplication_Server
{
    class LoginService
    {
        public static void Handle(TcpClient Client, NetworkStream ns, string[] Messages)
        {
            //1.Username(NrTel)
            //2.Parola

            string Username = Messages[1];
            string Password = Messages[2];

            string Response = string.Empty;
            string TipCont = string.Empty;
            string OrgEx = string.Empty;

            using (SqlConnection Database = new SqlConnection(@"Data Source=.\SQLEXPRESS;User id=Mihawai;" + "Password=11234567;" + "Database = SafeTraveling"))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT NumarTelefon,Parola,TipCont FROM Utilizatori", Database);
                using (SqlDataReader Reader = cmd.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        if (((string)Reader[0]).Equals(Username))
                            if (((string)Reader[1]).Equals(Password))
                            {
                                TipCont = (string)Reader[2];
                                Response = TipCont;
                                break;
                            }
                            else
                                break;
                    }
                }

                if (TipCont == "Organizator")
                {
                    cmd = new SqlCommand("SELECT [Numar Telefon] FROM Excursii WHERE [Numar Telefon]='"+Username+"'",Database);
                    SqlDataReader Reader = cmd.ExecuteReader();

                    if (Reader.Read())
                        OrgEx = "1";
                    else
                        OrgEx = "0";
                }
            }

            _Utils.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { TipCont,OrgEx }));

            Client.Close();
            ns.Dispose();
        }
    }
}
