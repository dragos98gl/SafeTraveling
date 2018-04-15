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
using System.Net.Sockets;

namespace SafeTraveling.Scripts.Utilizator.Trip.VoteQuestionPool
{
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class Utilizator_Trip_VoteQuestionPool : Activity
    {
        ListView ListaVariante;
        Button Vote;
        TextView Intrebare_TView;
        string Id;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Bundle Extras = Intent.Extras;

            string Intrebare = Extras.GetString(_Details.QuestionPoolIntrebare);
            string Variante = Extras.GetString(_Details.QuestionPoolVariante);
            Id = Extras.GetString(_Details.QuestionPoolId);

            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Utilizator_Trip_VoteQuestionPool);

            ListaVariante = FindViewById<ListView>(Resource.Id.listView1);
            Vote = FindViewById<Button>(Resource.Id.button1);
            Intrebare_TView = FindViewById<TextView>(Resource.Id.textView1);

            SetTypeface.Normal.SetTypeFace(this,Vote);
            SetTypeface.Normal.SetTypeFace(this, Intrebare_TView);

            Intrebare_TView.Text = Intrebare;
            ListaVariante.Adapter = new Utilizator_Trip_VoteQuestionPool_VarianteAdapter(this,Variante.Split(','),ListaVariante);

            Vote.Click += Vote_Click;
        }

        void Vote_Click(object sender, EventArgs e)
        {
            int? CheckedRow = (ListaVariante.Adapter as Utilizator_Trip_VoteQuestionPool_VarianteAdapter).CheckedRow;

            if (CheckedRow != null)
            {
                TcpClient Client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
                NetworkStream ns = Client.GetStream();

                string TripId = new SaveUsingSharedPreferences(this).LoadString(SaveUsingSharedPreferences.Tags.Trip.TipId);
                string Index = CheckedRow.ToString();

                _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] {_Details.VoteQuestionPool, TripId, Index, Id }));

                Finish();
            }
        }
    }
}