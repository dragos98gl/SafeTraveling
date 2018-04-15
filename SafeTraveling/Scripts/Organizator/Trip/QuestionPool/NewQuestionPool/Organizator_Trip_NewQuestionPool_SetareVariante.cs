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

namespace SafeTraveling
{
    public class Organizator_Trip_NewQuestionPool_SetareVariante : Fragment
    {
        public ListView VarianteListView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Organizator_Trip_NewQuestionPool_SetareVariante, null, false);

            RelativeLayout AdaugareVariante = v.FindViewById<RelativeLayout>(Resource.Id.relativeLayout2);
            VarianteListView = v.FindViewById<ListView>(Resource.Id.listView1);
            TextView AdaugareVariante_TView = v.FindViewById<TextView>(Resource.Id.textView1);

            SetTypeface.Normal.SetTypeFace(Activity,AdaugareVariante_TView);
            VarianteListView.Adapter = new Organizator_Trip_NewQuestionPool_SetareVariante_VarianteAdapter(Activity, new string[]{},VarianteListView);

            AdaugareVariante.Click += ((object sender, EventArgs e) =>
            {
                Dialog diag = new Dialog(Activity);
                diag.Window.RequestFeature(WindowFeatures.NoTitle);

                View CostumView = inflater.Inflate(Resource.Layout.Organizator_Trip_NewQuestionPool_SetareVariante_AlertDialogAdapter, null);
                TextView NumeCamp = CostumView.FindViewById<TextView>(Resource.Id.textView1);
                ImageView EditInfo = CostumView.FindViewById<ImageView>(Resource.Id.imageView1);
                EditText NewValue = CostumView.FindViewById<EditText>(Resource.Id.editText1);

                SetTypeface.Normal.SetTypeFace(Activity, NumeCamp);
                SetTypeface.Normal.SetTypeFace(Activity, NewValue);

                NumeCamp.Text = "Introduceti varianta:           ";
                EditInfo.Click += (object sender1, EventArgs e1) =>
                {
                    string[] LastVariante=((Organizator_Trip_NewQuestionPool_SetareVariante_VarianteAdapter)VarianteListView.Adapter).Variante;
                    List<string> Variante = new List<string>(LastVariante); 
                    Variante.Add(NewValue.Text);
                    
                    VarianteListView.Adapter = new Organizator_Trip_NewQuestionPool_SetareVariante_VarianteAdapter(Activity,Variante.ToArray(),VarianteListView);

                    diag.Cancel();
                };

                diag.SetContentView(CostumView);

                diag.SetCanceledOnTouchOutside(true);
                diag.Show();
                diag.Window.SetBackgroundDrawable(Activity.Resources.GetDrawable(Resource.Drawable.background_MarginiOvaleAlb));
            });

            return v;
        }
    }
}