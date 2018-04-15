
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
	[Activity (Label = "Organizator_Nou",ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]			
	public class Organizator_Nou : FragmentActivity
	{
		ViewPager Pager;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Organizator_Nou);

			Pager = FindViewById<ViewPager> (Resource.Id.viewPager1);

			Pager.Adapter = new Organizator_Nou_ViewPagerAdapter(SupportFragmentManager,this);
		}
	}
}

