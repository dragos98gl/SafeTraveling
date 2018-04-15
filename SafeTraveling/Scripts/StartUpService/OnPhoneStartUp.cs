using System;
using Android.Content;
using Android.App;
using System.Threading;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Locations;
using System.IO;
using System.Net.Sockets;
using Android.Provider;

namespace SafeTraveling
{
	[BroadcastReceiver]
	[IntentFilter(new[] {Intent.ActionBootCompleted})]
	public class OnPhoneStartUp:BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			Toast.MakeText (context,new SaveUsingSharedPreferences (context).LoadString (SaveUsingSharedPreferences.Tags.Trip.TipId),ToastLength.Short).Show();

			if (new SaveUsingSharedPreferences (context).LoadString (SaveUsingSharedPreferences.Tags.Trip.TipId) != string.Empty) {
				Intent ServiceIntent = new Intent (context, typeof(StartUpService));
				context.StartService (ServiceIntent);
			}
		}
	}

	[Service]
	[IntentFilter(new String[]{".StartUpService"})]
	public class StartUpService: IntentService
	{
		public StartUpService () : base("StartUpService")
		{
		}

		protected override void OnHandleIntent (Intent intent)
		{
		}

		public override void OnCreate ()
		{
			base.OnCreate ();
		}

		public override void OnTaskRemoved (Intent rootIntent)
		{
			Intent RestartService = new Intent (this,typeof(StartUpService));
			RestartService.SetPackage (PackageName);

			PendingIntent RestartServicePendingIntent = PendingIntent.GetService (ApplicationContext,1,RestartService,PendingIntentFlags.OneShot);
			AlarmManager alarmService = (AlarmManager) ApplicationContext.GetSystemService(AlarmService);
			alarmService.Set (AlarmType.ElapsedRealtime,SystemClock.ElapsedRealtime()+1000,RestartServicePendingIntent);

			base.OnTaskRemoved (rootIntent);
		}

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            NotificationCompat.Builder nBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.NotificationIcon)
                .SetContentTitle("Safe Traveling")
                .SetSound(Settings.System.DefaultNotificationUri)
                .SetContentText("Application service has successfully started!");

            NotificationManager nManager = (NotificationManager)this.GetSystemService(Context.NotificationService);
            nManager.Notify(0, nBuilder.Build());

            string NumarTelefon = new SaveUsingSharedPreferences(this).LoadString(SaveUsingSharedPreferences.Tags.Login.Username);
            string TripId = new SaveUsingSharedPreferences(this).LoadString(SaveUsingSharedPreferences.Tags.Trip.TipId);

            Handler mHandler = new Handler(Looper.MainLooper);
            new Thread(new ThreadStart(() =>
            {
                TcpClient Client = new TcpClient(_Details.ServerIP, _Details.LocationServicePort);
                NetworkStream ns = Client.GetStream();

                _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { TripId, NumarTelefon }));

                LocationManager lm = (LocationManager)GetSystemService(LocationService);
                ILocationListener gpsListener = new LocationListener(Client, ns);

                mHandler.Post(() =>
                    lm.RequestLocationUpdates(LocationManager.NetworkProvider, 5000, 1, gpsListener)
                );
            })).Start();

            new Thread(new ThreadStart(() =>
            {
                TcpClient Client = new TcpClient(_Details.ServerIP, _Details.NotificationServicePort);
                NetworkStream ns = Client.GetStream(); 


                _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { TripId, NumarTelefon }));

                new SafeTraveling.Scripts.StartUpService.NotificationService(this, new Client(Client, ns),NumarTelefon);
            })).Start();
            
            return StartCommandResult.Sticky;
        }
	}
}

