using System;
using Android.Support.V4.App;
using Android.App;
using Android.Content;

namespace SafeTraveling
{
	public static class Notification1
	{
		public static void Show (ref Context context)
		{
			NotificationCompat.Builder nBuilder = new NotificationCompat.Builder (context)
				.SetSmallIcon (Resource.Drawable.Icon)
				.SetContentTitle ("Safe Traveling")
				.SetContentText ("Application service has successfully started!");

			NotificationManager nManager = (NotificationManager)context.GetSystemService (Context.NotificationService);
			nManager.Notify (0,nBuilder.Build());
		}
	}
}

