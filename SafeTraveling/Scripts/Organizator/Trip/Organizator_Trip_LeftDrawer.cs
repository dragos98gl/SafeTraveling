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
using System.Net.Sockets;

namespace SafeTraveling.Scripts.Organizator.Trip
{
    public class Organizator_Trip_LeftDrawer : ArrayAdapter<string>
    {
        Drawable[] OptiuniIcon;

        Activity context;
        string[] Optiuni;
        Drawable Background;

        public Organizator_Trip_LeftDrawer(Activity context, Drawable Background, string[] Optiuni)
            : base(context, Resource.Layout.Common_LeftDrawer, Optiuni)
        {
            this.context = context;
            this.Optiuni = Optiuni;
            this.Background = Background;

            OptiuniIcon = new Drawable[] { 
				context.Resources.GetDrawable (Resource.Drawable.TripInfo)
			};
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            LayoutInflater inflater = context.LayoutInflater;
            View v = inflater.Inflate(Resource.Layout.Common_LeftDrawer, null, true);

            RelativeLayout LayoutOptiuni = v.FindViewById<RelativeLayout>(Resource.Id.relativeLayout4);
            ImageView OptionIconIView = v.FindViewById<ImageView>(Resource.Id.imageView2);
            TextView OptionTView = v.FindViewById<TextView>(Resource.Id.textView2);

            SetTypeface.Normal.SetTypeFace(context, OptionTView);

            OptionTView.Text = Optiuni[position];
            OptionIconIView.SetImageDrawable(OptiuniIcon[position]);

            LayoutOptiuni.Click += (object sender, EventArgs e) =>
            {
                switch (position)
                {
                    case 0:
                        {
                            Dialog diag = new Dialog(context);
                            diag.Window.RequestFeature(WindowFeatures.NoTitle);

                            View CostumView = inflater.Inflate(Resource.Layout.Organizator_Trip_TripId_AlertDialogAdapter, null);
                            TextView TripIdInfo = CostumView.FindViewById<TextView>(Resource.Id.textView1);

                            string TripId = new SaveUsingSharedPreferences(context).LoadString(SaveUsingSharedPreferences.Tags.Trip.TipId);


                            TripIdInfo.Text = "Id-ul excursiei este: "+TripId;

                            diag.SetContentView(CostumView);

                            diag.SetCanceledOnTouchOutside(true);
                            diag.Show();
                            diag.Window.SetBackgroundDrawable(context.Resources.GetDrawable(Resource.Drawable.background_MarginiOvaleAlb));

                        } break;
                }
            };

            return v;
        }
    }
}