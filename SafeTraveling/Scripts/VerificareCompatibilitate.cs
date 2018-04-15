using System;
using Android.App;
using Android.Views;
using Android.Net;
using Android.Widget;
using Java.Net;
using System.Threading;
using Android.Content;
using System.Net.Sockets;
using Android.Graphics.Drawables;
using System.Collections.Generic;

namespace SafeTraveling
{
	[Activity(ScreenOrientation=Android.Content.PM.ScreenOrientation.Landscape,NoHistory=true)]
	public class VerificareCompatibilitate : Activity
	{
		public static Drawable Background;

		ConnectivityManager ConMngr;
		Thread CheckInternetThread;
		Thread GifThread;
		Thread SwitchingBackgroundsThread;
		RelativeLayout Backgroud;
		ImageView iView;

		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			RequestWindowFeature (Android.Views.WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.VerificareCompatibilitate);

			Backgroud = FindViewById<RelativeLayout> (Resource.Id.relativeLayout1);
			iView = FindViewById<ImageView> (Resource.Id.imageView1);

			ConMngr = (ConnectivityManager)GetSystemService (ConnectivityService);
             
			SwitchingBackgrounds ();
            //RenderGif();

			CheckInternetThread = new Thread (new ThreadStart (() => {
				Thread.Sleep (1500);
				if (CheckInternetConnection ().Equals (true)) {
					Thread.Sleep (1500);
					if (CheckServerConnection ().Equals (true)) {
						Thread.Sleep (1500);

						Intent StartLogin = new Intent (this, typeof(Login));

						Background = Backgroud.Background;
						StartLogin.PutExtra ("BackgroundByteArray", DrawableConverter.DrawableToByteArray (Backgroud.Background));

                        SwitchingBackgroundsThread.Abort();

						StartActivity (StartLogin);
					} else {
					/*	AlertDialog.Builder aDiag = new AlertDialog.Builder (this);
						aDiag.SetTitle ("Error!");
						aDiag.SetMessage ("Nu s-a putut realiza conexiunea cu serverul!");
						aDiag.SetPositiveButton ("Skip", delegate(object sender, DialogClickEventArgs e) {*/
							Intent StartLogin = new Intent (this, typeof(Login));


							StartLogin.PutExtra ("BackgroundLoginByteArray", DrawableList.IndexOf(Backgroud.Background));

                            SwitchingBackgroundsThread.Abort();
                        //    GifThread.Abort();

							StartActivity (StartLogin);
						/*});

						aDiag.SetNegativeButton ("Exit", delegate(object sender, DialogClickEventArgs e) {
							RunOnUiThread (() => Toast.MakeText (this, "Couldn't reach the server!Please try again later!", ToastLength.Long).Show ());
							Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
						});

						RunOnUiThread (() => aDiag.Show ());*/
					}
				}
			}));
			CheckInternetThread.Start ();
		}

        List<Drawable> DrawableList = new List<Drawable>();
        private void SwitchingBackgrounds()
		{
			for (int i = 1; i < 5; i++)
				DrawableList.Add (DrawableConverter.GetDrawableFromAssets ("LB/img" + i.ToString () + ".jpg", this));
			for (int i = 1; i < 8; i++)
				DrawableList.Add (DrawableConverter.GetDrawableFromAssets ("LoadingBackgrounds/img" + i.ToString () + ".jpg", this));
		
            SwitchingBackgroundsThread = new Thread (new ThreadStart (() => {
				while (true) {
					int randId = new Random ().Next (0, DrawableList.Count);
                    
                    while(randId==3)
                        randId = new Random().Next(0, DrawableList.Count);

					RunOnUiThread (() => Backgroud.Background = DrawableList [randId]);
					Thread.Sleep (1000);
				}
			}));
			SwitchingBackgroundsThread.Start ();
		}

        private void RenderGif()
        {
            AnimationDrawable test = new AnimationDrawable();
            test.OneShot = false;

            for (int i = 1; i < 90; i++)
            {
                Drawable d = DrawableConverter.GetDrawableFromAssets("gif/i" + i.ToString() + ".png", this);
                test.AddFrame(d, 1);
            }

            iView.SetImageDrawable(test);
        }

		private bool CheckServerConnection()
		{
			TcpClient Client = new TcpClient();
			IAsyncResult Result = Client.BeginConnect("192.168.137.2", 2000, null, null);
			bool success = Result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
            success = true;
			//if (success)
			//	Client.EndConnect(Result);		
            
            return success;
		}

		private bool CheckInternetConnection()
		{
			/*if (IsNetworkOpen ()) {
				try {
					InetAddress IpAddress = InetAddress.GetByName ("google.com");

					if (IpAddress.Equals ("")) {
						RunOnUiThread (() => Toast.MakeText (this, "No internet connection!", ToastLength.Long).Show ());
						return false;
					} else
						return true;
				} catch {
					RunOnUiThread (() => Toast.MakeText (this, "No internet connection!", ToastLength.Long).Show ());
					return false;
				}
			} else {
				AlertDialog.Builder ErrorDiag = new AlertDialog.Builder(this);
				ErrorDiag.SetCancelable (false);
				ErrorDiag.SetMessage ("Error:No connection enabled!");
				ErrorDiag.SetPositiveButton ("Open settings", ((object sender, DialogClickEventArgs e) => {
					OpenSettings ();
				}));
				ErrorDiag.SetNegativeButton ("Exit",((object sender, DialogClickEventArgs e) => {
					Finish ();
				}));

				RunOnUiThread (() => ErrorDiag.Show ());
                //false*/
				return true;
			//}
		}

		private bool IsNetworkActive()
		{			
			return (ConMngr.ActiveNetworkInfo != null);
		}

		private bool IsNetworkOpen()
		{
			if (IsNetworkActive ())
				return (ConMngr.ActiveNetworkInfo.IsConnected);
			else 
				return false;
		}

		private void OpenSettings()
		{
			Intent OpnSettings = new Intent (Android.Provider.Settings.ActionSettings);
			StartActivityForResult (OpnSettings,0);
		}
	}
}

