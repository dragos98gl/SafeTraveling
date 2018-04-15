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

namespace SafeTraveling.Scripts.Utilizator.Trip.Game_Invite
{
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class Utilizator_Trip_InviteToGame : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            Bundle Extras = Intent.Extras;
            string GamePlayerNrTel = Extras.GetString(_Details.Game_Player_NrTel);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Utilizator_Trip_GameInvite);

            GridView Container = FindViewById<GridView>(Resource.Id.GridViewLay);
            Container.Adapter = new Utilizator_Trip_InviteToGame_GridViewAdapter(this,new string[] {"X si 0","Bounce Game"},GamePlayerNrTel);
        }
    }
}