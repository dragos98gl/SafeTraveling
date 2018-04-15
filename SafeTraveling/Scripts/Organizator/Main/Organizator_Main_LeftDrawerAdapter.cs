using System;
using Android.App;
using Android.Graphics.Drawables;
using Android.Widget;
using Android.Views;

namespace SafeTraveling
{
	public class Organizator_Main_LeftDrawerAdapter:ArrayAdapter<string>
	{
		Activity context;
		string[] Optiuni;
		Drawable[] OptiuniIcon;

		public Organizator_Main_LeftDrawerAdapter (Activity context,string[] Optiuni,Drawable[] OptiuniIcon):base(context,Resource.Layout.Common_LeftDrawer,Optiuni)
		{
			this.context = context;
			this.Optiuni = Optiuni;
			this.OptiuniIcon = OptiuniIcon;
		}

		public override View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			LayoutInflater inflater = context.LayoutInflater;
			View v = inflater.Inflate (Resource.Layout.Common_LeftDrawer, null, true);

			RelativeLayout LayoutOptiuni = v.FindViewById<RelativeLayout> (Resource.Id.relativeLayout4);
			ImageView OptionIconIView = v.FindViewById<ImageView> (Resource.Id.imageView2);
			TextView OptionTView = v.FindViewById<TextView> (Resource.Id.textView2);

			OptionTView.Text = Optiuni [position];
			SetTypeface.Normal.SetTypeFace (context, OptionTView);

			LayoutOptiuni.Click += (object sender, EventArgs e) => {
				Toast.MakeText (context, position.ToString (), ToastLength.Long).Show ();
			};

			return v;
		}
	}
}

