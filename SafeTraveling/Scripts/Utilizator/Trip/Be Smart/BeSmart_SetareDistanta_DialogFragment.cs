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

namespace SafeTraveling.Scripts.BeSmart
{
    class BeSmart_SetareDistanta_DialogFragment:DialogFragment
    {
        public delegate void DistanceChangeHandler(object sender, DistanceChangedEventArgs e);
        public event DistanceChangeHandler DistanceChanged;

        public virtual void OnDistangeChange(string Value)
        {
            if (DistanceChanged != null)
                DistanceChanged(this, new DistanceChangedEventArgs() { Value = Value});
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Extra_BeSmart_SetareDistanta_DialogFragment,null,false);

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            RelativeLayout Container = v.FindViewById<RelativeLayout>(Resource.Id.relativeLayout2);
            ListView Distante = v.FindViewById<ListView>(Resource.Id.listView1);
            SearchView SearchBar = v.FindViewById<SearchView>(Resource.Id.searchView1);

            Distante.Adapter = new BeSmart_SetareDistanta_Adapter(Activity,new string[] {
                "50",
                "100",
                "200",
                "300",
                "500",
                "1000",
                "1500",
                "2000",
                "3000",
                "4000",
                "5000",
                "10000",
                "20000",
                "30000"
            },this);

            WindowManagerLayoutParams lp = new WindowManagerLayoutParams();
            lp.CopyFrom(Dialog.Window.Attributes);
            lp.Height = Container.LayoutParameters.Height;
            lp.Width = Container.LayoutParameters.Width;

            Dialog.Show();

            Dialog.Window.Attributes = lp;

            return v;
        }
    }

    class DistanceChangedEventArgs:EventArgs
    {
        public string Value;
    }
}