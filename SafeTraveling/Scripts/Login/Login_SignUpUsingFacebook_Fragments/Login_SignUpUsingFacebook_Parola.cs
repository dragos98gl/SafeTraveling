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
using System.Net.Sockets;

namespace SafeTraveling.Login_SignUpUsingFacebook_Fragments
{
    public class Login_SignUpUsingFacebook_Parola : Fragment
    {
        public EditText Parola;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Login_SignUpUsingFacebook_Parola, null, false);

            TextView Parola_TView = v.FindViewById<TextView>(Resource.Id.textView1);
            Parola = v.FindViewById<EditText>(Resource.Id.editText1);

            SetTypeface.Normal.SetTypeFace(Activity, Parola_TView);
            SetTypeface.Normal.SetTypeFace(Activity, Parola);

            return v;
        }
    }
}