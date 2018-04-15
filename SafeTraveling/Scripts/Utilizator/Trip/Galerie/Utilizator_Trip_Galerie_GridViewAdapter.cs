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

namespace SafeTraveling.Scripts.Utilizator.Trip
{
    class Utilizator_Trip_Galerie_GridViewAdapter:ArrayAdapter<Drawable>
    {
        Activity context;
		Drawable[] Icons;

        public Utilizator_Trip_Galerie_GridViewAdapter(Activity context, Drawable[] Icons)
            : base(context, Resource.Layout.Utilizator_Trip_Galerie_Adapter, Icons)
		{
			this.context = context;
			this.Icons = Icons;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			LayoutInflater inflater = context.LayoutInflater;
            View v = inflater.Inflate(Resource.Layout.Utilizator_Trip_Galerie_Adapter, null, true);

			ImageView Icon_iView = v.FindViewById <ImageView> (Resource.Id.imageView1);
            ProgressBar LoadingState = v.FindViewById <ProgressBar>(Resource.Id.progressBar1);

            if (Icons[position] != null){
                LoadingState.Visibility= ViewStates.Gone;
                Icon_iView.Visibility = ViewStates.Visible;
                Icon_iView.Background = Icons[position];
            }
		
        return v;
		}
    }
}