 using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media;
using Android.Views;
using Android.Locations;
using Android.Content;

namespace SafeTraveling
{
	[Activity (MainLauncher = true,Label = "Safe Traveling", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,NoHistory=true)]
	public class MainActivity : Activity,ISurfaceHolderCallback
	{
		VideoView videoView;
		MediaPlayer player;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

			//Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Intro);

			//new SaveUsingSharedPreferences (this).Clear (SaveUsingSharedPreferences.Tags.Trip.TipId);

			videoView = FindViewById<VideoView> (Resource.Id.videoView1);

			StartIntro ("SafeTravelingIntro.mp4");
		}

		private void StartIntro(string IntroVideoPath)
		{
			ISurfaceHolder holder = videoView.Holder;
			holder.SetType (SurfaceType.PushBuffers);
			holder.AddCallback( this );
			player = new  MediaPlayer ();
			player.Completion+= Player_Completion;
			Android.Content.Res.AssetFileDescriptor afd = this.Assets.OpenFd(IntroVideoPath);
			if  (afd != null )
			{
				player.SetDataSource (afd.FileDescriptor, afd.StartOffset, afd.Length);
				player.Prepare ();
				player.Start ();
			}
		}

		void Player_Completion (object sender, System.EventArgs e)
		{
			StartActivity (typeof(VerificareCompatibilitate));
		}

		public void SurfaceChanged (ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
		{
			//throw new System.NotImplementedException ();
		}

		public void SurfaceCreated (ISurfaceHolder holder)
		{
			player.SetDisplay (holder);
		}

		public void SurfaceDestroyed (ISurfaceHolder holder)
		{
			//throw new System.NotImplementedException ();
		}
	}
}
