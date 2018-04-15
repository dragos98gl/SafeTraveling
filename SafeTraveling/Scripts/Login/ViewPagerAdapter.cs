using System;
using Android.Support.V4.App;
using Android.Widget;
using Android.App;
using Android.Views;
using Android.OS;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.View;
using System.Net.Mail;
using System.Net;
using System.Net.Sockets;
using Android.Content;

namespace SafeTraveling
{
	public class ViewPagerAdapter:FragmentStatePagerAdapter
	{
		int[] Layouturi;
        SignUp context;
		ViewPager Pager;

		public ViewPagerAdapter (Android.Support.V4.App.FragmentManager fm,int[] Layouturi,SignUp context,ViewPager Pager):base(fm)
		{
			this.Layouturi = Layouturi;
			this.context = context;
			this.Pager = Pager;
		}

		public override Android.Support.V4.App.Fragment GetItem (int position)
		{
			return new PagerFragment(position,Layouturi,context,Pager);
		}

		public override int Count {
			get {
				return Layouturi.Length;
			}
		}
	}

	public class PagerFragment : Android.Support.V4.App.Fragment
	{
		int position;
        SignUp context;
		int[] Layouturi;
		ViewPager Pager;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

        public PagerFragment(int position, int[] Layouturi, SignUp context, ViewPager Pager)
		{
			this.position = position;
			this.context = context;
			this.Layouturi = Layouturi;
			this.Pager = Pager;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Layouturi[position], container, false);
			SaveUsingSharedPreferences Save = new SaveUsingSharedPreferences (context);

			switch (position) {
			case 0:
				{
					TextView Nume_TView = v.FindViewById<TextView> (Resource.Id.textView1);
					TextView Prenume_TView = v.FindViewById<TextView> (Resource.Id.textView2);
					TextView Varsta_TView = v.FindViewById<TextView> (Resource.Id.textView3);
					TextView Sex_TView = v.FindViewById<TextView> (Resource.Id.textView4);
					RadioButton Masculin = v.FindViewById<RadioButton> (Resource.Id.radioButton1);
					RadioButton Feminin = v.FindViewById<RadioButton> (Resource.Id.radioButton3);
					EditText Nume_EText = v.FindViewById<EditText> (Resource.Id.editText1);
					EditText Prenume_EText = v.FindViewById<EditText> (Resource.Id.editText2);
					EditText Varsta_EText = v.FindViewById<EditText> (Resource.Id.editText3);

					Nume_EText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
						Save.Save(SaveUsingSharedPreferences.Tags.SetUp.Nume ,Nume_EText.Text);
					};

					Prenume_EText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
						Save.Save(SaveUsingSharedPreferences.Tags.SetUp.Prenume,Prenume_EText.Text);
					};

					Varsta_EText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
						Save.Save(SaveUsingSharedPreferences.Tags.SetUp.Varsta, Varsta_EText.Text);
					};

					Save.Save(SaveUsingSharedPreferences.Tags.SetUp.Sex,"Masculin");
					Masculin.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => {
						if(e.IsChecked)
							Save.Save(SaveUsingSharedPreferences.Tags.SetUp.Sex,"Masculin");
						else 
							Save.Save(SaveUsingSharedPreferences.Tags.SetUp.Sex,"Feminin");
					};

