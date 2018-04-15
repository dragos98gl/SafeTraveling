using System;
using Android.App;
using Java.Lang.Reflect;
using Android.Views;
using Android.Support.V4.View;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;
using Android.Graphics.Drawables;
using System.Collections.Generic;

namespace SafeTraveling
{
	[Activity(NoHistory=true,ScreenOrientation=Android.Content.PM.ScreenOrientation.Landscape,Theme="@style/Theme.AppCompat.Light")]
	public class SignUp:FragmentActivity
	{
		ViewPager Pager;
		TextView Titlu;
		public RelativeLayout Background;

		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			RequestWindowFeature (Android.Views.WindowFeatures.NoTitle);
			Window.SetFlags (Android.Views.WindowManagerFlags.Fullscreen,Android.Views.WindowManagerFlags.Fullscreen);
		
			Bundle Extras = Intent.Extras;
			int BackgroundByteArray = Extras.GetInt ("BackgroundSignUpByteArray");

			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.SignUp);

			Pager = FindViewById<ViewPager> (Resource.Id.viewPager1);
			Titlu = FindViewById <TextView> (Resource.Id.textView1);
			Background = FindViewById<RelativeLayout> (Resource.Id.relativeLayout1);

			string[] Titluri = new string[] { 
				"Informatii Personale",
				"Informatii Virtuale",
				"Finalizare",
			};

			int[] Layouturi = new int[] { 
				Resource.Layout.SignUp_Page1,
				Resource.Layout.SIgnUp_Page2,
				Resource.Layout.SignUp_Page3,
			};

			SetTypeface.Bold.SetTypeFace (this,Titlu);

            List<Drawable> DrawableList = new List<Drawable>();
            for (int i = 1; i < 5; i++)
                DrawableList.Add(DrawableConverter.GetDrawableFromAssets("LB/img" + i.ToString() + ".jpg", this));
            for (int i = 1; i < 8; i++)
                DrawableList.Add(DrawableConverter.GetDrawableFromAssets("LoadingBackgrounds/img" + i.ToString() + ".jpg", this));
            Background.Background = DrawableList[BackgroundByteArray];

			Pager.Adapter = new ViewPagerAdapter(SupportFragmentManager,Layouturi,this,Pager);
			Pager.OffscreenPageLimit = 3;
			Pager.PageSelected+= (object sender, ViewPager.PageSelectedEventArgs e) => {
				Titlu.Text = Titluri[e.Position];
			};
		}
	}
}

