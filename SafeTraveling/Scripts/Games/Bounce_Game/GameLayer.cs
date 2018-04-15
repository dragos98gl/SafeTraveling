using System;
using System.Collections.Generic;
using CocosSharp;
using System.Threading;
using Android.App;
using Android.Widget;
using System.Net.Sockets;
using Android.Views;
using System.Text;

namespace SafeTraveling
{
    public class GameLayer : CCLayerColor
    {
        CCSprite _player1Board;
        CCSprite _player2Board;
        CCSprite Ball;

        CCLabel Score_P1;
        CCLabel Score_P2;
        CCLabel CountDownTimer;

        Activity context;
        Thread SendPackets;
        bool finish = true;
        Client Client;
        float Size_X;
        float Size_Y;
        string IntentType;

        public GameLayer(Activity context, float Size_X, float Size_Y, string IntentType, string NrTel,string GameId)
            : base(CCColor4B.Black)
        {
            this.context = context;

            this.Size_X = Size_X;
            this.Size_Y = Size_Y;
            this.IntentType = IntentType;
            SendPackets = new Thread(new ThreadStart(Test));

            _player2Board = new CCSprite("Board1.png");
            _player2Board.PositionX = 500;
            _player2Board.PositionY = Size_Y - (Size_Y / 40) * 3;
            _player2Board.ContentSize = new CCSize(Size_X / 7, Size_Y / 40);

            _player1Board = new CCSprite("Board1");
            _player1Board.PositionX = 100;
            _player1Board.PositionY = (Size_Y / 40) * 3;
            _player1Board.ContentSize = new CCSize(Size_X / 7, Size_Y / 40);

            Ball = new CCSprite("Ball1");
            Ball.Visible = false;
            Ball.AnchorPoint = CCPoint.AnchorMiddle;
            Ball.ContentSize = new CCSize(Size_Y / 20, Size_Y / 20);
            Ball.PositionX = Size_X / 2;
            Ball.PositionY = Size_Y / 2;

            Score_P1 = new CCLabel("0", "Arial", 150, CCLabelFormat.SystemFont);
            Score_P1.PositionX = Size_X/40;
            Score_P1.PositionY = (Size_Y/4);
            Score_P1.Color = CCColor3B.White;
            Score_P1.AnchorPoint = CCPoint.AnchorLowerLeft;

            Score_P2 = new CCLabel("0", "Arial", 150, CCLabelFormat.SystemFont);
            Score_P2.PositionX = Size_X / 40;
            Score_P2.PositionY = (Size_Y / 4) * 3;
            Score_P2.Color = CCColor3B.White;
            Score_P2.AnchorPoint = CCPoint.AnchorUpperLeft;

            CountDownTimer = new CCLabel("0", "Arial", 400, CCLabelFormat.SystemFont);
            CountDownTimer.ContentSize = new CCSize(Size_Y / 20, Size_Y / 20);
            CountDownTimer.PositionX = Size_X / 2;
            CountDownTimer.PositionY = Size_Y / 2;
            CountDownTimer.Color = CCColor3B.White;
            CountDownTimer.AnchorPoint = CCPoint.AnchorMiddle;
            CountDownTimer.Text = "3";

            AddChild(Score_P1);
            AddChild(Score_P2);
            AddChild(_player1Board);
            AddChild(_player2Board);
            AddChild(Ball);
            AddChild(CountDownTimer);

            if (IntentType == "CREATE")
            {
                TcpClient client = new TcpClient(_Details.ServerIP, _Details.GamesHostPort);
                NetworkStream ns = client.GetStream();
                Client = new Client(client, ns);

                _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.Game_Bounce, "CREATE", Utilizator_Trip.Me.NumarTelefon, NrTel }));
                string Response = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns))[0];

                if (Response == "1")
                {
                    SendPackets.Start();
                    System.Timers.Timer T = new System.Timers.Timer();
                    T.Interval = 1000;
                    T.Elapsed += T_Elapsed;
                    T.Start();
                }
                else
                    context.Finish();
            }
            else
            {
                TcpClient client = new TcpClient(_Details.ServerIP, _Details.GamesHostPort);
                NetworkStream ns = client.GetStream();
                Client = new Client(client, ns);

                _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.Game_Bounce, "JOIN", GameId, NrTel }));
                string Response = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns))[0];

                if (Response == "1")
                {
                    SendPackets.Start();
                    System.Timers.Timer T = new System.Timers.Timer();
                    T.Interval = 1000;
                    T.Elapsed += T_Elapsed;
                    T.Start();
                }
                else
                    context.Finish();
            }
        }

        private void Test()
        {
            byte[] Buffer = new byte[26];
            while (true)
            {
                if (_player2Board != null)
                {
                    float BallRateX = Size_X / Ball.PositionX;
                    float BallRateY = Size_Y / Ball.PositionY;
                    float MyPosition = Size_X / _player1Board.PositionX;

                    if (BallRateX.ToString().Length < 8)
                        BallRateX += 0.000001f;
                    if (BallRateY.ToString().Length < 8)
                        BallRateY += 0.000001f;

                    byte[] PositionBytes = Encoding.ASCII.GetBytes(BallRateX + " " + BallRateY + " " + MyPosition);
                    Client.ns.Write(PositionBytes,0,PositionBytes.Length); 
                    
                    int PositionsLength = Client.ns.Read(Buffer,0,Buffer.Length);
                    string[] Coords = Encoding.ASCII.GetString(Buffer, 0, PositionsLength).Split(' ');
        
                    BallRateX = float.Parse(Coords[0]);
                    BallRateY = float.Parse(Coords[1]);
                    MyPosition = float.Parse(Coords[2]);

                    if (IntentType == "JOIN")
                    {
                        Ball.PositionX = Size_X - Size_X / BallRateX;
                        Ball.PositionY = Size_Y - Size_Y / BallRateY;
                    }

                    _player2Board.PositionX = Size_X - Size_X / MyPosition;
                }
            }
        }

        int Time = 3;
        void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Time > -1)
                CountDownTimer.Text = (Time--).ToString();
            else
            {
                CountDownTimer.Visible = false;
                Ball.Visible = true;

                CanPlay = true;

                Schedule(BallPhysics);
            }
        }

        bool CanPlay = true;
        float ballVelocityX;
        float ballVelocityY = 6000;
        float directionY = 1;
        int ScoreP1 = 0;
        int ScoreP2 = 0;

        void BallPhysics(float frameTimeInSeconds)
        {
            if (CanPlay)
            {
                if (IntentType == "CREATE")
                {
                    Ball.PositionX += ballVelocityX * frameTimeInSeconds * frameTimeInSeconds;
                    Ball.PositionY += ballVelocityY * directionY * frameTimeInSeconds * frameTimeInSeconds;

                    bool ballOverlapP2 = Ball.BoundingBoxTransformedToParent.IntersectsRect(_player2Board.BoundingBoxTransformedToParent);
                    bool ballOverlapP1 = Ball.BoundingBoxTransformedToParent.IntersectsRect(_player1Board.BoundingBoxTransformedToParent);

                    if (ballOverlapP2 || ballOverlapP1)
                    {
                        directionY = -directionY;

                        ballVelocityY += 4000;

                        ballVelocityX = CCRandom.GetRandomFloat(-ballVelocityY, ballVelocityY);
                    }
                }
                float screenRightM = VisibleBoundsWorldspace.MaxX;
                float screenLeftM = VisibleBoundsWorldspace.MaxY;

                float ballRight = Ball.BoundingBoxTransformedToParent.MaxX;
                float ballLeft = Ball.BoundingBoxTransformedToParent.MaxY;

                bool shouldReflectRight =
                    ((screenLeftM < ballLeft) || (ballLeft < 0));
                bool shouldReflectLeft =
                    ((screenRightM < ballRight) || (ballRight < 0));

                if (shouldReflectRight)
                {
                    if (screenLeftM < ballLeft)
                    {
                        CountDownTimer.Visible = true;
                        Time = 3;
                        CanPlay = false;

                        ResetPositions();

                        ScoreP2++;
                        Score_P2.Text = ScoreP2.ToString();
                    }
                    else
                    {
                        CountDownTimer.Visible = true;
                        Time = 3;
                        CanPlay = false;

                        ResetPositions();

                        ScoreP1++;
                        Score_P1.Text = ScoreP1.ToString();
                    }

                    directionY = -directionY;
                }
                if (shouldReflectLeft)
                    ballVelocityX = -ballVelocityX;
            }
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            CCRect bounds = VisibleBoundsWorldspace;

            var touchListener = new CCEventListenerTouchAllAtOnce();

            touchListener.OnTouchesEnded = OnTouchesEnded;
            touchListener.OnTouchesMoved = OnTouchesMoved;

            AddEventListener(touchListener, this);
        }

        void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            var locationOnScreen = touches[0].Location;
            _player1Board.PositionX = locationOnScreen.X;
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {

            }
        }

        void ResetPositions()
        {
            _player1Board.PositionX = 100;
            _player1Board.PositionY = (Size_Y / 40) * 3;
            _player1Board.PositionX = 100;
            _player1Board.PositionY = (Size_Y / 40) * 3;
            Ball.Visible = false;
            Ball.PositionX = Size_X / 2;
            Ball.PositionY = Size_Y / 2;

            ballVelocityX = 0;
            ballVelocityY = 6000;
            directionY = 1;
        }
    }
}