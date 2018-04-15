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
    class Organizator_Trip_NewQuestionPool_SetareVariante_VarianteAdapter:ArrayAdapter<string>
    {
        Activity context;
        ListView parentLV;
        public string[] Variante;

        public Organizator_Trip_NewQuestionPool_SetareVariante_VarianteAdapter(Activity context,string[] Variante,ListView parentLV):base (context,Resource.Layout.Organizator_Trip_NewQuestionPool_SetareVariante_VariantaAdapter,Variante)
        {
            this.context = context;
            this.Variante = Variante;
            this.parentLV = parentLV;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = context.LayoutInflater;
            View v = inflater.Inflate(Resource.Layout.Organizator_Trip_NewQuestionPool_SetareVariante_VariantaAdapter, null, false);

            RelativeLayout Container = v.FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
            TextView Varianta = v.FindViewById<TextView>(Resource.Id.textView1);
            ImageView StergereVarianta = v.FindViewById<ImageView>(Resource.Id.imageView1);

            SetTypeface.Normal.SetTypeFace(context,Varianta);

            Varianta.Text = Variante[position];

            StergereVarianta.Click += ((object obj, EventArgs e) =>
            {
                List<string> tempList = new List<string>(Variante);
                tempList.RemoveAt(position);

                parentLV.Adapter = new Organizator_Trip_NewQuestionPool_SetareVariante_VarianteAdapter(context,tempList.ToArray(),parentLV);
            });

            return v;
        }
    }
}