using System;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Android.App;
using Android.Graphics.Drawables;
using Android.Content;
using System.Net.Sockets;

namespace SafeTraveling
{
	public class Organizator_CreareExcursie_ViewPagerAdapter:FragmentStatePagerAdapter
	{
		int LayoutsCount = 4;
		Activity context;
		Drawable Background;

		public Organizator_CreareExcursie_ViewPagerAdapter (Android.Support.V4.App.FragmentManager fm,Activity context,Drawable Background):base (fm)
		{
			this.context = context;
			this.Background = Background;
		}

		public override int Count {
			get {
				return LayoutsCount;
			}
		}

		public override Android.Support.V4.App.Fragment GetItem (int position)
		{
			return new Organizator_CreareExcursie_ViewPagerFragmer (position,context,Background);
		}
	}

	public class Organizator_CreareExcursie_ViewPagerFragmer:Android.Support.V4.App.Fragment
	{
		int[] Layouturi = new int[] {
			Resource.Layout.Organizator_CreareExcursie_Page1,
			Resource.Layout.Organizator_CreareExcursie_Page2,
			Resource.Layout.Organizator_CreareExcursie_Page3,
			Resource.Layout.Organizator_CreareExcursie_Page4
		};

		int position;
		Activity context;
		Drawable Background; 

		public override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public Organizator_CreareExcursie_ViewPagerFragmer(int position,Activity context,Drawable Background)
		{
			this.position = position;
			this.context = context;
			this.Background = Background;
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Layouturi[position],null,true);
			SaveUsingSharedPreferences Save = new SaveUsingSharedPreferences (context);

			switch (position) {
			case 0:
				{
					TextView TViewDestinatiePrincipala = v.FindViewById <TextView> (Resource.Id.textView1);
					TextView TViewTipulParticipantilor = v.FindViewById<TextView> (Resource.Id.textView4);
					EditText DestinatiePrincipala = v.FindViewById<EditText> (Resource.Id.editText1);
					RadioButton Elevi = v.FindViewById<RadioButton> (Resource.Id.radioButton1);
					RadioButton PersoanePrivate = v.FindViewById<RadioButton> (Resource.Id.radioButton3);

					SetTypeface.Normal.SetTypeFace (context, TViewDestinatiePrincipala);
					SetTypeface.Normal.SetTypeFace (context, TViewTipulParticipantilor);
					SetTypeface.Normal.SetTypeFace (context, DestinatiePrincipala);
					SetTypeface.Normal.SetTypeFace (context, Elevi);
					SetTypeface.Normal.SetTypeFace (context, PersoanePrivate);

					DestinatiePrincipala.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
						Save.Save (SaveUsingSharedPreferences.Tags.NewTrip.DestinatiePrincipala, DestinatiePrincipala.Text);
					};

					Save.Save (SaveUsingSharedPreferences.Tags.NewTrip.TipulParticipantilor, "Elevi");
					Elevi.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => {
						if (e.IsChecked)
							Save.Save (SaveUsingSharedPreferences.Tags.SetUp.Sex, "Elevi");
						else
							Save.Save (SaveUsingSharedPreferences.Tags.SetUp.Sex, "Persoane private");
					};
				}
				break;
			case 1:
				{
					TextView PerioadaExcursieiTView = v.FindViewById<TextView> (Resource.Id.textView1);
					TextView DataPlecareTView = v.FindViewById<TextView> (Resource.Id.textView2);
					TextView DataIntoarcereTView = v.FindViewById <TextView> (Resource.Id.textView3);
					DatePicker DataPlecare = v.FindViewById<DatePicker> (Resource.Id.datePicker1);
					DatePicker DataIntoarcere = v.FindViewById<DatePicker> (Resource.Id.datePicker2);

					SetTypeface.Normal.SetTypeFace (context, PerioadaExcursieiTView);
					SetTypeface.Normal.SetTypeFace (context, DataPlecareTView);
					SetTypeface.Normal.SetTypeFace (context, DataIntoarcereTView);

					Save.Save (SaveUsingSharedPreferences.Tags.NewTrip.DataPlecare, System.DateTime.Now.Day.ToString () + "." + System.DateTime.Now.Month.ToString () + "." + System.DateTime.Now.Year.ToString ());
					DataPlecare.CalendarView.DateChange += (object sender, CalendarView.DateChangeEventArgs e) => {
						Save.Save (SaveUsingSharedPreferences.Tags.NewTrip.DataPlecare, e.DayOfMonth.ToString () + "." + e.Month.ToString () + "." + e.Year.ToString ());
					};

					Save.Save (SaveUsingSharedPreferences.Tags.NewTrip.DataIntoarcere, System.DateTime.Now.Day.ToString () + "." + System.DateTime.Now.Month.ToString () + "." + System.DateTime.Now.Year.ToString ());
					DataIntoarcere.CalendarView.DateChange += (object sender, CalendarView.DateChangeEventArgs e) => {
						Save.Save (SaveUsingSharedPreferences.Tags.NewTrip.DataIntoarcere, e.DayOfMonth.ToString () + "." + e.Month.ToString () + "." + e.Year.ToString ());
					};
				}
				break;
			case 2:
				{
					TextView LocatiePlecareTView = v.FindViewById<TextView> (Resource.Id.textView1);
					TextView OraPlecareTview = v.FindViewById<TextView> (Resource.Id.textView2);
					EditText LocatiePlecare = v.FindViewById<EditText> (Resource.Id.editText1);
					TimePicker OraPlecare = v.FindViewById<TimePicker> (Resource.Id.timePicker1);

					OraPlecare.SetIs24HourView (Java.Lang.Boolean.True);

					SetTypeface.Normal.SetTypeFace (context, LocatiePlecareTView);
					SetTypeface.Normal.SetTypeFace (context, OraPlecareTview);
					SetTypeface.Normal.SetTypeFace (context, LocatiePlecare);

					LocatiePlecare.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
						Save.Save (SaveUsingSharedPreferences.Tags.NewTrip.LocatiePlecare, LocatiePlecare.Text);
					};

