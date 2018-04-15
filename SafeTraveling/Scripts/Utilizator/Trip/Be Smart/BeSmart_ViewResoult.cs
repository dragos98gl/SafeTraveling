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

namespace SafeTraveling.Scripts.BeSmart
{
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class BeSmart_ViewResoult:Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Bundle Extra = Intent.Extras;
            string NumeProdus = Extra.GetString("Query");
            string Distanta = Extra.GetString("Distance");

            base.OnCreate(savedInstanceState);

            Window.RequestFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

            SetContentView(Resource.Layout.Extra_BeSmart_ViewResoult);

            TcpClient Client = new TcpClient(_Details.ServerIP,_Details.LargeFilesPort);
            NetworkStream ns = Client.GetStream();

            _TcpDataExchange.WriteStreamString(ns,CryptDecryptData.CryptData(new string[] {_Details.BeSmartSimpleQuery,NumeProdus,Distanta,new SaveUsingSharedPreferences(this).LoadString(SaveUsingSharedPreferences.Tags.Login.Username)}));

            string[] ListaProduse = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns));
            string[] ListaPreturi = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns));
            string[] ListaDistante = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns));
            string[] ListaLocatii = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns));

            for (int i = 0; i < ListaProduse.Length; i++)
                if (ListaProduse[i].Equals(string.Empty))
                {
                    ListaProduse = ListaProduse.Take(i).ToArray();
                    ListaPreturi = ListaPreturi.Take(i).ToArray();
                    ListaDistante = ListaDistante.Take(i).ToArray();
                    ListaLocatii = ListaLocatii.Take(i).ToArray();

                    break;
                }

            ListView ListaRezultate = FindViewById<ListView>(Resource.Id.listView1);
            ListaRezultate.Adapter = new BeSmart_ViewResoult_Adapter(this,ListaProduse,ListaPreturi,ListaDistante,ListaLocatii);
        }
    }
}