					SetTypeface.Normal.SetTypeFace (context,Nume_TView);
					SetTypeface.Normal.SetTypeFace (context,Prenume_TView);
					SetTypeface.Normal.SetTypeFace (context,Varsta_TView);
					SetTypeface.Normal.SetTypeFace (context,Sex_TView);
					SetTypeface.Normal.SetTypeFace (context,Masculin);
					SetTypeface.Normal.SetTypeFace (context,Feminin);
				}
				break;
			
			case 1:
				{
					TextView Email_TView = v.FindViewById<TextView> (Resource.Id.textView1);
					TextView NumarTelefon_TView = v.FindViewById<TextView> (Resource.Id.textView2);
					TextView Mark = v.FindViewById<TextView> (Resource.Id.textView3);
					TextView TipCont_TView = v.FindViewById<TextView> (Resource.Id.textView4);
					RadioButton Utilizator = v.FindViewById<RadioButton> (Resource.Id.radioButton1);
					RadioButton Organizator = v.FindViewById<RadioButton> (Resource.Id.radioButton3);
					EditText Email_EText = v.FindViewById<EditText> (Resource.Id.editText1);
					EditText NumarTelefon_EText = v.FindViewById<EditText> (Resource.Id.editText2);

					SetTypeface.Normal.SetTypeFace (context,Email_TView);
					SetTypeface.Normal.SetTypeFace (context,NumarTelefon_TView);
					SetTypeface.Normal.SetTypeFace (context,Mark);
					SetTypeface.Normal.SetTypeFace (context,TipCont_TView);
					SetTypeface.Normal.SetTypeFace (context,Utilizator);
					SetTypeface.Normal.SetTypeFace (context,Organizator);

					Save.Save(SaveUsingSharedPreferences.Tags.SetUp.TipCont,"Utilizator");
					Utilizator.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => {
						if(e.IsChecked)
							Save.Save(SaveUsingSharedPreferences.Tags.SetUp.TipCont,"Utilizator");
						 else 
							Save.Save(SaveUsingSharedPreferences.Tags.SetUp.TipCont,"Organizator");
					};

					Email_EText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
						Save.Save(SaveUsingSharedPreferences.Tags.SetUp.Email, Email_EText.Text);
					};

					NumarTelefon_EText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
						Save.Save(SaveUsingSharedPreferences.Tags.SetUp.NumarTelefon ,NumarTelefon_EText.Text);
					};
				}
				break;

			case 2:
				{
					ImageView ChaptaView = v.FindViewById<ImageView> (Resource.Id.imageView1);
					TextView VIntroducetiCodul = v.FindViewById<TextView> (Resource.Id.textView1);
					TextView VParola = v.FindViewById <TextView> (Resource.Id.textView2);
					TextView VConfParola = v.FindViewById<TextView> (Resource.Id.textView3);
					Button Gata = v.FindViewById<Button> (Resource.Id.button1);
					EditText Chapta = v.FindViewById<EditText> (Resource.Id.editText1);
					EditText Parola_EText = v.FindViewById<EditText> (Resource.Id.editText2);
					EditText ConfParola_Etext = v.FindViewById<EditText> (Resource.Id.editText3);

					SetTypeface.Normal.SetTypeFace (context, VIntroducetiCodul);
					SetTypeface.Normal.SetTypeFace (context, Gata);
					SetTypeface.Normal.SetTypeFace (context, VParola);
					SetTypeface.Normal.SetTypeFace (context, VConfParola);

					DisplayChapta displayChapta = new DisplayChapta (5);
					displayChapta.Create ();
					string ChaptaText = displayChapta.GetText ();
					Bitmap b = displayChapta.GetBitmap ();
					ChaptaView.SetImageBitmap (b);

					Gata.Click += (object sender, EventArgs e) => {
						string Nume = Save.LoadString (SaveUsingSharedPreferences.Tags.SetUp.Nume);
						string Prenume =Save.LoadString (SaveUsingSharedPreferences.Tags.SetUp.Prenume);
						string Varsta = Save.LoadString (SaveUsingSharedPreferences.Tags.SetUp.Varsta);
						string Sex = Save.LoadString (SaveUsingSharedPreferences.Tags.SetUp.Sex);
						string Email = Save.LoadString (SaveUsingSharedPreferences.Tags.SetUp.Email);
						string TipCont = Save.LoadString (SaveUsingSharedPreferences.Tags.SetUp.TipCont);
						string NumarTelefon = Save.LoadString (SaveUsingSharedPreferences.Tags.SetUp.NumarTelefon);
						string Parola = Parola_EText.Text;
						string ConfParola = ConfParola_Etext.Text;

						//if (VerificareDate (
						//	Nume,Prenume,Varsta,Email,NumarTelefon,Parola,ConfParola,Chapta.Text, ChaptaText)) {

						TcpClient Client = new TcpClient(_Details.ServerIP,_Details.LoginSignUpNewTripPort);
							NetworkStream ns = Client.GetStream();

							_TcpDataExchange.WriteStreamString(ns,CryptDecryptData.CryptData(new string[] {"SIGNUP",Nume,Prenume,Varsta,Sex,Email,NumarTelefon,TipCont,Parola}));

							if ( CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns))[0]=="ok")
						{
                            Toast.MakeText(context, "Ala-i bosssss!", ToastLength.Long).Show();

                            Intent StartLogin = new Intent(context, typeof(Login));
                            StartLogin.PutExtra("BackgroundLoginByteArray", DrawableConverter.DrawableToByteArray(context.Background.Background));
                            context.StartActivity(StartLogin);
						} else {
							Toast.MakeText (context, "Fail!", ToastLength.Long).Show ();
                            Intent StartLogin = new Intent(context, typeof(Login));
                            StartLogin.PutExtra("BackgroundLoginByteArray", DrawableConverter.DrawableToByteArray(context.Background.Background));
                            context.StartActivity(StartLogin);
						}
							
						//}
					};
				}
				break;
			}

			return v;
		}

		public bool VerificareDate(string Nume,string Prenume,string Varsta,string Email,string NumarTelefon,string Parola,string ConfParola,string Chapta,string ChaptaText)
		{
			if (Parola.Length != 0) {
				if (Parola.Equals (ConfParola)) {
					if (NumarTelefon.Length.Equals (10)) {
						if (Email.Contains ("@")) {
							if (Nume != string.Empty) {
								if (Prenume != string.Empty) {
									if (Varsta != string.Empty) {
										if (Chapta.Equals (ChaptaText)) {
											return true;
										} else {
											Toast.MakeText (context, "Codul chapta este introdus gresit,va rugam sa reintroduceti codul!", ToastLength.Long).Show ();
											return false;
										}
									} else {
										Toast.MakeText (context, "Nu ati completat varsta!", ToastLength.Long).Show ();
										return false;
									}
								} else {
									Toast.MakeText (context, "Nu ati completat prenumele!", ToastLength.Long).Show ();
									return false;
								}
							} else {
								Toast.MakeText (context, "Nu ati completat numele!", ToastLength.Long).Show ();
								return false;
							}
						} else {
							Toast.MakeText (context, "Adresa de e-mail este invalida,va rugam sa recompletati careul!", ToastLength.Long).Show ();
							return false;	
						}
					} else {
						Toast.MakeText (context, "Numarul de telefon introdus este invalid,va rugam sa recompletati careul!", ToastLength.Long).Show ();
						return false;
					}
				} else {
					Toast.MakeText (context, "Parolele nu se potrivesc,va rugam sa verificati careele!", ToastLength.Long).Show ();
					return false;
				}
			} else {
				Toast.MakeText (context, "Nu ati completat parola!", ToastLength.Long).Show ();
				return false;
			}
		}
	}
}