					Save.Save (SaveUsingSharedPreferences.Tags.NewTrip.OraPlecare, System.DateTime.Now.Minute.ToString () + ":" + System.DateTime.Now.Hour.ToString ());
					OraPlecare.TimeChanged += (object sender, TimePicker.TimeChangedEventArgs e) => {
						Save.Save (SaveUsingSharedPreferences.Tags.NewTrip.OraPlecare, e.Minute.ToString () + ":" + e.HourOfDay.ToString ());
					};
				}
				break;
			case 3:
				{
					Button Finalizare = v.FindViewById<Button> (Resource.Id.button1);
					TextView ObservatiiTView = v.FindViewById <TextView> (Resource.Id.textView1);
					EditText ObservatiiEText = v.FindViewById <EditText> (Resource.Id.editText1);

					SetTypeface.Normal.SetTypeFace (context, Finalizare);
					SetTypeface.Normal.SetTypeFace (context, ObservatiiTView);
					SetTypeface.Normal.SetTypeFace (context, ObservatiiEText);

					Finalizare.Click += (object sender, EventArgs e) => {
						string DestinatiePrincipala = Save.LoadString (SaveUsingSharedPreferences.Tags.NewTrip.DestinatiePrincipala);
						string TipulParticipantilor = Save.LoadString (SaveUsingSharedPreferences.Tags.NewTrip.TipulParticipantilor);
						string DataPlecare = Save.LoadString (SaveUsingSharedPreferences.Tags.NewTrip.DataPlecare);
						string DataIntoarcere = Save.LoadString (SaveUsingSharedPreferences.Tags.NewTrip.DataIntoarcere);
						string LocatiePlecare = Save.LoadString (SaveUsingSharedPreferences.Tags.NewTrip.LocatiePlecare);
						string OraPlecare = Save.LoadString (SaveUsingSharedPreferences.Tags.NewTrip.OraPlecare);
						string Observatii = ObservatiiEText.Text;
						string NumeOrganizator = "numeOrganizator";
                        string NumarTelefon = new SaveUsingSharedPreferences(context).LoadString(SaveUsingSharedPreferences.Tags.Login.Username);

						if (VerificareDate (DestinatiePrincipala, LocatiePlecare)) {
							TcpClient Client = new TcpClient (_Details.ServerIP, _Details.TripPort_INPUT);
							NetworkStream ns = Client.GetStream ();

							_TcpDataExchange.WriteStreamString (ns, CryptDecryptData.CryptData (new string[] {
								"NEWTRIP",
								DestinatiePrincipala,
								TipulParticipantilor,
								DataPlecare,
								DataIntoarcere,
								LocatiePlecare,
								OraPlecare,
								Observatii,
								NumeOrganizator,
								NumarTelefon
							}));

							if (CryptDecryptData.DecryptData (_TcpDataExchange.ReadStreamString (ns)) [0] == "1") {
								Toast.MakeText (context, "Ala-i bosssss!", ToastLength.Long).Show ();

                                TcpClient client = new TcpClient(_Details.ServerIP, _Details.LargeFilesPort);
                                NetworkStream Ns = client.GetStream();

                                _TcpDataExchange.WriteStreamString(Ns, CryptDecryptData.CryptData(new string[] { _Details.GetTripId, new SaveUsingSharedPreferences(context).LoadString(SaveUsingSharedPreferences.Tags.Login.Username) }));
                                string TripId = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(Ns))[0];
                                new SaveUsingSharedPreferences(context).Save(SaveUsingSharedPreferences.Tags.Trip.TipId, TripId);
                               
                                ns.Dispose();
                                client.Close();

                                Intent StartOrganizatorTrip = new Intent(context, typeof(Organizator_Trip));
                                StartOrganizatorTrip.PutExtra("BackgroundByteArray", DrawableConverter.DrawableToByteArray(Background));
                                context.StartActivity(StartOrganizatorTrip);
							} else {
								Toast.MakeText (context, "Fail!", ToastLength.Long).Show ();
								context.Finish ();
							}
						}
					};
				}
				break;
			}

			return v;
		}

		private bool VerificareDate(string DestinatiePrincipala,string LocatiePlecare)
		{
			if (DestinatiePrincipala != string.Empty) {
				if (LocatiePlecare != string.Empty)
					return true;
				else {
					Toast.MakeText (context, "Nu ati completat locatia plecarii!", ToastLength.Long).Show ();
					return false;
				}
			} else {
				Toast.MakeText (context,"Nu ati completat destinatia principala!",ToastLength.Long).Show ();
				return false;
			}
		}
	}
}

