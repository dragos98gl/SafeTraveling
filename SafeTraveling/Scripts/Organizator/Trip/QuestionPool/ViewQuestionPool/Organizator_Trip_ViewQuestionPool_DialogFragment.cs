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

namespace SafeTraveling
{
    class Organizator_Trip_ViewQuestionPool_DialogFragment:DialogFragment
    {
        Organizator_Trip_ViewQuestionPool_Adapters.IntrebarVariante[] Votes;

        public Organizator_Trip_ViewQuestionPool_DialogFragment(Organizator_Trip_ViewQuestionPool_Adapters.IntrebarVariante[] Votes)
        {
            this.Votes = Votes;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Organizator_Trip_ViewQuestionPool_DialogAdapter,null,false);

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            RelativeLayout Frame = v.FindViewById<RelativeLayout>(Resource.Id.relativeLayout2);
            ListView ListaIntrebari = v.FindViewById<ListView>(Resource.Id.listView1);
            ListView ListaVariante = v.FindViewById<ListView>(Resource.Id.listView2);

            ListaIntrebari.Adapter = new Organizator_Trip_ViewQuestionPool_Adapters.Organizator_Trip_ViewQuestionPool_ListaIntrebari_Adapter(Activity,ListaVariante,Votes);
            
            WindowManagerLayoutParams lp = new WindowManagerLayoutParams();
            lp.CopyFrom(Dialog.Window.Attributes);
            lp.Height = Frame.LayoutParameters.Height;
            lp.Width = Frame.LayoutParameters.Width;
            lp.Alpha = 0.8f;

            Dialog.SetCanceledOnTouchOutside(true);
            Dialog.Show();

            Dialog.Window.Attributes = lp;
            return v;
        }
    }
}