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
using Android.Support.V4.App;
using Android.Support.V4.View;
using Java.Lang;
using System.IO;
using Android.Graphics;
using System.Net.Sockets;

namespace SafeTraveling
{
	public class ClientGetMessageEventArgs:EventArgs { public string[] Messages; }

    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class Utilizator_Trip : FragmentActivity
	{
		ListView LeftDrawerListView;public ListView RightDrawerListView;ViewPager Pager;TabHost tabHost;ImageView UserProfilePic;ImageView OrganizatorProfilePic;TextView UserNume;TextView OrganizatorNume;RelativeLayout Background;
		public static TripClient Me;
		public static string [] TripInfo;
        public static string TripId;

		public delegate void ClientGetMessageHandler (object sender,ClientGetMessageEventArgs e);
		public event ClientGetMessageHandler ClientGetMessage;

		public static Utilizator_Trip test;

		public virtual void OnClientGetMessage(string[] Msgs)
		{
			if (ClientGetMessage != null) {
				ClientGetMessage (this,new ClientGetMessageEventArgs() { Messages = Msgs});
			}
		}

		int[] TabsIcon = new int[] {
			Resource.Drawable.Chat,
			Resource.Drawable.TakePhoto,
			Resource.Drawable.Icon };

		List<string> Tags = new List<string> (new string[] {
			"MaskA",
			"MaskB",
			"MaskC"
		});

		protected override void OnCreate (Bundle savedInstanceState)
		{
			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

			Bundle Extras = Intent.Extras;
			byte[] BackgroundByteArray = Extras.GetByteArray ("BackgroundByteArray");
			TripInfo = Extras.GetStringArrayList ("TripData").ToArray();
			Me = new TripClient (Utilizator_Main.INPUT, Utilizator_Main.OUTPUT,new SaveUsingSharedPreferences(this).LoadString(SaveUsingSharedPreferences.Tags.Login.Username) ,this); 
            TripId = new SaveUsingSharedPreferences(this).LoadString(SaveUsingSharedPreferences.Tags.Trip.TipId);

			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Utilizator_Trip);

			InitiateView ();
			CheckIfNewInTrip ();

			test = this;

			LocalActivityManager LAM = new LocalActivityManager (this,false);
			LAM.DispatchCreate (savedInstanceState);
			LAM.DispatchResume ();
			LAM.DispatchPause (IsFinishing);
			tabHost.Setup (LAM);

			int[] Layouturi = new int[] { 
				Resource.Layout.Utilizator_Trip_ChatView,
				Resource.Layout.Utilizator_Trip_MaskB,
				Resource.Layout.Utilizator_Trip_MaskC,
			};

			Background.Background = DrawableConverter.ByteArrayToDrawable (BackgroundByteArray,this);
            
            SetTypeface.Normal.SetTypeFace (this,UserNume);
            LeftDrawerListView.Adapter = new Utilizator_Trip_LeftDrawerAdapter(this, Background.Background, new string[] { "Galerie", "Modificare Cont","As vrea sa cumpar", "Informatii excursie", "Setari", "Iesire Excursie", "Logut" });

		//	OrganizatorProfilePic.SetImageDrawable(RoundedUserProfile);
		//	OrganizatorNume.Text = "Nume Prenume";
		//	SetTypeface.Normal.SetTypeFace (this,OrganizatorNume);

			Pager.Adapter = new Utilizator_Trip_ViewPagerAdapter (SupportFragmentManager,this);

			for (int i=0;i<3;i++)
				CreateTab (typeof(Utilizator_Trip_MaskA),Tags[i],string.Empty,TabsIcon[i],tabHost);

			tabHost.TabChanged+= TabHost_TabChanged;
			Pager.PageSelected += Pager_PageSelected;
			ClientGetMessage += Utilizator_Trip_ClientGetMessage; 
		}



        List<OnlineClient> OnlineClients = new List<OnlineClient>();
        void Utilizator_Trip_ClientGetMessage(object sender, ClientGetMessageEventArgs e)
        {
            if (e.Messages[0] == "ClientEnterExit")
            {
                if (e.Messages[1] == "1")
                {
                    OnlineClient OC = new OnlineClient(this, e.Messages);
                    OnlineClients.Add(OC);
                }
                else
                {
                    OnlineClient OC = new OnlineClient(this, e.Messages);
                    OnlineClients.Add(OC);

                    RunOnUiThread(() =>
                    {
                        if (OnlineClients.Count!=0)
                           RightDrawerListView.Adapter = new Utilizator_Trip_RightDrawerAdapter(this, OnlineClients.ToArray());
                    });
                    OnlineClients = new List<OnlineClient>();
                }
            }
        }

