using System;
using Android.App;
using Android.Widget;
using System.Threading;
using Android.Graphics;
using Android.Content.Res;
using Java.IO;
using Android.Graphics.Drawables;
using Android.Content;
using Android.OS;
using System.Net.Sockets;
using System.Collections.Generic;
using Xamarin.Auth;
using Android.Views;
using System.IO;
using Newtonsoft.Json.Linq;
using Android.Support.V4.App;
using System.Net;
using Android.Media;

namespace SafeTraveling
{
	[Activity(ScreenOrientation=Android.Content.PM.ScreenOrientation.Landscape,NoHistory=true)]
	public class Login: FragmentActivity
	{
		EditText LoginText; 
		TextView ParolaText;
		RelativeLayout Backgroud;
		Button LoginBtn;
		Button SignUpBtn;
		CheckBox RememberMe;
        ImageView LoginWithFacebook;
        int BackID;

		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			RequestWindowFeature (Android.Views.WindowFeatures.NoTitle);
			Window.SetFlags (Android.Views.WindowManagerFlags.Fullscreen,Android.Views.WindowManagerFlags.Fullscreen);

			Bundle Extras = Intent.Extras;
            string[] DateUtilizatorNou = Extras.GetStringArray("FacebookUserData");
			BackID = Extras.GetInt ("BackgroundLoginByteArray");

			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Login);

			LoginText = FindViewById<EditText> (Resource.Id.editText1);
			ParolaText = FindViewById<EditText> (Resource.Id.editText2);
			LoginBtn = FindViewById<Button> (Resource.Id.button1);
			SignUpBtn = FindViewById<Button> (Resource.Id.button2);
			RememberMe = FindViewById<CheckBox> (Resource.Id.checkBox1);
			Backgroud = FindViewById<RelativeLayout> (Resource.Id.relativeLayout1);
            LoginWithFacebook = FindViewById<ImageView>(Resource.Id.imageView3);

			SetTypeface.Italic.SetTypeFace (this,LoginText);
			SetTypeface.Italic.SetTypeFace (this,ParolaText);
			SetTypeface.Italic.SetTypeFace (this,LoginBtn);
			SetTypeface.Italic.SetTypeFace (this,SignUpBtn);
			SetTypeface.Italic.SetTypeFace (this,RememberMe);

            if (DateUtilizatorNou != null)
                CreateNewFacebookUser(DateUtilizatorNou);

            List<Drawable> DrawableList = new List<Drawable>();
            for (int i = 1; i < 5; i++)
                DrawableList.Add(DrawableConverter.GetDrawableFromAssets("LB/img" + i.ToString() + ".jpg", this));
            for (int i = 1; i < 8; i++)
                DrawableList.Add(DrawableConverter.GetDrawableFromAssets("LoadingBackgrounds/img" + i.ToString() + ".jpg", this));
            Backgroud.Background = DrawableList[BackID];

			SignUpBtn.Click+= SignUpBtn_Click;
			LoginBtn.Click+= LoginBtn_Click;
            LoginWithFacebook.Click += LoginWithFacebook_Click;

