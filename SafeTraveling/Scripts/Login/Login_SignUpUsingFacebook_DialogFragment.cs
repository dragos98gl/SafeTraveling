using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using SafeTraveling.Login_SignUpUsingFacebook_Fragments;
using System.Net;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.Net.Sockets;
using System.IO;
using Java.IO;

namespace SafeTraveling
{
    class Login_SignUpUsingFacebook_DialogFragment : DialogFragment
    {
        string[] DateUtilizatorNou;
        Fragment[] Frags;
        TextView ActualFragment;
        Bitmap ProfilePic;

        public Login_SignUpUsingFacebook_DialogFragment(string[] DateUtilizatorNou)
        {
            this.DateUtilizatorNou = DateUtilizatorNou;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.Login_SignUpUsingFacebook_DialogAdapter, null, false);

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            FrameLayout Container = v.FindViewById<FrameLayout>(Resource.Id.frameLayout1);
            RelativeLayout Frame = v.FindViewById<RelativeLayout>(Resource.Id.relativeLayout2);
            TextView NumeUtilizator = v.FindViewById<TextView>(Resource.Id.textView1);
            ImageView PozaUtilizator = v.FindViewById<ImageView>(Resource.Id.imageView1);
            ImageView LeftArrow = v.FindViewById<ImageView>(Resource.Id.imageView2);
            ImageView RightArrow = v.FindViewById<ImageView>(Resource.Id.imageView3);
            ActualFragment = v.FindViewById<TextView>(Resource.Id.textView2);

            SetTypeface.Normal.SetTypeFace(Activity, ActualFragment);
            SetTypeface.Normal.SetTypeFace(Activity, NumeUtilizator);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DateUtilizatorNou[5]);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            ProfilePic = BitmapFactory.DecodeStream(response.GetResponseStream());
            NumeUtilizator.Text = DateUtilizatorNou[0] + " " + DateUtilizatorNou[1];
            PozaUtilizator.SetImageDrawable(new BitmapDrawable(Activity.Resources, RoundedBitmap.MakeRound(ProfilePic, ProfilePic.Height / 2)));

            Login_SignUpUsingFacebook_NumarTelefon NumarTelefon_Fragment = new Login_SignUpUsingFacebook_NumarTelefon();
            Login_SignUpUsingFacebook_TipCont TipCont_Fragment = new Login_SignUpUsingFacebook_TipCont();
            Login_SignUpUsingFacebook_Parola Parola_Fragment = new Login_SignUpUsingFacebook_Parola();

            Frags = new Fragment[] { 
                NumarTelefon_Fragment,
                TipCont_Fragment,
                Parola_Fragment
            };

            var trans = ChildFragmentManager.BeginTransaction();
            trans.Add(Container.Id, Parola_Fragment, "Parola");
            trans.Add(Container.Id, TipCont_Fragment, "TipCont");
            trans.Add(Container.Id, NumarTelefon_Fragment, "NumarTelefon");
            trans.Hide(Parola_Fragment);
            trans.Hide(TipCont_Fragment);
            trans.Commit();

            WindowManagerLayoutParams lp = new WindowManagerLayoutParams();
            lp.CopyFrom(Dialog.Window.Attributes);
            lp.Height = Frame.LayoutParameters.Height;
            lp.Width = Frame.LayoutParameters.Width;
            lp.Alpha = 0.8f;

            Dialog.SetCanceledOnTouchOutside(true);
            Dialog.Show();

            Dialog.Window.Attributes = lp;

            RightArrow.Click += RightArrow_Click;
            LeftArrow.Click += LeftArrow_Click;

            return v;
        }

        int current = 0;
        void LeftArrow_Click(object sender, EventArgs e)
        {
            if (current > 0)
            {
                var trans = ChildFragmentManager.BeginTransaction();

                trans.SetCustomAnimations(
                    Resource.Animation.TranslationNext,
                    Resource.Animation.TranslateForward,
                    Resource.Animation.TranslationNext,
                    Resource.Animation.TranslateForward
                    );

                trans.Hide(Frags[current]);
                trans.Show(Frags[current - 1]);
                trans.Commit();

                current--;

                ActualFragment.Text = (current + 1).ToString() + "/3";
            }
        }

        void RightArrow_Click(object sender, EventArgs e)
        {
            if (current < 2)
            {
                var trans = ChildFragmentManager.BeginTransaction();

                trans.SetCustomAnimations(
                    Resource.Animation.TranslationNext,
                    Resource.Animation.TranslateForward,
                    Resource.Animation.TranslationNext,
                    Resource.Animation.TranslateForward
                    );

                trans.Hide(Frags[current]);
                trans.Show(Frags[current + 1]);
                trans.Commit();

                current++;

                ActualFragment.Text = (current + 1).ToString() + "/3";
            }
            else
            {
                string Nume = DateUtilizatorNou[0];
                string Prenume = DateUtilizatorNou[1];
                string Sex = DateUtilizatorNou[2];
                string Varsta = DateUtilizatorNou[3];
                string Email = DateUtilizatorNou[4];
                string PhotoURL = DateUtilizatorNou[5];
                string NumarTelefon = ((Login_SignUpUsingFacebook_NumarTelefon)Frags[0]).NumarTelefon.Text;
                string Parola = ((Login_SignUpUsingFacebook_Parola)Frags[2]).Parola.Text;
                string TipCont = string.Empty;

                    if (((Login_SignUpUsingFacebook_TipCont)Frags[1]).Utilizator.Checked)
                        TipCont = "Organizator";
                    else
                        TipCont = "Utilizator";

                TcpClient Client = new TcpClient(_Details.ServerIP, _Details.LoginSignUpNewTripPort);
                NetworkStream ns = Client.GetStream();

                _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { "SIGNUP", Nume, Prenume, Varsta, Sex, Email, NumarTelefon, TipCont, Parola }));

                if (CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns))[0] == "ok")
                {
                    Client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
                    ns = Client.GetStream();

                    MemoryStream m = new MemoryStream();
                    ByteArrayOutputStream s = new ByteArrayOutputStream();
                    ProfilePic.Compress(Bitmap.CompressFormat.Png,100,m);


                    using (MemoryStream ms = new MemoryStream(m.ToArray()))
                    {
                        int PackSize = 1000;
                        int TotalLength = (int)ms.Length;
                        int NoOfPackets = (int)System.Math.Ceiling((double)ms.Length / (double)PackSize);
                        int CurrentPackSize;

                        _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] { _Details.UpdateProfilePic, NumarTelefon }));

                        for (int i = 0; i < NoOfPackets; i++)
                        {
                            if (TotalLength > PackSize)
                            {
                                TotalLength -= PackSize;
                                CurrentPackSize = PackSize;
                            }
                            else
                                CurrentPackSize = TotalLength;

                            byte[] CurrentBytes = new byte[CurrentPackSize];
                            int ReadedLength = ms.Read(CurrentBytes, 0, CurrentBytes.Length);

                            ns.Write(CurrentBytes, 0, ReadedLength);
                        }

                        Client.Close();
                    }

                    Toast.MakeText(Activity, "Ala-i bosssss!", ToastLength.Long).Show();
                    Dialog.Cancel();
                }
                else
                {
                    Toast.MakeText(Activity, "Fail!", ToastLength.Long).Show();
                }
            }
        }
    }
}