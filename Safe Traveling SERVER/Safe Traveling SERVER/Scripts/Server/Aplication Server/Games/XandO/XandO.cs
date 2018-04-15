using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.Games
{
    class XandO:Game
    {
        XandO_Player Player1;
        XandO_Player Player2;
        Thread GamePoolThread;
        List<Game> Games;

        public XandO(Client P1, NotificableClient NotificableP2, List<Game> Games,string Username)
        {
            this.Games = Games;
            Player1 = new XandO_Player(P1);

            Thread TimeOut = new Thread(() =>
            {
                while (this.Player2 == null)
                {
                    Thread.Sleep(1000);
                }
            });
            TimeOut.Start();

            new Thread(() => {
                if (!TimeOut.Join(TimeSpan.FromSeconds(20)))
                {
                    Player1.Dispose();
                    Games.Remove(this);
                }
            }).Start();

            NotificableP2.Send(new string[] { _Details.GameInvitation_XandO, base.GameId,Username });
        }

        public void Join(Client P2)
        {
            Player2 = new XandO_Player(P2);

            GamePoolThread = new Thread(GamePool);
            GamePoolThread.Start();
        }

        void GamePool()
        {
            Player1.Send("1");                                                                                          
            Player2.Send("1");
            byte[] Buffer1 = new byte[26];
            byte[] Buffer2 = new byte[26];

            while (true)
            {
                try
                {
                    int P1Pos = Player1.Client.ns.Read(Buffer1, 0, Buffer1.Length);
                    Player2.Client.ns.Write(Buffer1, 0, P1Pos);
                 
                    int P2Pos = Player2.Client.ns.Read(Buffer2, 0, Buffer2.Length);
                    Player1.Client.ns.Write(Buffer2, 0, P2Pos);
                }
                catch {
                    Player1.Dispose();
                    Player2.Dispose();
                    Games.Remove(this);
                    GamePoolThread.Abort();
                }
            }
        }
    }

    class XandO_Player
    {
        public Client Client;

        public XandO_Player(Client Client)
        {
            this.Client = Client;
        }

        public void Send(string Message)
        {
            _Utils.WriteStreamString(Client.ns, CryptDecryptData.CryptData(new string[] { Message }));
        }

        public void Send(string[] Messages)
        {
            _Utils.WriteStreamString(Client.ns, CryptDecryptData.CryptData(Messages));
        }

        public void Dispose()
        {
            Client.ns.Dispose();
            Client.client.Close();
        }
    }
}
