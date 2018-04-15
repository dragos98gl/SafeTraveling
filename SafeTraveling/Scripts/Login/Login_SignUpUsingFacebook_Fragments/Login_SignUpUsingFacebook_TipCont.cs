using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace SafeTraveling.Login_SignUpUsingFacebook_Fragments
{
    public class Login_SignUpUsingFacebook_TipCont : Fragment
    {
        public RadioButton Utilizator;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Login_SignUpUsingFacebook_TipCont, null, false);

            TextView TipCont_TView = v.FindViewById<TextView>(Resource.Id.textView1);
            Utilizator = v.FindViewById<RadioButton>(Resource.Id.radioButton1);
            RadioButton Organizator = v.FindViewById<RadioButton>(Resource.Id.radioButton2);

            SetTypeface.Normal.SetTypeFace(Activity, TipCont_TView);
            SetTypeface.Normal.SetTypeFace(Activity, Utilizator);
            SetTypeface.Normal.SetTypeFace(Activity, Organizator);

            return v;
        }
    }
}