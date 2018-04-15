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

namespace SafeTraveling.Scripts.Games.Bounce_Game
{
    class Class1:CCLayerColor
    {
        public static CCSprite _player1Board;
        public static CCSprite _player2Board;
        CCSprite Ball;

        CCLabel Score_P1;
        CCLabel Score_P2;
        CCLabel CountDownTimer;

        Activity context;
        bool finish = true;

        public Class1(Activity context, Dialog diag)
            : base(CCColor4B.Black)
        {
            this.context = context;
          //  if (diag != null)
           //     diag.Cancel();

            _player2Board = new CCSprite("Board1.png");
            _player2Board.PositionX = 924;
            _player2Board.PositionY = 668;

            _player1Board = new CCSprite("Board1.png");
            _player1Board.PositionX = 100;
            _player1Board.PositionY = 100;

            Ball = new CCSprite("Ball1");
            Ball.Visible = false;
            Ball.PositionX = 500;
            Ball.PositionY = 300;

            Score_P1 = new CCLabel("0", "Arial", 200, CCLabelFormat.SystemFont);
            Score_P1.PositionX = 50;
            Score_P1.PositionY = 500;
            Score_P1.Color = CCColor3B.White;
            Score_P1.AnchorPoint = CCPoint.AnchorUpperLeft;

            Score_P2 = new CCLabel("0", "Arial", 200, CCLabelFormat.SystemFont);
            Score_P2.PositionX = 50;
            Score_P2.PositionY = 400;
            Score_P2.Color = CCColor3B.White;
            Score_P2.AnchorPoint = CCPoint.AnchorUpperLeft;

            CountDownTimer = new CCLabel("0", "Arial", 400, CCLabelFormat.SystemFont);
            CountDownTimer.PositionX = 500;
            CountDownTimer.Text = "3";
            CountDownTimer.PositionY = 500;
            CountDownTimer.Color = CCColor3B.White;
            CountDownTimer.AnchorPoint = CCPoint.AnchorUpperLeft;

            AddChild(Score_P1);
            AddChild(Score_P2);
            AddChild(_player1Board);
            AddChild(_player2Board);
            AddChild(Ball);
            AddChild(CountDownTimer);
        }
    }
}