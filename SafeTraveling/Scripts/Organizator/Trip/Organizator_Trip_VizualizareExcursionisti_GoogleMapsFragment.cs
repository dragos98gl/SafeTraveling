using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;

namespace SafeTraveling.Scripts.Organizator.Trip
{
    public class Organizator_Trip_VizualizareExcursionisti_GoogleMapsFragment : Fragment
    {
        FrameLayout FragmentContainer;

        public Organizator_Trip_VizualizareExcursionisti_GoogleMapsFragment(FrameLayout FragmentContainer)
        {
            this.FragmentContainer = FragmentContainer;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        bool UpDown = true;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Organizator_Trip_VizualizareExcursionisti_GoogleMapsFragment,null,false);

            ImageView PullUpBtn = v.FindViewById<ImageView> (Resource.Id.imageView1);

            PullUpBtn.Click += delegate
            {
                if (UpDown)
                {
                    OvershootInterpolator interpolator = new OvershootInterpolator(2.5f);
                    FragmentContainer.Animate().SetInterpolator(interpolator)
                        .TranslationYBy(-500)
                        .SetDuration(1000);

                    UpDown = false;
                }
                else {
                    OvershootInterpolator interpolator = new OvershootInterpolator(1);
                    FragmentContainer.Animate().SetInterpolator(interpolator)
                        .TranslationYBy(500)
                        .SetDuration(500);

                    UpDown = true;
                }
            };

            return v;
        }
    }
}