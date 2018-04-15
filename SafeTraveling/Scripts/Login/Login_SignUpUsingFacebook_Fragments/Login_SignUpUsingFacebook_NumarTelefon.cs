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
    public class Login_SignUpUsingFacebook_NumarTelefon : Fragment
    {
        public EditText NumarTelefon;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Login_SignUpUsingFacebook_NumarTelefon, null, false);

            TextView NumarTelefon_TView = v.FindViewById<TextView>(Resource.Id.textView1);
            NumarTelefon = v.FindViewById<EditText>(Resource.Id.editText1);

            SetTypeface.Normal.SetTypeFace(Activity, NumarTelefon_TView);
            SetTypeface.Normal.SetTypeFace(Activity, NumarTelefon);

            return v;
        }
    }
}