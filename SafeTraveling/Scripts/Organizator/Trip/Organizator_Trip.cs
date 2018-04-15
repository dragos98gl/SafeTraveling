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
using Android.Graphics.Drawables;
using Java.Lang;
using SafeTraveling.Scripts.Organizator.Trip;
using System.Net.Sockets;
using System.IO;
using Android.Graphics;

namespace SafeTraveling
{
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class Organizator_Trip : Activity
	{
		GridView IconsContainter;
		RelativeLayout Background;

        public static int Distanta;
        ListView LeftDrawerListView;
        ImageView UserProfilePic;
        TextView UserNume;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

			Bundle Extras = Intent.Extras;
			byte[] BackgroundByteArray = Extras.GetByteArray ("BackgroundByteArray");

			base.OnCreate (savedInstanceState);
            Initiate();

			SetContentView (Resource.Layout.Organizator_Trip);

            CheckIfFirstLogin();

            IconsContainter = FindViewById<GridView>(Resource.Id.GridViewLay);
            Background = FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
            LeftDrawerListView = FindViewById<ListView>(Resource.Id.left_drawer_listview);
            UserProfilePic = FindViewById<ImageView>(Resource.Id.imageView1);
            UserNume = FindViewById<TextView>(Resource.Id.textView1);

			Background.Background = DrawableConverter.ByteArrayToDrawable (BackgroundByteArray,this);

            SetTypeface.Normal.SetTypeFace(this, UserNume);

            IconsContainter.Adapter = new Organizator_Trip_GridViewAdapter(this, new Drawable[] { Resources.GetDrawable(Resource.Drawable.Chat), Resources.GetDrawable(Resource.Drawable.VizualizareExcursionisti), Resources.GetDrawable(Resource.Drawable.Galerie), Resources.GetDrawable(Resource.Drawable.Distance),Resources.GetDrawable(Resource.Drawable.QuestionPool) });
            LeftDrawerListView.Adapter = new Organizator_Trip_LeftDrawer(this, Background.Background, new string[] { "Informatii excursie" });

			IconsContainter.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
				Toast.MakeText(this,e.Position.ToString(),ToastLength.Short).Show();
			};
		}

        private void Initiate()
        {
            SaveUsingSharedPreferences Save = new SaveUsingSharedPreferences(this);
            string DistanceString = Save.LoadString(SaveUsingSharedPreferences.Tags.Organizator.Distanta);
            if (DistanceString != "")
                Distanta = int.Parse(Save.LoadString(SaveUsingSharedPreferences.Tags.Organizator.Distanta));
            else{
                Save.Save(SaveUsingSharedPreferences.Tags.Organizator.Distanta,"50");
                Distanta = 50;
            }
        }

        private void CheckIfFirstLogin()
        {
            SaveUsingSharedPreferences Save = new SaveUsingSharedPreferences(this);

         ////   if (Save.LoadString(SaveUsingSharedPreferences.Tags.Trip.TipId).Equals(string.Empty))
           // {
                if (!IsServiceStarted(Class.FromType(typeof(StartUpService))))
                {
                   Intent StartServices = new Intent(this, typeof(StartUpService));
                   StartService(StartServices);
                }
           // }
        }

        private bool IsServiceStarted(Class serviceClass)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(ActivityService);

            foreach (Android.App.ActivityManager.RunningServiceInfo service in manager.GetRunningServices(int.MaxValue))
                if (serviceClass.Name.Equals(service.Service.ClassName))
                    return true;
            return false;
        }

        protected override void OnResume()
        {
            TcpClient Client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
            NetworkStream ns = Client.GetStream();

            _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.GetProfilePic, new SaveUsingSharedPreferences(this).LoadString(SaveUsingSharedPreferences.Tags.Login.Username) }));

            int ReadedBytes;
            byte[] Buffer = new byte[1000];
            MemoryStream ms = new MemoryStream();

            while ((ReadedBytes = ns.Read(Buffer, 0, Buffer.Length)) > 0)
            {
                ms.Write(Buffer, 0, ReadedBytes);
            }

            Client.Close();
            ns.Dispose();

            Bitmap bitMap = ((BitmapDrawable)DrawableConverter.ByteArrayToDrawable(ms.ToArray(), this)).Bitmap;
            UserProfilePic.SetImageDrawable(new BitmapDrawable(Resources, RoundedBitmap.MakeRound(bitMap, bitMap.Height / 2)));

            TcpClient client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
            NetworkStream Ns = client.GetStream();

            _TcpDataExchange.WriteStreamString(Ns, CryptDecryptData.CryptData(new string[] { _Details.GetUserName, new SaveUsingSharedPreferences(this).LoadString(SaveUsingSharedPreferences.Tags.Login.Username) }));
            string NumeUtilizator = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(Ns))[0];
            UserNume.Text = NumeUtilizator;

            client.Close();
            Ns.Dispose();

            base.OnResume();
        }
	}
}

