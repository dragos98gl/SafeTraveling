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
    class BeSmart_ViewResoult_Adapter:ArrayAdapter<string>
    {
        Activity context;
        string[] NumeProduse;
        string[] Preturi;
        string[] Distante;
        string[] Locatii;

        public BeSmart_ViewResoult_Adapter(Activity context,string[] NumeProduse,string[] Preturi,string[] Distante,string[] Locatii):base(context,Resource.Layout.Extra_BeSmart_ViewResoult_Adapter,NumeProduse)
        {
            this.context = context;
            this.Preturi = Preturi;
            this.NumeProduse = NumeProduse;
            this.Distante = Distante;
            this.Locatii = Locatii;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = context.LayoutInflater;
            View v = inflater.Inflate(Resource.Layout.Extra_BeSmart_ViewResoult_Adapter,null,false);

            RelativeLayout container = v.FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
            TextView NumeProdus = v.FindViewById<TextView> (Resource.Id.textView1);
            TextView Pret = v.FindViewById<TextView> (Resource.Id.textView2);
            TextView Distanta = v.FindViewById<TextView> (Resource.Id.textView3);

            SetTypeface.Normal.SetTypeFace(context,NumeProdus);
            SetTypeface.Normal.SetTypeFace(context,Pret);
            SetTypeface.Normal.SetTypeFace(context,Distanta);

            NumeProdus.Text = NumeProduse[position];
            Distanta.Text = Distante[position];
            Pret.Text = Preturi[position];

            container.Click += ((object sender, EventArgs e) => {
                string Long = Locatii[position].Split(',')[0];
                string Lat = Locatii[position].Split(',')[1];

                Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("geo:<"+Long+">,<"+Lat+">?q=<"+Long+">,<"+Lat+">("+NumeProduse[position]+")"));
                context.StartActivity(intent);
            });

            return v;
        }
    }
}