		private void CreateTab(Type activityType,string tag,string lavel,int drawableId,TabHost tabHost)
		{
			Intent intent = new Intent (this,activityType);
			intent.AddFlags (ActivityFlags.NewTask);

			TabHost.TabSpec specs = tabHost.NewTabSpec (tag);
			Drawable drawableIcon = Resources.GetDrawable (drawableId);
			specs.SetIndicator (lavel,drawableIcon);
			specs.SetContent (intent);

			tabHost.AddTab (specs);
		}

		void Pager_PageSelected (object sender, ViewPager.PageSelectedEventArgs e)
		{
			tabHost.SetCurrentTabByTag(Tags[e.Position]);
		}

		void TabHost_TabChanged (object sender, TabHost.TabChangeEventArgs e)
		{
			if (Tags.IndexOf(e.TabId.ToString())==Tags.IndexOf(tabHost.CurrentTabTag))
				Pager.SetCurrentItem(Tags.IndexOf(e.TabId.ToString()),true);
		}

		private void CheckIfNewInTrip()
		{
			SaveUsingSharedPreferences Save = new SaveUsingSharedPreferences (this);

		//	if (Save.LoadString (SaveUsingSharedPreferences.Tags.Trip.TipId).Equals(string.Empty)) {
			//	Save.Save (SaveUsingSharedPreferences.Tags.Trip.TipId,TripId);

				if (!IsServiceStarted(Class.FromType(typeof(StartUpService)))) {
					Intent StartServices = new Intent (this, typeof(StartUpService));
					StartService (StartServices);
				}
			//}
		}

        private bool IsServiceStarted(Class serviceClass)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(ActivityService);

            foreach (Android.App.ActivityManager.RunningServiceInfo service in manager.GetRunningServices(int.MaxValue))
                if (serviceClass.Name.Equals(service.Service.ClassName))
                    return true;
            return false;
        }

		private void InitiateView()
		{
			LeftDrawerListView = FindViewById<ListView> (Resource.Id.left_drawer_listview);
			RightDrawerListView = FindViewById<ListView> (Resource.Id.right_drawer_listview);
			Pager = FindViewById<ViewPager> (Resource.Id.viewPager1);
			tabHost = FindViewById<TabHost> (Resource.Id.tabHost);
			UserProfilePic = FindViewById<ImageView> (Resource.Id.imageView1);
			UserNume = FindViewById<TextView> (Resource.Id.textView1);
			OrganizatorProfilePic = FindViewById <ImageView> (Resource.Id.imageView2);
			OrganizatorNume = FindViewById <TextView> (Resource.Id.textView2);
			Background = FindViewById <RelativeLayout> (Resource.Id.relativeLayout4);
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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode.Equals(Result.Ok))
                try
                {
                    string Path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures) + "/temp/temp.jpg";
                    Java.IO.File file = new Java.IO.File(Path);

                    MemoryStream mss = new MemoryStream();

                    Bitmap b = BitmapFactory.DecodeFile(file.Path);
                    b.Compress(Bitmap.CompressFormat.Jpeg, 100, mss);

                    byte[] bytes = mss.ToArray();

                    file.Delete();

                    Dialog diag = new Dialog(this);
                    diag.RequestWindowFeature((int)WindowFeatures.NoTitle);

                    LinearLayout View = new LinearLayout(this);
                    ProgressBar UploadProgress = new ProgressBar(this);

                    diag.SetContentView(View);
                    diag.Show();

                    new System.Threading.Thread(() =>
                    {
                        TcpClient Client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
                        NetworkStream ns = Client.GetStream();

                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            int PackSize = 1000;
                            int TotalLength = (int)ms.Length;
                            int NoOfPackets = (int)System.Math.Ceiling((double)ms.Length / (double)PackSize);
                            int CurrentPackSize;
                            UploadProgress.Max = NoOfPackets;

                            _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.AddPhotoToGallery, TripId }));

                            for (int i = 0; i < NoOfPackets; i++)
                            {
                                if (TotalLength > PackSize)
                                {
                                    TotalLength -= PackSize;
                                    CurrentPackSize = PackSize;
                                }
                                else
                                    CurrentPackSize = TotalLength;

                                byte[] CurrentBytes = new byte[CurrentPackSize];
                                int ReadedLength = ms.Read(CurrentBytes, 0, CurrentBytes.Length);

                                ns.Write(CurrentBytes, 0, ReadedLength);
                            }

                            Client.Close();
                            diag.Cancel();
                        }
                    }).Start();
                }
                catch
                {
                }
        }
	}
}

