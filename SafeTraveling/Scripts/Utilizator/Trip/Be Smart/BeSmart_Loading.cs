using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using Android.Views.Animations;

namespace SafeTraveling
{
    [Activity(Label = "BeSmart_REBUILDED",NoHistory=true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, MainLauncher = false, Icon = "@drawable/icon")]
    public class BeSmart_Loading : Activity
    {
        public static string MyPos;
        ImageView Logo;
        TextView Promo;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Extra_BeSmart_LoadingScreen);

            Logo = FindViewById<ImageView>(Resource.Id.imageView1);
            Promo = FindViewById<TextView>(Resource.Id.textView1);

            SetTypeface.Normal.SetTypeFace(this,Promo);

            StartAnimation();

            Thread Temp = new Thread(() => {
                Thread.Sleep(3000);
                StartActivity(typeof(BeSmart_Main));
            });
            Temp.IsBackground = true;
            Temp.Start();
        }

        void StartAnimation()
        {
            Animation ScaleIn = AnimationUtils.LoadAnimation(this, Resource.Animation.ScaleIn);
            ScaleIn.FillAfter = true;

            Animation ScaleOut = AnimationUtils.LoadAnimation(this, Resource.Animation.ScaleOut);
            ScaleIn.FillAfter = true;

            ScaleIn.AnimationEnd += (object sender1, Animation.AnimationEndEventArgs e1) =>
            {
                Logo.StartAnimation(ScaleOut);
            };

            ScaleOut.AnimationEnd += (object sender1, Animation.AnimationEndEventArgs e1) =>
            {
                Logo.StartAnimation(ScaleIn);
            };

            RotateAnimation animRotate = new RotateAnimation(0.0f, -360.0f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            animRotate.Duration = 3000;
            animRotate.RepeatCount = Animation.Infinite;

            Logo.StartAnimation(ScaleIn);
        }
    }
}