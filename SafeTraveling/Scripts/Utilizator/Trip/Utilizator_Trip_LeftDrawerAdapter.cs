using System;
using Android.Widget;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Content;
using SafeTraveling.Scripts.Utilizator.Trip;
using Android.Provider;

namespace SafeTraveling
{
	public class Utilizator_Trip_LeftDrawerAdapter:ArrayAdapter<string>
	{
		Drawable[] OptiuniIcon;

		Activity context;
		string[] Optiuni;
		Drawable Background;

		public Utilizator_Trip_LeftDrawerAdapter (Activity context,Drawable Background,string [] Optiuni):base(context,Resource.Layout.Common_LeftDrawer,Optiuni)
		{
			this.context = context;
			this.Optiuni = Optiuni;
			this.Background = Background;

			OptiuniIcon = new Drawable[] { 
				context.Resources.GetDrawable (Resource.Drawable.Galerie),
				context.Resources.GetDrawable (Resource.Drawable.ModificareCont),
                context.Resources.GetDrawable (Resource.Drawable.Shop),
				context.Resources.GetDrawable (Resource.Drawable.TripInfo),
				context.Resources.GetDrawable (Resource.Drawable.SettingsIcon),
				context.Resources.GetDrawable (Resource.Drawable.Logout),
				context.Resources.GetDrawable (Resource.Drawable.IesireExcursie)
			};
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			LayoutInflater inflater = context.LayoutInflater;
			View v = inflater.Inflate (Resource.Layout.Common_LeftDrawer, null, true);

			RelativeLayout LayoutOptiuni = v.FindViewById<RelativeLayout> (Resource.Id.relativeLayout4);
			ImageView OptionIconIView = v.FindViewById<ImageView> (Resource.Id.imageView2);
			TextView OptionTView = v.FindViewById<TextView> (Resource.Id.textView2);

			SetTypeface.Normal.SetTypeFace (context, OptionTView);

			OptionTView.Text = Optiuni [position];
			OptionIconIView.SetImageDrawable(OptiuniIcon[position]);

			LayoutOptiuni.Click += (object sender, EventArgs e) => {
				switch (position)
				{
                    case 0: {
                        PopupMenu popup = new PopupMenu(context, v);
                        popup.MenuItemClick += (object s, PopupMenu.MenuItemClickEventArgs e1) =>
                        {
                            switch (e1.Item.ToString())
                            {
                                case "Adauga poza":
                                    {
                                        string Path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures) + "/temp";
                                        Java.IO.File file = new Java.IO.File(Path, string.Format("temp.jpg", Guid.NewGuid()));

                                        Intent i = new Intent(MediaStore.ActionImageCapture);
                                        i.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(file));
                                        context.StartActivityForResult(i, 0);
                                    } break;
                                case "Vizualizare Galerie":
                                    {
                                        context.StartActivity(typeof(Utilizator_Trip_Galerie));
                                    } break;
                            }
                        };
                        MenuInflater menuInflater = popup.MenuInflater;
                        menuInflater.Inflate(Resource.Menu.OptiuniGalerie, popup.Menu);
                        popup.Show();
                    }break;
				case 1:
					{
						Intent StartModifyAcc = new Intent (context,typeof(Utilizator_Trip_ModifyAccount));
						StartModifyAcc.PutExtra("BackgroundByteArray",DrawableConverter.DrawableToByteArray(Background));

						context.StartActivity(StartModifyAcc);
					} break;
                case 2: 
                {
                    context.StartActivity(typeof(BeSmart_Loading));
                }break;
				case 3:
					{
						Intent StartTripInfo = new Intent (context,typeof(Utilizator_Trip_TripInfo));
						StartTripInfo.PutExtra ("BackgroundByteArray" , DrawableConverter.DrawableToByteArray(Background));

						context.StartActivity(StartTripInfo);	
					} break;
				}
			};

			return v;
		}
	}
}

