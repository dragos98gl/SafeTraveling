using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.Games.Bounce_Game;
using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.Games
{
    class GamesHost
    {
        const int SleepTime = 300;
        List<Game> Games;
        List<Excursie> Excursii;
        TcpListener host;

        public void Start(TcpListener host,List<Excursie> Excursii)
        {
            this.host = host;
            Games = new List<Game>();
            this.Excursii = Excursii;

            new Thread(new ThreadStart(LoadClients)).Start();
        }

        private void LoadClients()
        {
            while (true)
            {
                if (host.Pending())
                    new Thread(new ParameterizedThreadStart(RedirectClient)).Start(host.AcceptTcpClient());
                Thread.Sleep(SleepTime);
            }
        }

        private void RedirectClient(object newClient)
        {
            TcpClient Client = (TcpClient)newClient;
            NetworkStream ns = Client.GetStream();

            string[] Request = CryptDecryptData.DecryptData(_Utils.ReadStreamString(ns));

            //1.GameType
            //2.Intent-p1,p2
            //        -p1
            //3.GameId


            switch (Request[0])
            {
                case _Details.Game_XandO: {
                    if (Request[1] == "CREATE")
                    {
                        string NrTel_P1 = Request[2];
                        string NrTel_P2 = Request[3];
                        NotificableClient NotificableP2 = null;

                        foreach (Excursie e in Excursii)
                            foreach (NotificableClient nc in e.NotificableClients)
                                if (nc.NumarTelefon == NrTel_P2)
                                    NotificableP2 = nc;

                        XandO newBounceGame = new XandO(new Client(Client, ns), NotificableP2, Games,GetUserName(NrTel_P1));
                        Games.Add((Game)newBounceGame);
                    }
                    if (Request[1] == "JOIN")
                    {
                        string GameId = Request[2];
                        bool IfExist = false;

                        foreach (XandO bg in Games.ToList())
                        {
                            if (bg.GameId == GameId)
                            {
                                IfExist = true;
                                bg.Join(new Client(Client, ns));
                                break;
                            }
                        }

                        if (IfExist)
                        {

                        }
                        else
                        {

                        }
                    }
                }break;
                case _Details.Game_Bounce: {
                    if (Request[1] == "CREATE")
                    {
                        string NrTel_P1=Request[2];
                        string NrTel_P2=Request[3];
                        NotificableClient NotificableP2 =null;

                        foreach (Excursie e in Excursii)
                            foreach (NotificableClient nc in e.NotificableClients)
                            {
                                if (nc.NumarTelefon == NrTel_P2)
                                {
                                    NotificableP2 = nc;
                                }
                            }

                        BounceGame newBounceGame = new BounceGame(new Client(Client, ns), NotificableP2, Games,GetUserName(NrTel_P1));
                        Games.Add(newBounceGame);
                    }
                    if (Request[1] == "JOIN")
                    {
                        string GameId = Request[2];
                        bool IfExist = false;

                        foreach (BounceGame bg in Games)
                        {
                            if (bg.GameId == GameId)
                            {
                                IfExist = true;
                                bg.Join(new Client(Client,ns));
                                break;
                            }
                        }

                        if (IfExist)
                        {

                        }
                        else
                        {
                            
                        }
                    }

                }break;
            }
        }

        private string GetUserName(string NrTel)
        {
            using (SqlConnection Database = new SqlConnection(_Details.DatabaseConnectionString))
            {
                Database.Open();

                SqlCommand cmd = new SqlCommand("SELECT [Nume],[Prenume] FROM Utilizatori WHERE [NumarTelefon]=@NrTel",Database);
                cmd.Parameters.AddWithValue("NrTel",NrTel);

                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.Read())
                    return (Reader[0] as string + " " + Reader[1] as string);
                else return "err";
            }
        }
    }

    class Game
    {
        public string GameId;

        public Game()
        {
            GameId = new Guid().ToString();
        }
    }
}