			if (new SaveUsingSharedPreferences (this).LoadString (SaveUsingSharedPreferences.Tags.Login.Username)!=string.Empty) {
				Intent StartSignUp = new Intent (this, typeof(Utilizator_Main));

				StartSignUp.PutExtra ("BackgroundUtilizatorMainByteArray", DrawableConverter.DrawableToByteArray (Backgroud.Background));
				StartActivity (StartSignUp);
			}
		}

        void LoginWithFacebook_Click(object sender, EventArgs e)
        {
            OAuth2Authenticator auth = new OAuth2Authenticator(_Details.FaceBookAppId, "user_friends+user_status+public_profile+user_friends+email+user_about_me+user_actions.books+user_actions.fitness+user_actions.music+user_actions.news+user_actions.video+user_birthday+user_education_history+user_events+user_games_activity+user_hometown+user_likes+user_location+user_managed_groups+user_photos+user_posts+user_relationships+user_relationship_details+user_religion_politics+user_tagged_places+user_videos+user_website+user_work_history+read_custom_friendlists+read_insights+read_audience_network_insights+read_page_mailboxes+manage_pages+publish_pages+publish_actions+rsvp_event+pages_show_list", new Uri("https://m.facebook.com/dialog/oauth/"), new Uri("http://www.facebook.com/connect/login_success.html"));
            this.StartActivity(auth.GetUI(this));

            auth.Completed += auth_Completed;
        }

        void auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            OAuth2Request Query = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=gender,email,birthday,first_name,last_name,picture.type(large)"),null,e.Account);
            try
            {
                Query.GetResponseAsync().ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        System.Console.WriteLine("Error: " + t.Exception.InnerException.Message);
                    else
                    {
                        JObject Response = JObject.Parse(t.Result.GetResponseText());

                        string[] DateUtilizatorNou = new string[] { 
                        Response["first_name"].ToString(),
                        Response["last_name"].ToString(),
                        Response["gender"].ToString(),
                        Response["birthday"].ToString(),
                        Response["email"].ToString(),   
                        Response["picture"]["data"]["url"].ToString()
                    };
                        
                        Intent StartLogin = new Intent(this, typeof(Login));
                        StartLogin.PutExtra("FacebookUserData", DateUtilizatorNou);
                        StartLogin.PutExtra("BackgroundLoginByteArray", DrawableConverter.DrawableToByteArray(Backgroud.Background));
                        StartActivity(StartLogin);
                    }
                });
            }
            catch {
                Intent StartLogin = new Intent(this, typeof(Login));
                StartLogin.PutExtra("BackgroundLoginByteArray", DrawableConverter.DrawableToByteArray(Backgroud.Background));
                StartActivity(StartLogin);
            }
        }

		void LoginBtn_Click (object sender, EventArgs e)
		{
			string Nume = LoginText.Text;
			string Parola = ParolaText.Text;

			TcpClient Client = new TcpClient (_Details.ServerIP,_Details.LoginSignUpNewTripPort);
			NetworkStream ns = Client.GetStream ();

			_TcpDataExchange.WriteStreamString (ns,CryptDecryptData.CryptData(new string[] {"LOGIN",Nume,Parola }));
			string[] Response = CryptDecryptData.DecryptData (_TcpDataExchange.ReadStreamString (ns));

			switch (Response[0]) {
			case "Utilizator":
				{
					if (RememberMe.Checked) {
                        SaveUsingSharedPreferences Save = new SaveUsingSharedPreferences(this);

                        Save.Save(SaveUsingSharedPreferences.Tags.Login.Username, Nume);
                        Save.Save(SaveUsingSharedPreferences.Tags.Login.Password, Parola);

						Intent StartSignUp = new Intent (this, typeof(Utilizator_Main));

                        StartSignUp.PutExtra("BackgroundUtilizatorMainByteArray", BackID);
                        StartActivity(StartSignUp);
					}
					else {
					Intent StartSignUp = new Intent (this, typeof(Utilizator_Main));

                    StartSignUp.PutExtra("BackgroundUtilizatorMainByteArray", BackID);
					StartActivity (StartSignUp);
					}
				}
				break;
			case "Organizator":
				{
                    if (Response[1]=="1")
                    {
                        SaveUsingSharedPreferences Save = new SaveUsingSharedPreferences(this);

                        Save.Save(SaveUsingSharedPreferences.Tags.Login.Username, Nume);
                        Save.Save(SaveUsingSharedPreferences.Tags.Login.Password, Parola);

                        Intent StartOrganizatorTrip = new Intent(this, typeof(Organizator_Trip));
                        StartOrganizatorTrip.PutExtra("BackgroundByteArray", DrawableConverter.DrawableToByteArray(Backgroud.Background));
                        StartActivity(StartOrganizatorTrip);
                    }
                    else {
                        SaveUsingSharedPreferences Save = new SaveUsingSharedPreferences(this);

                        Save.Save(SaveUsingSharedPreferences.Tags.Login.Username, Nume);
                        Save.Save(SaveUsingSharedPreferences.Tags.Login.Password, Parola);

                        Intent StartOrganizatorMain = new Intent(this, typeof(Organizator_Main));

                        StartOrganizatorMain.PutExtra("BackgroundUtilizatorMainByteArray", DrawableConverter.DrawableToByteArray(Backgroud.Background));
                        StartActivity(StartOrganizatorMain);
                    }
                }
				break;
			default:
				{
					Toast.MakeText(this,"Ne pare rau,numele utilizatorului sau parola au fost introduse gresit!",ToastLength.Long).Show();
				}
				break;
			}
		}

        void CreateNewFacebookUser(string[] DateUtilizatorNou)
        {
            ParolaText.ClearFocus();

            Login_SignUpUsingFacebook_DialogFragment diag = new Login_SignUpUsingFacebook_DialogFragment(DateUtilizatorNou);
            diag.Show(SupportFragmentManager, "frag1");
        }

		void SignUpBtn_Click (object sender, EventArgs e)
		{
			Intent StartSignUp = new Intent(this,typeof(SignUp));

			StartSignUp.PutExtra ("BackgroundSignUpByteArray",BackID);
			StartActivity (StartSignUp);
		}

		public override void OnBackPressed ()
		{
			Android.OS.Process.KillProcess (Android.OS.Process.MyPid());
		}
	}
}

