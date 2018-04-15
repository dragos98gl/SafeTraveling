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
    class Organizator_Trip_ViewQuestionPool_Adapters
    {
        public class Organizator_Trip_ViewQuestionPool_ListaIntrebari_Adapter : ArrayAdapter<IntrebarVariante>
        {
            Activity context;
            ListView ListaVariante;
            IntrebarVariante[] IntrebareVariante;

            public Organizator_Trip_ViewQuestionPool_ListaIntrebari_Adapter(Activity context, ListView ListaVariante, IntrebarVariante[] IntrebareVariante)
                : base(context, Resource.Layout.Organizator_Trip_ViewQuestionPool_ListaIntrebare_Adapter, IntrebareVariante)
            {
                this.context = context;
                this.ListaVariante = ListaVariante;
                this.IntrebareVariante = IntrebareVariante;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                LayoutInflater inflater = context.LayoutInflater;
                View v = inflater.Inflate(Resource.Layout.Organizator_Trip_ViewQuestionPool_ListaIntrebare_Adapter,null,false);

                TextView Intrebare = v.FindViewById<TextView>(Resource.Id.textView1);
                RelativeLayout Container = v.FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);

                SetTypeface.Normal.SetTypeFace(context,Intrebare);
                Intrebare.Text = IntrebareVariante[position].Intrebare;

                Container.Click += ((object sender, EventArgs e) => {
                    ListaVariante.Adapter = new Organizator_Trip_ViewQuestionPool_ListaVariante_Adapter(context,IntrebareVariante[position].Variante,IntrebareVariante[position].Procente);
                });

                return v;
            }
        }

        public class Organizator_Trip_ViewQuestionPool_ListaVariante_Adapter : ArrayAdapter<string>
        {
            Activity context;
            string[] Variante;
            string[] Procente;

            public Organizator_Trip_ViewQuestionPool_ListaVariante_Adapter(Activity context,string[] Variante,string[] Procente):base(context,Resource.Layout.Organizator_Trip_ViewQuestionPool_ListaVariante_Adapter,Variante)
            {
                this.context = context;
                this.Variante = Variante;
                this.Procente = Procente;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                LayoutInflater inflater = context.LayoutInflater;
                View v = inflater.Inflate(Resource.Layout.Organizator_Trip_ViewQuestionPool_ListaVariante_Adapter,null,false);

                TextView Varianta = v.FindViewById<TextView>(Resource.Id.textView1);
                TextView Procent = v.FindViewById<TextView>(Resource.Id.textView2);

                SetTypeface.Normal.SetTypeFace(context, Varianta);
                SetTypeface.Normal.SetTypeFace(context, Procent);

                Varianta.Text = Variante[position];
                Procent.Text = Procente[position];

                return v;
            }
        }

        public struct IntrebarVariante
        {
            public string Intrebare;
            public string[] Variante;
            public string[] Procente; 
        }
    }
}