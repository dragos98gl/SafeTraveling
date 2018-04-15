
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
using System.Drawing;
using System.IO;
using Android.Graphics;
using System.Net.Sockets;
using Android.Provider;
using Java.Lang;

namespace SafeTraveling
{
    [Activity(Label = "Utilizator_Trip_ModifyAccount", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class Utilizator_Trip_ModifyAccount : Activity
    {
        string Nume;
        string Prenume;
        string Varsta;
        string Sex;
        string Email;
        string PhotoString;
        TripClient Me;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            Bundle Extras = Intent.Extras;
            byte[] BackgroundByteArray = Extras.GetByteArray("BackgroundByteArray");

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Utilizator_Trip_ModifyAccount);

            ListView RestulDatelor = FindViewById<ListView>(Resource.Id.listView1);
            RelativeLayout Background = FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
            ImageView ProfilePic = FindViewById<ImageView>(Resource.Id.imageView1);
            ImageView ChangeProfilePic = FindViewById<ImageView>(Resource.Id.imageView2);
            TextView ChangeProfilePic_TV = FindViewById<TextView>(Resource.Id.textView2);

            Background.Background = DrawableConverter.ByteArrayToDrawable(BackgroundByteArray, this);
            SetTypeface.Bold.SetTypeFace(this, ChangeProfilePic_TV);

            ChangeProfilePic.Click += (object sender, EventArgs e) =>
            {
                string Path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures) + "/temp";
                Java.IO.File file = new Java.IO.File(Path, string.Format("temp.jpg", Guid.NewGuid()));

                Intent i = new Intent(MediaStore.ActionImageCapture);
                i.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(file));
                StartActivityForResult(i, 1);
                //Recreate();
            };

            Utilizator_Trip.test.ClientGetMessage += (object sender, ClientGetMessageEventArgs e) =>
            {
                if (e.Messages[0].Equals(_Details.GetUserDataByPhone))
                {
                    TcpClient Client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
                    NetworkStream ns = Client.GetStream();

                    _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.GetProfilePic, Me.NumarTelefon }));

                    int ReadedBytes;
                    byte[] Buffer = new byte[1000];
                    MemoryStream ms = new MemoryStream();

                    while ((ReadedBytes = ns.Read(Buffer, 0, Buffer.Length)) > 0)
                    {
                        ms.Write(Buffer, 0, ReadedBytes);
                    }

                    Client.Close();
                    ns.Dispose();

                    if (!e.Messages[2].Equals(string.Empty))
                    {
                        Nume = e.Messages[1];
                        Prenume = e.Messages[2];
                        Varsta = e.Messages[3];
                        Sex = e.Messages[4];
                        Email = e.Messages[5];

                        RunOnUiThread(() =>
                        {
                            RestulDatelor.Adapter = new Utilizator_Trip_ModifyAccount_Adapter(this, new string[] { "Nume:" + Nume, "Prenume:" + Prenume, "Varsta:" + Varsta, "Sex:" + Sex, "E-mail:" + Email });

                            Bitmap bitMap = ((BitmapDrawable)DrawableConverter.ByteArrayToDrawable(ms.ToArray(), this)).Bitmap;
                            ProfilePic.SetImageDrawable(new BitmapDrawable(Resources, RoundedBitmap.MakeRound(bitMap, bitMap.Height / 2)));
                        });
                    }
                }
                else if (e.Messages[0].Equals(_Details.EditUserInfo))
                {
                    switch (e.Messages[1])
                    {
                        case "1":
                            {
                                RunOnUiThread(() => Toast.MakeText(this, "Succes", ToastLength.Short).Show());
                                RunOnUiThread(() => Recreate());
                            } break;
                        case "0":
                            {
                                RunOnUiThread(() => Toast.MakeText(this, "Fail", ToastLength.Short).Show());
                            } break;
                    }
                }
            };

            Me = Utilizator_Trip.Me;
            Me.OUTPUT_SEND(new string[] { _Details.GetUserDataByPhone, Me.NumarTelefon });
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

                            _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.UpdateProfilePic, Me.NumarTelefon }));

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

                            Intent intent = Intent;
                            Finish();
                            StartActivity(intent);
                        }
                    }).Start();
                }
                catch
                {
                }
        }
    }

	public class Utilizator_Trip_ModifyAccount_Adapter:ArrayAdapter<string>
	{
		Activity context;
		string[] Campuri;

		public Utilizator_Trip_ModifyAccount_Adapter (Activity context,string[] Campuri):base(context,Resource.Layout.Utilizator_Trip_ModifyAccount_Adapter,Campuri)
		{
			this.context = context;
			this.Campuri = Campuri;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			LayoutInflater inflater = context.LayoutInflater;
			View v = inflater.Inflate (Resource.Layout.Utilizator_Trip_ModifyAccount_Adapter, null, true);

			TextView CurrentCamp = v.FindViewById<TextView> (Resource.Id.textView1);
			ImageView EditBtn = v.FindViewById<ImageView> (Resource.Id.imageView1);

            SetTypeface.Bold.SetTypeFace(context, CurrentCamp);

			CurrentCamp.Text = Campuri[position];

			EditBtn.Click += (object sender, EventArgs e) => {
                Dialog diag = new Dialog(context);
                diag.Window.RequestFeature(WindowFeatures.NoTitle);

                View CostumView = inflater.Inflate(Resource.Layout.Utilizator_Trip_TripInfo_AlertDialogAdapter, null);
                TextView NumeCamp = CostumView.FindViewById<TextView>(Resource.Id.textView1);
                ImageView EditInfo = CostumView.FindViewById<ImageView>(Resource.Id.imageView1);
                EditText NewValue = CostumView.FindViewById<EditText>(Resource.Id.editText1);

                NumeCamp.Text = "Introduceti noul " + Campuri[position].Split(':')[0].ToLower();
                EditInfo.Click += (object sender1, EventArgs e1) =>
                {
                    TripClient Me = Utilizator_Trip.Me;
                    Me.OUTPUT_SEND(new string[] { _Details.EditUserInfo, Campuri[position].Split(':')[0].ToLower(), Me.NumarTelefon, NewValue.Text });
                    diag.Cancel();
                };

                diag.SetContentView(CostumView);

                diag.SetCanceledOnTouchOutside(true);
                diag.Show();
                diag.Window.SetBackgroundDrawable(context.Resources.GetDrawable(Resource.Drawable.background_MarginiOvaleAlb));
			};

			return v;
		}
	}
}