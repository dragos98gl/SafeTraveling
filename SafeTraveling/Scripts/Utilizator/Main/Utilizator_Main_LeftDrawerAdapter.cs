using System;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.App;
using Android.Views;
using Android.Content;

namespace SafeTraveling
{
	public class Utilizator_Main_LeftDrawerAdapter:ArrayAdapter<string>
	{
		Activity context;
		string[] Optiuni;
		Drawable[] OptiuniIcon;
		Drawable Background;

		public Utilizator_Main_LeftDrawerAdapter (Activity context,string[] Optiuni,Drawable Background):base(context,Resource.Layout.Common_LeftDrawer,Optiuni)
		{
			this.context = context;
			this.Optiuni = Optiuni;
			this.Background = Background;

			OptiuniIcon = new Drawable[] { 
				context.Resources.GetDrawable (Resource.Drawable.Logout)
			};
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			LayoutInflater inflater = context.LayoutInflater;
			View v = inflater.Inflate (Resource.Layout.Common_LeftDrawer, null, true);

			ImageView OptionIconIView = v.FindViewById<ImageView> (Resource.Id.imageView2);
			TextView OptionTView = v.FindViewById<TextView> (Resource.Id.textView2);

			OptionTView.Text = Optiuni [position];
			SetTypeface.Normal.SetTypeFace (context, OptionTView);

			OptionIconIView.SetImageDrawable (OptiuniIcon[position]);

			OptionTView.Click += (object sender, EventArgs e) => {
				switch(((TextView)sender).Text)
				{
				case "Logout":{
						SaveUsingSharedPreferences Save = new SaveUsingSharedPreferences (context);

						Save.Clear(SaveUsingSharedPreferences.Tags.Login.Username);

						Intent StartSignUp = new Intent (context, typeof(Login));

						StartSignUp.PutExtra ("BackgroundLoginByteArray", DrawableConverter.DrawableToByteArray (Background));
						context.StartActivity (StartSignUp);
					}break;
				}
			};

			return v;
		}
	}
}

