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
using Android.Gms.Common;
using Android.Graphics;
using SafeTraveling.Scripts.BeSmart;
using Android.Locations;

namespace SafeTraveling
{
    public class BeSmart_Main_CautaProdusFragment : Fragment
    {
        Button ChangeDistance;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            try
            {
                MapsInitializer.Initialize(Activity);
            }
            catch (GooglePlayServicesNotAvailableException e)
            {
                e.PrintStackTrace();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Extra_BeSmart_MainView_CautaProdus,null,false);

            ChangeDistance = v.FindViewById<Button>(Resource.Id.button1);
            TextView DistantaCautare = v.FindViewById<TextView>(Resource.Id.textView1);
            SearchView SearchBar = v.FindViewById<SearchView>(Resource.Id.searchView1);
            RelativeLayout AdvanceSearch = v.FindViewById<RelativeLayout>(Resource.Id.relativeLayout4);

            SetTypeface.Normal.SetTypeFace(Activity, ChangeDistance);
            SetTypeface.Normal.SetTypeFace(Activity, DistantaCautare);

            LocationManager lm = (LocationManager)Activity.GetSystemService(Activity.LocationService);
            Location l = lm.GetLastKnownLocation(LocationManager.NetworkProvider);

            LatLng location = new LatLng(double.Parse(l.Latitude.ToString()), double.Parse(l.Longitude.ToString()));
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(18);

            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            MapFragment mapFrag = MapFragment.NewInstance();
            mapFrag = (MapFragment)((Android.Support.V4.App.FragmentActivity)Activity).FragmentManager.FindFragmentById(Resource.Id.map);
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
                    diag.Show(FragmentManager, "SetareDistanta2");

                    diag.DistanceChanged += ((object sender1, DistanceChangedEventArgs e1) =>
                    {
                        ChangeDistance.Text = e1.Value;

                        RazaCautare.Remove();

                        CircleOpts.InvokeRadius(int.Parse(e1.Value));
                        RazaCautare = map.AddCircle(CircleOpts);
                    });
                });
            }

            SearchBar.QueryTextSubmit += SearchBar_QueryTextSubmit;
            AdvanceSearch.Click += AdvanceSearch_Click;

           return v;
        }

        void SearchBar_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            Intent ViewResoults = new Intent(Activity,typeof(BeSmart_ViewResoult));
            ViewResoults.PutExtra("Distance",ChangeDistance.Text);
            ViewResoults.PutExtra("Query",e.Query);
            Activity.StartActivity(ViewResoults);
        }

        void AdvanceSearch_Click(object sender, EventArgs e)
        {
            BeSmart_AdvancedSearch_DialogFragment diag = new BeSmart_AdvancedSearch_DialogFragment();
            diag.Show(FragmentManager, "AdvancedSearch");
        }
    }
}