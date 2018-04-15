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

namespace SafeTraveling.Scripts.BeSmart
{
    class BeSmart_AdvancedSearch_DialogFragment:DialogFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Extra_BeSmart_AdvancedSearch_DialogFragment,null,false);

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            RelativeLayout Container = v.FindViewById<RelativeLayout>(Resource.Id.relativeLayout2);
            TextView CategoriePrimara_TView = v.FindViewById<TextView>(Resource.Id.textView1);
            TextView CategorieSecundara_TView = v.FindViewById<TextView>(Resource.Id.textView2);
            TextView Firma_TView = v.FindViewById<TextView>(Resource.Id.textView3);
            TextView NumeProdus_TView = v.FindViewById<TextView>(Resource.Id.textView4);
            TextView Cauta_TView = v.FindViewById<TextView>(Resource.Id.textView5);

            SetTypeface.Normal.SetTypeFace(Activity, CategoriePrimara_TView);
            SetTypeface.Normal.SetTypeFace(Activity, CategorieSecundara_TView);
            SetTypeface.Normal.SetTypeFace(Activity, Firma_TView);
            SetTypeface.Normal.SetTypeFace(Activity, NumeProdus_TView);
            SetTypeface.Normal.SetTypeFace(Activity, Cauta_TView);

            WindowManagerLayoutParams lp = new WindowManagerLayoutParams();
            lp.CopyFrom(Dialog.Window.Attributes);
            lp.Height = Container.LayoutParameters.Height;
            lp.Width = Container.LayoutParameters.Width;

            Dialog.Show();

            Dialog.Window.Attributes = lp;

            return v;
        }
    }
}