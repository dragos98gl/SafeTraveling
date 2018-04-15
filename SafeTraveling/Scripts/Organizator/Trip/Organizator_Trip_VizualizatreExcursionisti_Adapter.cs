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
using System.Net.Sockets;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.IO;
using Android.Provider;

namespace SafeTraveling.Scripts.Organizator.Trip
{
    class Organizator_Trip_VizualizatreExcursionisti_Adapter:ArrayAdapter<Excursionist>
    {
        Activity context;
        Excursionist[] Excursionisti;

        public Organizator_Trip_VizualizatreExcursionisti_Adapter(Activity context,Excursionist[] Excursionisti):base(context,Resource.Layout.Organizator_Trip_VizualizareExcursionisti_Adapter,Excursionisti)
        {
            this.context = context;
            this.Excursionisti = Excursionisti;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = context.LayoutInflater;
            View v = inflater.Inflate(Resource.Layout.Organizator_Trip_VizualizareExcursionisti_Adapter, null, true);

            ImageView ProfilePic = v.FindViewById<ImageView>(Resource.Id.imageView1);
            TextView Nume = v.FindViewById<TextView>(Resource.Id.textView1);
            TextView Distanta = v.FindViewById<TextView>(Resource.Id.textView2);

            ProfilePic.SetImageDrawable(Excursionisti[position].Photo);
            Nume.Text = "Nume:"+Excursionisti[position].Nume + " " + Excursionisti[position].Prenume;
            Distanta.Text = "Distanta:"+Excursionisti[position].Distanta;
            
            return v;
        }
    }

    public class Excursionist
    {
        Context context;
        public string Nume;
        public string Prenume;
        public string TipCont;
        public string NumarTelefon;
        public string Distanta;
        public string Pozitie;

        public Drawable Photo;

        public Excursionist(Context context, string[] Messages,string nrTel)
        {
            this.context = context;

            Nume = Messages[2];
            Prenume = Messages[3];
            TipCont = Messages[4];
            NumarTelefon = Messages[5];
            Distanta = Messages[7];
            Pozitie = Messages[8];

            TcpClient Client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
            NetworkStream ns = Client.GetStream();

            using (MemoryStream ms = new MemoryStream())
            {
                _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.GetProfilePic, NumarTelefon }));

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