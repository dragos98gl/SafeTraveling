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
using Android.Support.V7.App;
using Android.Views.Animations;
using Android.Graphics.Drawables;
using SafeTraveling.Fragments;
using Android.Locations;

namespace SafeTraveling
{
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/MyTheme")]
    class BeSmart_Main : ActionBarActivity, Android.Support.V7.App.ActionBar.ITabListener
    {
        FrameLayout FragmentContainer;

        BeSmart_Main_CautaMagazinFragment CautaMagazinFragment;
        BeSmart_Main_CautaProdusFragment CautaProdusFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Extra_BeSmart_MainView);

            SupportActionBar.SetDisplayShowCustomEnabled(true);
            SupportActionBar.SetCustomView(Resource.Layout.Extra_BeSmart_ActionBar);
            SupportActionBar.NavigationMode = Convert.ToInt32(ActionBarNavigationMode.Tabs);
            SupportActionBar.SetHomeButtonEnabled(false);
            SupportActionBar.SetDisplayShowHomeEnabled(false);
            SupportActionBar.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Rgb(213, 219, 254)));
            SupportActionBar.SetStackedBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Rgb(213, 219, 254)));

            var tab1 = SupportActionBar.NewTab();
            tab1.SetText("Cauta produs");
            tab1.SetTabListener(this);
            
            var tab2 = SupportActionBar.NewTab();
            tab2.SetText("Cauta magazin");
            tab2.SetTabListener(this);

            SupportActionBar.AddTab(tab1);
            SupportActionBar.AddTab(tab2);

            {
                ImageView ActionBarIcon = SupportActionBar.CustomView.FindViewById<ImageView>(Resource.Id.imageView1);
                TextView ActionBarPromo = SupportActionBar.CustomView.FindViewById<TextView>(Resource.Id.textView1);

                SetBeatingAnimation(ActionBarIcon);
                SetTypeface.Normal.SetTypeFace(this,ActionBarPromo);
            }

            FragmentContainer = FindViewById<FrameLayout>(Resource.Id.frameLayout1);
            FragmentTransaction transact = FragmentManager.BeginTransaction();

            CautaMagazinFragment = new BeSmart_Main_CautaMagazinFragment();
            CautaProdusFragment = new BeSmart_Main_CautaProdusFragment();

            transact.Add(FragmentContainer.Id, CautaMagazinFragment, "CautaMagazin");
            transact.Hide(CautaMagazinFragment);
            transact.Add(FragmentContainer.Id, CautaProdusFragment, "CautaProdus");

            transact.Commit();
        }

        public void OnTabReselected(Android.Support.V7.App.ActionBar.Tab tab, Android.Support.V4.App.FragmentTransaction ft)
        {
        }

        public void OnTabSelected(Android.Support.V7.App.ActionBar.Tab tab, Android.Support.V4.App.FragmentTransaction ft)
        {
            switch (tab.Text)
            {
                case "Cauta produs":
                    {
                        if ((CautaMagazinFragment != null) && (CautaProdusFragment != null))
                        {
                            FragmentTransaction transact = FragmentManager.BeginTransaction();
                            transact.SetCustomAnimations(
    Resource.Animation.ScaleToMax,
    Resource.Animation.ScaleToZero);
                            transact.Hide(CautaMagazinFragment);
                            transact.Show(CautaProdusFragment);
                            transact.AddToBackStack(null);
                            transact.Commit();
                        }
                    } break;
                case "Cauta magazin":
                    {
                        FragmentTransaction transact = FragmentManager.BeginTransaction();
                        transact.SetCustomAnimations(
    Resource.Animation.ScaleToMax,
    Resource.Animation.ScaleToZero);
                        transact.Hide(CautaProdusFragment);
                        transact.Show(CautaMagazinFragment);
                        transact.AddToBackStack(null);
                        transact.Commit();

                    } break;
            }
        }

        public void OnTabUnselected(Android.Support.V7.App.ActionBar.Tab tab, Android.Support.V4.App.FragmentTransaction ft)
        {
        }

        void SetBeatingAnimation(View obj)
        {
            Animation ScaleIn = AnimationUtils.LoadAnimation(this, Resource.Animation.ScaleIn);
            ScaleIn.FillAfter = true;

            Animation ScaleOut = AnimationUtils.LoadAnimation(this, Resource.Animation.ScaleOut);
            ScaleIn.FillAfter = true;

            ScaleIn.AnimationEnd += (object sender1, Animation.AnimationEndEventArgs e1) =>
            {
                obj.StartAnimation(ScaleOut);
            };

            ScaleOut.AnimationEnd += (object sender1, Animation.AnimationEndEventArgs e1) =>
            {
                obj.StartAnimation(ScaleIn);
            };

            obj.StartAnimation(ScaleIn);
        }
    }
}