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
using Android.Gms.Maps.Model;
using Android.Gms.Maps;
using Android.Support.V4.App;
using Android.Gms.Common;
using Android.Graphics;
using SafeTraveling.Scripts.BeSmart;
using Android.Locations;

namespace SafeTraveling.Fragments
{
    public class BeSmart_Main_CautaMagazinFragment : Android.App.Fragment
    {
        Button ChangeDistance;
        TextView DistantaCautare;
        SearchView SearchBar;
        RelativeLayout AdvanceSearch;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Extra_BeSmart_MainView_CautaMagazin, null, false);

            ChangeDistance = v.FindViewById<Button>(Resource.Id.button1);
            DistantaCautare = v.FindViewById<TextView>(Resource.Id.textView1);
            SearchBar = v.FindViewById<SearchView>(Resource.Id.searchView1);

            SetTypeface.Normal.SetTypeFace(Activity,ChangeDistance);
            SetTypeface.Normal.SetTypeFace(Activity,DistantaCautare);

            LocationManager lm = (LocationManager)Activity.GetSystemService(Activity.LocationService);
            Location l = lm.GetLastKnownLocation(LocationManager.NetworkProvider);

            LatLng location = new LatLng(double.Parse(l.Latitude.ToString()), double.Parse(l.Longitude.ToString()));
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(18);

            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            MapFragment mapFrag = MapFragment.NewInstance();
            mapFrag = (MapFragment)((FragmentActivity)Activity).FragmentManager.FindFragmentById(Resource.Id.map2);
            if (mapFrag != null)
            {
                GoogleMap map = mapFrag.Map;

                Circle RazaCautare;
                CircleOptions CircleOpts = new CircleOptions();
                CircleOpts.InvokeCenter(location);
                CircleOpts.InvokeRadius(50);
                CircleOpts.InvokeStrokeColor(Color.Black);
                CircleOpts.InvokeFillColor(Color.Argb(140, 0, 0, 0));

                RazaCautare = map.AddCircle(CircleOpts);

                map.MapType = GoogleMap.MapTypeNormal;
                map.MoveCamera(cameraUpdate);

                ChangeDistance.Click += ((object sender, EventArgs e) =>
                {
                    BeSmart_SetareDistanta_DialogFragment diag = new BeSmart_SetareDistanta_DialogFragment();
                    diag.Show(FragmentManager, "SetareDistanta");

                    diag.DistanceChanged += ((object sender1, DistanceChangedEventArgs e1) =>
                    {
                        ChangeDistance.Text = e1.Value;

                        RazaCautare.Remove();

                        CircleOpts.InvokeRadius(int.Parse(e1.Value));
                        RazaCautare=map.AddCircle(CircleOpts);
                    });
                });
            }

            return v;
        }
    }
}