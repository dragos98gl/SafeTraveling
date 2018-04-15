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
using Android.Support.V4.View;
using Android.Support.V4.App;

namespace SafeTraveling
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]			
	public class Organizator_CreareExcursie : FragmentActivity
	{
		string[] Titluri = new string[] { 
			"1",
			"2",
			"3",
			"4"
		};

		ViewPager Pager;
		TextView Titlu;
		RelativeLayout Background;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

			Bundle Extras = Intent.Extras;
			byte[] BackgroundByteArray = Extras.GetByteArray ("BackgroundUtilizatorMainByteArray");

 			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Organizator_CreareExcursie);

			Pager = FindViewById<ViewPager> (Resource.Id.viewPager1);
			Titlu = FindViewById<TextView> (Resource.Id.textView1);
			Background = FindViewById<RelativeLayout> (Resource.Id.relativeLayout1);

			SetTypeface.Bold.SetTypeFace (this,Titlu);
			Background.Background = DrawableConverter.ByteArrayToDrawable (BackgroundByteArray,this);

			Pager.Adapter = new Organizator_CreareExcursie_ViewPagerAdapter (SupportFragmentManager,this,Background.Background);
			Pager.PageSelected += (object sender, ViewPager.PageSelectedEventArgs e) => {
				Titlu.Text = Titluri[e.Position];
			};
		}
	}
}

