
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
using System.Threading;
using System.IO;

namespace SafeTraveling
{
	[Activity (Label = "Utilizator_Trip_ModifyAccount_ConfirmPhoto")]			
	public class Utilizator_Trip_ModifyAccount_ConfirmPhoto : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			Bundle GetBytes = Intent.Extras;
			byte[] PhotoBytes = GetBytes.GetByteArray ("PhBytes");

			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Utilizator_Trip_ModifyAccount_ConfirmPhoto);

			ImageView Photo = FindViewById<ImageView> (Resource.Id.imageView1);
			Button Confim = FindViewById<Button> (Resource.Id.button1);
			Button Cancel = FindViewById<Button> (Resource.Id.button2);
			TripClient Me = Utilizator_Trip.Me;

			Photo.SetImageDrawable (DrawableConverter.ByteArrayToDrawable(PhotoBytes,this));

			Confim.Click += (object sender, EventArgs e) => {
				Dialog diag = new Dialog(this);
				diag.RequestWindowFeature((int)WindowFeatures.NoTitle);

				LinearLayout View = new LinearLayout(this);
				ProgressBar UploadProgress = new ProgressBar(this);

				diag.SetContentView (View);

				new Thread(()=>{
					using (MemoryStream ms = new MemoryStream(PhotoBytes))
					{
						int PackSize = 1000;
						int TotalLength = (int)ms.Length;
						int NoOfPackets = (int) Math.Ceiling((double)ms.Length / (double)PackSize);
						int CurrentPackSize;
						UploadProgress.Max = NoOfPackets;

						for (int i = 0;i<NoOfPackets;i++)
						{
							if (TotalLength>PackSize)
							{
								TotalLength -= PackSize;
								CurrentPackSize = PackSize;
							} else 
								CurrentPackSize = TotalLength;

							byte[] CurrentBytes = new byte[CurrentPackSize];
							int ReadedLength = ms.Read(CurrentBytes,0,CurrentBytes.Length);

							string CurrentString = Encoding.ASCII.GetString(CurrentBytes,0,ReadedLength);

							Me.OUTPUT_SEND(new string[] {_Details.GetUserDataByPhone,Me.NumarTelefon,CurrentString});
							UploadProgress.Progress = i;
						}
					}
				}).Start();
			};

			Cancel.Click += (object sender, EventArgs e) => {
				Finish();
			};
		}
	}
}

