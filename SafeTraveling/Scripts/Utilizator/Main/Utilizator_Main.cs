
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
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Net.Sockets;
using System.IO;
using Android.Graphics;

namespace SafeTraveling
{
	[Activity (Label = "Utilizator_Main",ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,NoHistory = true)]			
	public class Utilizator_Main : FragmentActivity
	{
		RelativeLayout Background;
		RelativeLayout Container;
		EditText Cod;
		TextView IntroducetiCodul;
		ListView LeftDrawer;
		Button Next;
		ImageView UserProfilePic;
		TextView UserNume;

		public static Client INPUT;
		public static Client OUTPUT;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

			Bundle Extras = Intent.Extras;
			int BackgroundByteArray = Extras.GetInt ("BackgroundUtilizatorMainByteArray");

			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Utilizator_Main);

			Background = FindViewById<RelativeLayout> (Resource.Id.relativeLayout1);
			LeftDrawer = FindViewById<ListView> (Resource.Id.left_drawer_listview);
			Next = FindViewById<Button> (Resource.Id.button1);
			Container = FindViewById<RelativeLayout> (Resource.Id.relativeLayout3);
			Cod = FindViewById <EditText> (Resource.Id.editText1);
			IntroducetiCodul = FindViewById<TextView> (Resource.Id.textView2);
			UserProfilePic = FindViewById<ImageView> (Resource.Id.imageView1);
			UserNume = FindViewById<TextView> (Resource.Id.textView1);

            List<Drawable> DrawableList = new List<Drawable>();
            for (int i = 1; i < 5; i++)
                DrawableList.Add(DrawableConverter.GetDrawableFromAssets("LB/img" + i.ToString() + ".jpg", this));
            for (int i = 1; i < 8; i++)
                DrawableList.Add(DrawableConverter.GetDrawableFromAssets("LoadingBackgrounds/img" + i.ToString() + ".jpg", this));
            Background.Background = DrawableList[BackgroundByteArray];

            Cod.Text = new SaveUsingSharedPreferences(this).LoadString(SaveUsingSharedPreferences.Tags.Trip.TipId);
            //Cod.Text = "c41z1zb6xc";

			SetTypeface.Normal.SetTypeFace (this,IntroducetiCodul);
			SetTypeface.Normal.SetTypeFace (this,Next);
			SetTypeface.Normal.SetTypeFace (this,Cod);

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

			SetTypeface.Normal.SetTypeFace (this,UserNume);
			LeftDrawer.Adapter = new Utilizator_Main_LeftDrawerAdapter (this,new string[] {"Logout"},Background.Background);

			Next.Click += (object sender, EventArgs e) => {
				TcpClient OUTPUT_Client = new TcpClient (_Details.ServerIP,_Details.TripPort_INPUT);
				NetworkStream OUTPUT_ns = OUTPUT_Client.GetStream();
				OUTPUT = new Client(OUTPUT_Client,OUTPUT_ns);

				string NumePrenume = "nume Prenume";
                string NumarTelefon = new SaveUsingSharedPreferences(this).LoadString(SaveUsingSharedPreferences.Tags.Login.Username);
                string TripId = Cod.Text;

				_TcpDataExchange.WriteStreamString(OUTPUT_ns,CryptDecryptData.CryptData(new String [] {"TRIPENTER",NumePrenume,NumarTelefon,TripId}));

				List<string> Messages = new List<string>();
				Messages.AddRange(CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(OUTPUT_ns)));
				Messages.Add(TripId);

				TcpClient INPUT_Client = new TcpClient (_Details.ServerIP,_Details.TripPort_OUTPUT);
				NetworkStream INPUT_ns = INPUT_Client.GetStream();
				INPUT = new Client(INPUT_Client,INPUT_ns);

				_TcpDataExchange.WriteStreamString(INPUT_ns,CryptDecryptData.CryptData(new String [] {NumePrenume,NumarTelefon,TripId}));

                new SaveUsingSharedPreferences(this).Save(SaveUsingSharedPreferences.Tags.Trip.TipId, TripId);
                Intent StartUtilizatorTrip = new Intent(this, typeof(Utilizator_Trip));
				StartUtilizatorTrip.PutExtra ("BackgroundByteArray" , DrawableConverter.DrawableToByteArray(Background.Background));
				StartUtilizatorTrip.PutStringArrayListExtra ("TripData" ,Messages);
				StartActivity(StartUtilizatorTrip);
			};
		}
	}
}

