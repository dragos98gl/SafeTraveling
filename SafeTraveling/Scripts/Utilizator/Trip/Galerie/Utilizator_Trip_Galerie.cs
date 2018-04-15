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
using Android.Graphics.Drawables;
using Java.Lang;
using System.IO;

namespace SafeTraveling.Scripts.Utilizator.Trip
{
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    class Utilizator_Trip_Galerie : Activity
    {
        GridView PhotosContainer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Utilizator_Trip_Galerie);

            PhotosContainer = FindViewById<GridView>(Resource.Id.GridViewLay);

            TcpClient Client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
            NetworkStream ns = Client.GetStream();

            _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.GetGalleryCount,Utilizator_Trip.TripId}));

            int PhotosCount = int.Parse(CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns))[0]);

            List<Drawable> Poze = new List<Drawable>();

            for (int i = 0; i < PhotosCount; i++)
                Poze.Add(null);

            PhotosContainer.Adapter = new Utilizator_Trip_Galerie_GridViewAdapter(this, Poze.ToArray());

            new Thread(() => {
                for (int i = 0; i < PhotosCount; i++)
                {
                    Client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
                    ns = Client.GetStream();

                    _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.GetGalleryPhotoByIndex, Utilizator_Trip.TripId,i.ToString() }));

                    int ReadedBytes;
                    byte[] Buffer = new byte[1000];
                    MemoryStream ms = new MemoryStream();

                    while ((ReadedBytes = ns.Read(Buffer, 0, Buffer.Length)) > 0)
                    {
                        ms.Write(Buffer, 0, ReadedBytes);
                    }

                    Client.Close();
                    ns.Dispose();
                    
                    Drawable Photo = DrawableConverter.ByteArrayToDrawable(ms.ToArray(),this);

                    Poze[i] = Photo;

                    RunOnUiThread(() => {
                        PhotosContainer.Adapter = new Utilizator_Trip_Galerie_GridViewAdapter(this, Poze.ToArray());
                    });
                }
            }).Start();
        }
    }
}