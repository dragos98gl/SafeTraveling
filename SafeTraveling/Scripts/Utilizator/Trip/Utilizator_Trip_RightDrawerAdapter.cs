using System;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.App;
using Android.Views;
using System.Collections.Generic;
using Android.Graphics;
using System.Text;
using System.IO;
using System.Net.Sockets;
using Android.Content;
using Java.Lang;
using SafeTraveling.Scripts.Games.XandO;
using SafeTraveling.Scripts.Utilizator.Trip.Game_Invite;

namespace SafeTraveling
{
	public class Utilizator_Trip_RightDrawerAdapter:ArrayAdapter<OnlineClient>
	{
		Activity context;
		public OnlineClient[] Clients;

		public Utilizator_Trip_RightDrawerAdapter (Activity context,OnlineClient[] Clients):base (context,Resource.Layout.Utilizator_Trip_RightDrawerAdapter,Clients)
		{
			this.context = context;
			this.Clients = Clients;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			LayoutInflater inflater = context.LayoutInflater;
			View v = inflater.Inflate (Resource.Layout.Utilizator_Trip_RightDrawerAdapter,null,true);

			RelativeLayout RelLay = v.FindViewById <RelativeLayout> (Resource.Id.relativeLayout1);

			TextView NumeTView = v.FindViewById <TextView> (Resource.Id.textView1);
			ImageView PozaIView = v.FindViewById <ImageView> (Resource.Id.imageView1);

			PozaIView.SetImageDrawable(Clients[position].Photo);
			NumeTView.Text = Clients [position].Nume + " " + Clients [position].Prenume;

			SetTypeface.Normal.SetTypeFace (context,NumeTView);

			NumeTView.Click += (object sender, EventArgs e) => {
				PopupMenu popup = new PopupMenu (context,v);
				popup.MenuItemClick += (object s, PopupMenu.MenuItemClickEventArgs e1) => {
                    switch (e1.Item.ToString())
                    {
                        case "Invita la un joc":
                            {
                                Intent InviteGame = new Intent(context, typeof(Utilizator_Trip_InviteToGame));
                                InviteGame.PutExtra(_Details.Game_Player_NrTel, Clients[position].NumarTelefon);
                                context.StartActivity(InviteGame);
                            } break;
                        case "Suna":
                            {
                                Intent Suna = new Intent(Intent.ActionView, Android.Net.Uri.Parse("tel:" + Clients[position].NumarTelefon));
                                context.StartActivity(Suna);
                            } break;
                        case "Mesaj SMS":
                            {
                                Intent SMS = new Intent(Intent.ActionView, Android.Net.Uri.Parse("sms:" + Clients[position].NumarTelefon));
                                context.StartActivity(SMS);
                            } break;
                        case "Cere pozitia":
                            {
                                foreach(OnlineClient oc in Clients)
                                    if (oc.NumarTelefon == Utilizator_Trip.Me.NumarTelefon)
                                    {
                                        string myPos=oc.Position;
                                        string userPos = Clients[position].Position;
                                        Intent ShowDirections = new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://maps.google.com/maps?saddr="+myPos+"&daddr=" + userPos));
                                        context.StartActivity(ShowDirections);
                                    }
                        }break;
                    }
				};
				MenuInflater menuInflater = popup.MenuInflater;
				menuInflater.Inflate (Resource.Menu.UtilizatoriMenu,popup.Menu);
				popup.Show();
			};

			return v;
		}

		private int PositionBySender(object sender)
		{
			List<string> ListNume = new List<string> ();
			foreach (OnlineClient oc in Clients)
				ListNume.Add (oc.Nume + " " + oc.Prenume);

			return ListNume.IndexOf (((TextView)sender).Text);
		}
	}

    public class OnlineClient
    {
        Utilizator_Trip context;
        public string Nume;
        public string Prenume;
        public string TipCont;
        public string NumarTelefon;
        public string Position;

        public Drawable Photo;

        public OnlineClient(Utilizator_Trip context, string[] Messages)
        {
            this.context = context;

            Nume = Messages[2];
            Prenume = Messages[3];
            TipCont = Messages[4];
            NumarTelefon = Messages[5];
            Position = Messages[7];

            TcpClient Client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
            NetworkStream ns = Client.GetStream();

            using (MemoryStream ms = new MemoryStream())
            {
                _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.GetProfilePic, NumarTelefon}));

                int ReadedBytes;
                byte[] Buffer = new byte[1000];

                while ((ReadedBytes = ns.Read(Buffer, 0, Buffer.Length)) > 0)
                {
                    ms.Write(Buffer, 0, ReadedBytes);
                }

                Client.Close();
                ns.Dispose();

                Drawable PhotoDrawable = DrawableConverter.ByteArrayToDrawable(ms.ToArray(), context);
                Bitmap PhotoBitmap = ((BitmapDrawable)PhotoDrawable).Bitmap;

                Photo = new BitmapDrawable(context.Resources, RoundedBitmap.MakeRound(PhotoBitmap, PhotoBitmap.Width));
            }
        }
    }
}

