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
using System.Net.Sockets;

namespace SafeTraveling
{
    public class Organizator_Trip_NewQuestionPool_SetareDurata : Fragment
    {
        Organizator_Trip_NewQuestionPool_IntroducereIntrebare IntroducereIntrebare;
        Organizator_Trip_NewQuestionPool_SetareVariante SetareVariante;
        
        public Organizator_Trip_NewQuestionPool_SetareDurata(Organizator_Trip_NewQuestionPool_IntroducereIntrebare IntroducereIntrebare,Organizator_Trip_NewQuestionPool_SetareVariante SetareVariante)
        {
            this.IntroducereIntrebare = IntroducereIntrebare;
            this.SetareVariante = SetareVariante;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Organizator_Trip_NewQuestionPool_SetareDurata, null, false);

            NumberPicker PickDurata = v.FindViewById<NumberPicker>(Resource.Id.numberPicker1);
            TextView IntroducetiDurata_TView = v.FindViewById<TextView>(Resource.Id.textView1);
            TextView MinuteHint = v.FindViewById<TextView>(Resource.Id.textView2);
            TextView TrimitereIntrebare_TView = v.FindViewById<TextView>(Resource.Id.textView3);
            RelativeLayout TrimitereIntrebare = v.FindViewById<RelativeLayout>(Resource.Id.relativeLayout2);

            SetTypeface.Normal.SetTypeFace(Activity,TrimitereIntrebare_TView);
            SetTypeface.Normal.SetTypeFace(Activity, IntroducetiDurata_TView);
            SetTypeface.Normal.SetTypeFace(Activity, MinuteHint);

            PickDurata.MaxValue = 60;
            PickDurata.MinValue = 10;
            PickDurata.WrapSelectorWheel = false;

            TrimitereIntrebare.Click += ((object sender, EventArgs e) => {
                TcpClient Client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
                NetworkStream ns = Client.GetStream();

                string TripId = new SaveUsingSharedPreferences(Activity).LoadString(SaveUsingSharedPreferences.Tags.Trip.TipId);
                string NrTel = new SaveUsingSharedPreferences(Activity).LoadString(SaveUsingSharedPreferences.Tags.Login.Username);
                string Intrebare = IntroducereIntrebare.IntroducetiIntrebarea_EText.Text;
                string Variante = string.Join(",",((Organizator_Trip_NewQuestionPool_SetareVariante_VarianteAdapter)SetareVariante.VarianteListView.Adapter).Variante);
                string Durata = PickDurata.Value.ToString();
                string Id = Guid.NewGuid().ToString();

                _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.NewQuestionPool, NrTel, TripId, Intrebare, Variante, Durata ,Id}));

                Activity.Finish();
            });

            return v;
        }
    }
}