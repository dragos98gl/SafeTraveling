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

namespace SafeTraveling
{
    public class Organizator_Trip_NewQuestionPool_IntroducereIntrebare : Fragment
    {
        public EditText IntroducetiIntrebarea_EText;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Organizator_Trip_NewQuestionPool_IntroducereIntrebare,null,false);

            TextView IntroducetiIntrebarea_TView = v.FindViewById<TextView>(Resource.Id.textView1);
            IntroducetiIntrebarea_EText = v.FindViewById<EditText>(Resource.Id.editText1);

            SetTypeface.Normal.SetTypeFace(Activity, IntroducetiIntrebarea_TView);
            SetTypeface.Normal.SetTypeFace(Activity, IntroducetiIntrebarea_EText);

            return v;
        }
    }
}