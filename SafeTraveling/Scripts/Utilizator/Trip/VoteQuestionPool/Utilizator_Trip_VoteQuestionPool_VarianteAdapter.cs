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

namespace SafeTraveling.Scripts.Utilizator.Trip.VoteQuestionPool
{
    public class Utilizator_Trip_VoteQuestionPool_VarianteAdapter : ArrayAdapter<string>
    {
        public int? CheckedRow = null;
        Activity context;
        ListView Parent;
        string[] Variante;

        public Utilizator_Trip_VoteQuestionPool_VarianteAdapter(Activity context,string[] Variante,ListView Parent):base(context,Resource.Layout.Utilizator_Trip_VoteQuestionPool_VarianteAdapter,Variante)
        {
            this.context = context;
            this.Variante = Variante;
            this.Parent = Parent;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = context.LayoutInflater;
            View v = inflater.Inflate(Resource.Layout.Utilizator_Trip_VoteQuestionPool_VarianteAdapter,null,false);

            RadioButton VoteButton = v.FindViewById<RadioButton>(Resource.Id.radioButton1);
            VoteButton.Text = Variante[position];

            if (CheckedRow!=null)
                if (position.Equals(CheckedRow))
                    VoteButton.Checked = true;

            VoteButton.CheckedChange += ((object sender, CompoundButton.CheckedChangeEventArgs e)=>{
                CheckedRow = position;
                Parent.Adapter = this;
            });

            SetTypeface.Normal.SetTypeFace(context,VoteButton);

            return v;
        }
    }
}