
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
using Android.Graphics.Drawables;
using Android.Graphics;

namespace SafeTraveling
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]			
	public class Organizator_Main : Activity
	{
		Button CreareExcursieBtn;
		TextView CreareExcursieTView;
		ListView LeftDrawer;
		ImageView UserProfilePic;
		TextView UserNume;
		RelativeLayout Background;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

			Bundle Extras = Intent.Extras;
 			byte[] BackgroundByteArray = Extras.GetByteArray ("BackgroundUtilizatorMainByteArray");

			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Organizator_Main);

			CreareExcursieBtn = FindViewById<Button> (Resource.Id.button1);
			CreareExcursieTView = FindViewById <TextView> (Resource.Id.textView2);
			LeftDrawer = FindViewById <ListView> (Resource.Id.left_drawer_listview);
			UserProfilePic = FindViewById<ImageView> (Resource.Id.imageView1);
			UserNume = FindViewById<TextView> (Resource.Id.textView1);
			Background = FindViewById<RelativeLayout> (Resource.Id.relativeLayout1);

			Background.Background = DrawableConverter.ByteArrayToDrawable (BackgroundByteArray,this);

			Drawable RoundedUserProfile = DrawableConverter.GetDrawableFromAssets ("LoadingBackgrounds/img1.jpg", this);
			RoundedUserProfile = new BitmapDrawable (Resources, RoundedBitmap.MakeRound (((BitmapDrawable)RoundedUserProfile).Bitmap, ((BitmapDrawable)RoundedUserProfile).Bitmap.Width));
			UserProfilePic.SetImageDrawable(RoundedUserProfile);
			UserNume.Text = "Dragos Mihaitza";
			
            SetTypeface.Normal.SetTypeFace (this,UserNume);
			Drawable d = Resources.GetDrawable(Resource.Drawable.SettingsIcon);
			LeftDrawer.Adapter = new Organizator_Main_LeftDrawerAdapter (this,new string[] {"test 1","test 2","test 3",},new Drawable[] {d,d,d});

            SetTypeface.Normal.SetTypeFace(this, CreareExcursieTView);
            SetTypeface.Normal.SetTypeFace(this, CreareExcursieBtn);

			CreareExcursieBtn.Click += (object sender, EventArgs e) => {
				Intent StartCreareExcursie = new Intent (this, typeof(Organizator_CreareExcursie));

				StartCreareExcursie.PutExtra ("BackgroundUtilizatorMainByteArray", DrawableConverter.DrawableToByteArray (Background.Background));
				StartActivity (StartCreareExcursie);
			};
		}
	}
}

