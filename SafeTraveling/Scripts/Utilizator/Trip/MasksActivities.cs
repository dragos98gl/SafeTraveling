using System;
using Android.App;
using Android.OS;

namespace SafeTraveling
{
	[Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
	public class Utilizator_Trip_MaskA:Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Utilizator_Trip_ChatView);
		}
	}

	[Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
	public class Utilizator_Trip_MaskB:Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Utilizator_Trip_MaskB);
		}
	}

	[Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
	public class Utilizator_Trip_MaskC:Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Utilizator_Trip_MaskC);
		}
	}
}

