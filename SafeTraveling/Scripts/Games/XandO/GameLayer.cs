using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CocosSharp;
using System.Threading;
using System.Net.Sockets;

namespace SafeTraveling.Scripts.Games.XandO
{
    class GameLayer: CCLayerColor
    {
        CCSprite X_Y_Table;
        CCLabel Score;
        CCLabel Player1;
        CCLabel Player2;

        float Size_X;
        float Size_Y;

        string IntentType;
        Activity context;
        Client Client;
        Thread GetDataThread;

        int Player1Score=0;
        int Player2Score=0;

        PlayerType PlayerType;

        public GameLayer(Activity context, float Size_X, float Size_Y, string IntentType, string NrTel,string GameId)
            : base(CCColor4B.Black)
        {
            this.context = context;

            this.Size_X = Size_X;
            this.Size_Y = Size_Y;
            this.IntentType = IntentType;
            GetDataThread = new Thread(GetData);

            float offset = Size_Y / 15;
            X_Y_Table = new CCSprite("X_O_Table");
            X_Y_Table.AnchorPoint = CCPoint.AnchorUpperLeft;
            X_Y_Table.PositionX = offset;
            X_Y_Table.PositionY = Size_Y-offset;
            X_Y_Table.ContentSize = new CCSize(Size_Y - 2 * offset, Size_Y - 2 * offset);

            Score = new CCLabel("Score", "Fonts/MarkerFelt", 200, CCLabelFormat.SystemFont);
            Score.AnchorPoint = CCPoint.AnchorUpperRight;
            Score.PositionX = Size_X-100;
            Score.PositionY = Size_Y-100;

            Player1 = new CCLabel("You : 0", "Fonts/MarkerFelt", 100, CCLabelFormat.SystemFont);
            Player1.AnchorPoint = CCPoint.AnchorUpperLeft;
            Player1.PositionX = Score.PositionX - Score.ContentSize.Width;
            Player1.PositionY = Score.PositionY - 100;

            Player2 = new CCLabel("Player 2 : 0", "Fonts/MarkerFelt", 100, CCLabelFormat.SystemFont);
            Player2.AnchorPoint = CCPoint.AnchorUpperLeft;
            Player2.PositionX = Score.PositionX - Score.ContentSize.Width;
            Player2.PositionY = Player1.PositionY - 50;

            CreateTouchArea();

            if (IntentType == "CREATE")
            {
                TcpClient client = new TcpClient(_Details.ServerIP, _Details.GamesHostPort);
                NetworkStream ns = client.GetStream();
                Client = new Client(client, ns);

                _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.Game_XandO, "CREATE", Utilizator_Trip.Me.NumarTelefon, NrTel }));
                string Response = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns))[0];

                if (Response == "1")
                {
                    AddChild(Player1);
                    AddChild(Player2);
                    AddChild(Score);
                    AddChild(X_Y_Table);

                    MyTurn = true;
                    PlayerType = PlayerType.X;
                    GetDataThread.Start();
                }
                else
                    context.Finish();
            }
            else
            {
                TcpClient client = new TcpClient(_Details.ServerIP, _Details.GamesHostPort);
                NetworkStream ns = client.GetStream();
                Client = new Client(client, ns);

                _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.Game_XandO, "JOIN", GameId, NrTel }));
                string Response = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns))[0];

                if (Response == "1")
                {
                    AddChild(Player1);
                    AddChild(Player2);
                    AddChild(Score);
                    AddChild(X_Y_Table);

                    GetDataThread.Start();
                    PlayerType = PlayerType.O;
                    MyTurn = false;
                }
                else
                    context.Finish();
            }

            X_Clone = new CCSprite("X");
            O_Clone = new CCSprite("O");
            X_Win = new CCSprite("X_Win");
            O_Win = new CCSprite("O_Win");
        }

        CCSprite X_Clone;
        CCSprite O_Clone;
        CCSprite X_Win;
        CCSprite O_Win;

        void GetData()
        {
            while (true)
                if (!MyTurn)
                {
                    byte[] Buffer = new byte[1];
                    int MoveLength = Client.ns.Read(Buffer, 0, Buffer.Length);
                    int GetMove = int.Parse(Encoding.ASCII.GetString(Buffer, 0, MoveLength));

                    if (PlayerType != PlayerType.X)
                    {
                        CCSprite X = new CCSprite();
                        X.Texture = X_Clone.Texture;
                        X.AnchorPoint = CCPoint.AnchorMiddle;
                        X.ContentSize = new CCSize(Moves[GetMove].Rect.Size.Width - 50, Moves[GetMove].Rect.Size.Height - 50);
                        X.PositionX = Moves[GetMove].Rect.MidX;
                        X.PositionY = Moves[GetMove].Rect.MidY;
                        Moves[GetMove].State = MoveState.X;

                        AddChild(X);
                    }
                    else {
                        CCSprite O = new CCSprite();
                        O.Texture = O_Clone.Texture;
                        O.AnchorPoint = CCPoint.AnchorMiddle;
                        O.ContentSize = new CCSize(Moves[GetMove].Rect.Size.Width - 50, Moves[GetMove].Rect.Size.Height - 50);
                        O.PositionX = Moves[GetMove].Rect.MidX;
                        O.PositionY = Moves[GetMove].Rect.MidY;
                        Moves[GetMove].State = MoveState.O;

                        AddChild(O);
                    }

                    switch (GetTableState())
                    {
                        case TableState.OWin:
                            {
                                Player2Score++;
                                Player2.Text = "Player 2 : " + Player2Score.ToString();

                                GameFinished();
                            } break;
                        case TableState.XWin:
                            {
                                Player1Score++;
                                Player1.Text = "You : " + Player1Score.ToString();

                                GameFinished();
                            } break;
                        case TableState.Draw:
                            {
                                GameFinished();
                            } break;
                    }

                    MyTurn = true;
                }
        }

        List<Move> Moves;
        void CreateTouchArea()
        {
            Moves = new List<Move>();
            float size = X_Y_Table.ContentSize.Height;
            float offset = X_Y_Table.PositionX;
            float regionSize = size / 3;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    CCRect Rect = new CCRect(offset + regionSize * i, Size_Y - offset - regionSize - regionSize * j, regionSize, regionSize);
                    Move m = new Move();
                    m.State = MoveState.Free;
                    m.Rect = Rect;
                    Moves.Add(m);
                }
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
        }

        bool MyTurn;
        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (MyTurn)
                for (int i = 0; i < Moves.Count; i++)
                {
                    if (Moves[i].State.Equals(MoveState.Free))
                        if (Moves[i].Rect.ContainsPoint(touches[0].Location))
                        {
                            if (PlayerType == PlayerType.X)
                            {
                                CCSprite X = new CCSprite("X");
                                X.ContentSize = new CCSize(Moves[i].Rect.Size.Width - 50, Moves[i].Rect.Size.Height - 50);
                                X.PositionX = Moves[i].Rect.MidX;
                                X.PositionY = Moves[i].Rect.MidY;
                                Moves[i].State = MoveState.X;

                                AddChild(X);
                            }
                            else {
                                CCSprite O = new CCSprite("O");
                                O.ContentSize = new CCSize(Moves[i].Rect.Size.Width - 50, Moves[i].Rect.Size.Height - 50);
                                O.PositionX = Moves[i].Rect.MidX;
                                O.PositionY = Moves[i].Rect.MidY;
                                Moves[i].State = MoveState.O;

                                AddChild(O);
                            }
                            byte[] Buffer = new byte[1];
                            byte[] SendMove = Encoding.ASCII.GetBytes(i.ToString());
                            Client.ns.Write(SendMove, 0, SendMove.Length);

                            switch (GetTableState())
                            {
                                case TableState.OWin:
                                    {
                                        Player2Score++;
                                        Player2.Text = "Player 2 : " + Player2Score.ToString();

                                        GameFinished();
                                    } break;
                                case TableState.XWin:
                                    {
                                        Player1Score++;
                                        Player1.Text = "You : " + Player1Score.ToString();

                                        GameFinished();
                                    } break;
                                case TableState.Draw:
                                    {
                                        GameFinished();
                                    } break;
                            }
                            Score.Text = GetTableState().ToString();

                            MyTurn = false;
                        }
                }
        }

        void GameFinished()
        {
            new Thread(()=>{
                Thread.Sleep(1000);
                RemoveAllChildren();
                CreateTouchArea();

                AddChild(Player1);
                AddChild(Player2);
                AddChild(Score);
                AddChild(X_Y_Table);

                if (PlayerType == PlayerType.X)
                    PlayerType = PlayerType.O;
                else
                    PlayerType = PlayerType.X;
            }).Start();
        }

        TableState GetTableState()
        {
            int[,] WinCases = new int[,] { 
            {0,1,2},
            {3,4,5},
            {6,7,8},
            {0,4,8},
            {6,4,2},
            {0,3,6},
            {1,4,7},
            {2,5,8}
            };

            for (int i = 0; i < 8; i++)
            {
                int a=WinCases[i,0], b=WinCases[i,1], c= WinCases[i,2];
                Move m1 = Moves[a], m2 = Moves[b], m3 = Moves[c];

                if (m1.State == m2.State && m2.State == m3.State)
                {
                    switch (m1.State)
                    {
                        case MoveState.O:
                            {
                                CCSprite O = new CCSprite();
                                O.Texture = O_Win.Texture;
                                O.AnchorPoint = CCPoint.AnchorMiddle;
                                O.ContentSize = new CCSize(m1.Rect.Size.Width - 50, m1.Rect.Size.Height - 50);
                                O.PositionX = m1.Rect.MidX;
                                O.PositionY = m1.Rect.MidY;
                                AddChild(O);

                                O = new CCSprite();
                                O.Texture = O_Win.Texture;
                                O.AnchorPoint = CCPoint.AnchorMiddle;
                                O.ContentSize = new CCSize(m2.Rect.Size.Width - 50, m2.Rect.Size.Height - 50);
                                O.PositionX = m2.Rect.MidX;
                                O.PositionY = m2.Rect.MidY;
                                AddChild(O);

                                O = new CCSprite();
                                O.Texture = O_Win.Texture;
                                O.AnchorPoint = CCPoint.AnchorMiddle;
                                O.ContentSize = new CCSize(m3.Rect.Size.Width - 50, m3.Rect.Size.Height - 50);
                                O.PositionX = m3.Rect.MidX;
                                O.PositionY = m3.Rect.MidY;
                                AddChild(O);

                                return TableState.OWin;
                            } break;
                        case MoveState.X:
                            {
                                CCSprite X = new CCSprite();
                                X.Texture = X_Win.Texture;
                                X.AnchorPoint = CCPoint.AnchorMiddle;
                                X.ContentSize = new CCSize(m1.Rect.Size.Width - 50, m1.Rect.Size.Height - 50);
                                X.PositionX = m1.Rect.MidX;
                                X.PositionY = m1.Rect.MidY;
                                AddChild(X);

                                X = new CCSprite();
                                X.Texture = X_Win.Texture;
                                X.AnchorPoint = CCPoint.AnchorMiddle;
                                X.ContentSize = new CCSize(m2.Rect.Size.Width - 50, m2.Rect.Size.Height - 50);
                                X.PositionX = m2.Rect.MidX;
                                X.PositionY = m2.Rect.MidY;
                                AddChild(X);

                                X = new CCSprite();
                                X.Texture = X_Win.Texture;
                                X.AnchorPoint = CCPoint.AnchorMiddle;
                                X.ContentSize = new CCSize(m3.Rect.Size.Width - 50, m3.Rect.Size.Height - 50);
                                X.PositionX = m3.Rect.MidX;
                                X.PositionY = m3.Rect.MidY;
                                AddChild(X);

                                return TableState.XWin;
                            } break;
                    }
                }
            }

                            
                for (int  i= 0; i < Moves.Count; i++)
                {
                    if (Moves[i].State.Equals(MoveState.Free))
                        return TableState.InGame;
                }
            
                return TableState.Draw;
        }
    }

    class Move
    {
        public CCRect Rect;
        public MoveState State;
    }

    public enum PlayerType
    {
        X,
        O
    }

    public enum TableState
    {
        Draw,
        XWin,
        OWin,
        InGame
    }

    public enum MoveState
    { 
        X,
        O,
        Free
    }
}