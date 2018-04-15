using System;
using Android.Locations;
using System.IO;
using System.Net.Sockets;

namespace SafeTraveling
{
	public class LocationListener:Java.Lang.Object,ILocationListener
	{
		TcpClient Client;
		NetworkStream ns;

		public LocationListener(TcpClient Client,NetworkStream ns)
		{
			this.Client = Client;
			this.ns = ns;
		}

		public void OnLocationChanged (Location location)
		{
			double Lat = location.Latitude;
			double Long = location.Longitude;

			_TcpDataExchange.WriteStreamString (ns,CryptDecryptData.CryptData(new string[]{Lat.ToString(),Long.ToString()}));
            string[] Response = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns));

            if (Response[0] == "1")
            {
                int i = 1;
                while (Response[i] != "")
                {
                    string NrTel = Response[i];
                    string Distanta = Response[i+1];

                    LocationableClient LClient = new LocationableClient(NrTel,Distanta);

                    i += 2;
                }
            }

			string current;

			string path = Android.OS.Environment.GetExternalStoragePublicDirectory (Android.OS.Environment.DirectoryPictures) + "/gps.txt";

			FileStream ReadStream = new FileStream (path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			StreamReader r = new StreamReader (ReadStream);
			current = r.ReadToEnd ();
			r.Close ();
			ReadStream.Close ();

			FileStream WriteStream = new FileStream (path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			StreamWriter w = new StreamWriter (WriteStream);
			w.WriteLine (current + System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString() + ":" + System.DateTime.Now.Second.ToString()+"=>"+Lat.ToString() + "    " + Long.ToString()+ "    " + location.Accuracy.ToString());
			w.Close ();
			WriteStream.Close ();
		}
		public void OnProviderDisabled (string provider)
		{
		}
		public void OnProviderEnabled (string provider)
		{
		}
		public void OnStatusChanged (string provider, Availability status, Android.OS.Bundle extras)
		{
		}
	}

    class LocationableClient
    {
        public string NrTel;
        public string Distanta;

        public LocationableClient(string NrTel,string Distanta)
        {
            this.NrTel = NrTel;
            this.Distanta = Distanta;
        }
    }
}

