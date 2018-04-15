
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

namespace SafeTraveling
{
	[Activity (Label = "Utilizator_Trip_TripInfo",ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]			
	public class Utilizator_Trip_TripInfo : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

			Bundle Extras = Intent.Extras;
			byte[] BackgroundByteArray = Extras.GetByteArray ("BackgroundByteArray");

			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Utilizator_Trip_TripInfo);

			ListView ListaInformatii = FindViewById<ListView> (Resource.Id.listView1);
			LinearLayout Background = FindViewById<LinearLayout> (Resource.Id.linearLayout1);

			Background.Background = DrawableConverter.ByteArrayToDrawable (BackgroundByteArray,this);

			string[] Informatii = new string[] {
				Utilizator_Trip.TripInfo[1],
				Utilizator_Trip.TripInfo[3],
				Utilizator_Trip.TripInfo[4],
				Utilizator_Trip.TripInfo[5],
				Utilizator_Trip.TripInfo[6],
				Utilizator_Trip.TripInfo[7]
			};

			ListaInformatii.Adapter = new Utilizator_Trip_TripInfo_Adapter (this,Informatii);
		}
	}

	public class Utilizator_Trip_TripInfo_Adapter:ArrayAdapter<string>
	{
		Activity context;
		string[] Informatii;

		string[] DescriereInformatii = new string[] {
			"Destinatia principala:",
			"Data plecarii:",
			"Data intoarcerii:",
			"Locatia plecarii:",
			"Ora plecarii:",
			"Observatii:"
		};

		public Utilizator_Trip_TripInfo_Adapter(Activity context,string[] Informatii):base(context,Resource.Layout.Utilizator_Trip_TripInfo_Adapter,Informatii)
		{
			this.context = context;
			this.Informatii = Informatii;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			LayoutInflater inflater = context.LayoutInflater;
			View v = inflater.Inflate (Resource.Layout.Utilizator_Trip_TripInfo_Adapter,null,true);

			TextView DescriereInformatiiTView = v.FindViewById<TextView> (Resource.Id.textView1);
			TextView InformatiiTView = v.FindViewById<TextView> (Resource.Id.textView2);

			SetTypeface.Normal.SetTypeFace (context,DescriereInformatiiTView);
			SetTypeface.Normal.SetTypeFace (context,InformatiiTView);

			DescriereInformatiiTView.Text = DescriereInformatii[position];
			InformatiiTView.Text = Informatii[position];

			return v;
		}
	}
}

