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
    class BeSmart_SetareDistanta_Adapter:ArrayAdapter<string>
    {
        Activity context;
        string[] Distante;
        BeSmart_SetareDistanta_DialogFragment Parent;

        public BeSmart_SetareDistanta_Adapter(Activity context, string[] Distante, BeSmart_SetareDistanta_DialogFragment Parent)
            : base(context, Resource.Layout.Extra_BeSmart_SetareDistanta_Adapter, Distante)
        {
            this.context = context;
            this.Distante = Distante;
            this.Parent = Parent;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = context.LayoutInflater;
            View v = inflater.Inflate(Resource.Layout.Extra_BeSmart_SetareDistanta_Adapter,null,false);

            TextView Distanta = v.FindViewById<TextView>(Resource.Id.textView1);

            SetTypeface.Normal.SetTypeFace(context,Distanta);

            Distanta.Text = Distante[position];
            Distanta.Click += ((object sender, EventArgs e) => {
                Parent.OnDistangeChange(Distante[position]);
                Parent.Dialog.Cancel();
            });

            return v;
        }
    }
}