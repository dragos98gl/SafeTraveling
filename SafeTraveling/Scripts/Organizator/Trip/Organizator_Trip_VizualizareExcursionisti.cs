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
using Ns = SafeTraveling.Scripts.StartUpService.NotificationService;
using Android.Support.V4.App;
using Android.Views.Animations;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using System.Net.Sockets;
using System.IO;
using Android.Graphics.Drawables;
using Android.Locations;

namespace SafeTraveling.Scripts.Organizator.Trip
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]			
    public class Organizator_Trip_VizualizareExcursionisti:FragmentActivity
    {
        static GridView AfisareTipGrid;
        static Activity context;
        FrameLayout FragmentContainer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen,WindowManagerFlags.Fullscreen);

            base.OnCreate(savedInstanceState);
            context = this;

            SetContentView(Resource.Layout.Organizator_Trip_VizualizareExcursionisti);

            AfisareTipGrid = FindViewById<GridView>(Resource.Id.GridViewLay);
            FragmentContainer = FindViewById<FrameLayout>(Resource.Id.FragmentContainer);

            var transact = FragmentManager.BeginTransaction();
            transact.Add(FragmentContainer.Id,new Organizator_Trip_VizualizareExcursionisti_GoogleMapsFragment(FragmentContainer),"GoogleMapsFragment");
            transact.Commit();

            OvershootInterpolator interpolator = new OvershootInterpolator(1);
            FragmentContainer.Animate().SetInterpolator(interpolator)
                .TranslationYBy(-605)
                .SetDuration(100);
        }

        public static void LocationUpdated(List<Excursionist> Excursionisti)
        {
            if (AfisareTipGrid != null)
                context.RunOnUiThread(() =>{
                    AfisareTipGrid.Adapter = new Organizator_Trip_VizualizatreExcursionisti_Adapter(context, Excursionisti.ToArray());

                    LocationManager lm = (LocationManager)context.GetSystemService(LocationService);
                    Location l = lm.GetLastKnownLocation(LocationManager.NetworkProvider);

                    LatLng location = new LatLng(double.Parse(l.Latitude.ToString()), double.Parse(l.Longitude.ToString()));
                    CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                    builder.Target(location);
                    builder.Zoom(18);

                    CameraPosition cameraPosition = builder.Build();
                    CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

                    MapFragment mapFrag = MapFragment.NewInstance();
                    mapFrag = (MapFragment)((FragmentActivity)context).FragmentManager.FindFragmentById(Resource.Id.map);
                    if (mapFrag != null)
                    {
                        GoogleMap map = mapFrag.Map;
                        map.Clear();

                        List<MarkerOptions> Markers = new List<MarkerOptions>();

                        foreach (Excursionist e in Excursionisti.ToList())
                        {
                            Bitmap UserPhoto = ((BitmapDrawable)e.Photo).Bitmap;
                            
                            MarkerOptions UserMarker = new MarkerOptions();
                            try
                            {
                                string Latitude = e.Pozitie.Split(',')[0];
                                string Longitude = e.Pozitie.Split(',')[1];

                                UserMarker.SetPosition(new LatLng(double.Parse(Latitude.Replace('.', ',')), double.Parse(Longitude.Replace('.', ','))));
                                UserMarker.SetIcon(BitmapDescriptorFactory.FromBitmap(UserPhoto));
                                UserMarker.SetTitle(e.Nume + " " + e.Prenume);
                                map.AddMarker(UserMarker);
                            }
                            catch { }
                        }

                        map.MapType = GoogleMap.MapTypeNormal;
                        map.MoveCamera(cameraUpdate);
                    }
                });
       
        }
    }
}