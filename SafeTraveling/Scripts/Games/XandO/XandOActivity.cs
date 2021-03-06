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
using Android.Content.PM;
using CocosSharp;
using Android.Graphics;

namespace SafeTraveling.Scripts.Games.XandO
{
    [Activity(Label = "CocosSharpGame1",Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
    AlwaysRetainTaskState = true,
    LaunchMode = LaunchMode.SingleInstance,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class XandOActivity : Activity
    {
        string NrTel;
        string IntentType;
        string GameId;

        protected override void OnCreate(Bundle bundle)
        {
            Bundle GetIntentType = Intent.Extras;
            IntentType = (string)GetIntentType.Get(_Details.Game_IntentType);
            NrTel = (string)GetIntentType.Get(_Details.Game_Player_NrTel);
            GameId = (string)GetIntentType.Get(_Details.Game_GameId);

            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            SetContentView(Resource.Layout.Game_XandO);
            
            CCGameView gameView = (CCGameView)FindViewById(Resource.Id.GameView);
            gameView.ViewCreated += LoadGame;
        }

        void LoadGame(object sender, EventArgs e)
        {
            CCGameView gameView = sender as CCGameView;
            if (gameView != null)
            {
                var contentSearchPaths = new List<string>() {
					"Fonts",
					"Sounds"
				};
                CCSizeI viewSize = gameView.ViewSize;

                Display display = WindowManager.DefaultDisplay;
                Point Size = new Point();
                display.GetSize(Size);

                int width = Size.X - 1;
                int height = Size.Y - 1;
                gameView.DesignResolution = new CCSizeI(width, height);

                if (width < viewSize.Width)
                {
                    contentSearchPaths.Add("Images/Hd");
                    CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
                }
                else
                {
                    contentSearchPaths.Add("Images/Ld");
                    CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
                }
                gameView.ContentManager.SearchPaths = contentSearchPaths;
                CCScene gameScene = new CCScene(gameView);
                gameScene.AddLayer(new GameLayer(this, width, height, IntentType,NrTel,GameId));
                gameView.RunWithScene(gameScene);
            }
        }
    }
}