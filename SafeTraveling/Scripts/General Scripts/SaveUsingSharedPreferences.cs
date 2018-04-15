using System;
using Android.Content;
using Android.App;
using System.Collections.Generic;
using Android.Preferences;

namespace SafeTraveling
{
	public class SaveUsingSharedPreferences
	{
		Context AppContext;

		public SaveUsingSharedPreferences (Activity context)
		{
			this.AppContext = context.ApplicationContext;
		}

		public SaveUsingSharedPreferences (Context context)
		{
			AppContext = context.ApplicationContext;
		}

		public void Save(string Tag,string Value)
		{
			ISharedPreferencesEditor editor = PreferenceManager.GetDefaultSharedPreferences (AppContext).Edit ();
			editor.PutString (Tag , Value);
			editor.Apply();
		}

		public string LoadString(string Tag)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences (AppContext);
			return prefs.GetString (Tag,string.Empty);
		}

		public void ClearAll()
		{
		}

		public void Clear(string Tag)
		{
			ISharedPreferencesEditor editor = PreferenceManager.GetDefaultSharedPreferences (AppContext).Edit ();
			editor.Remove (Tag);
			editor.Apply();
		}

		public class Tags
		{
			public static class SetUp
			{
				public const string Nume = "nume";
				public const string Prenume = "prenume";
				public const string Varsta = "varsta";
				public const string Sex= "sex";
				public const string Email= "email";
				public const string NumarTelefon = "nrtel";
				public const string TipCont = "tipcont";
			}

			public static class NewTrip
			{
				public const string DestinatiePrincipala = "destinatieprincipala";
				public const string TipulParticipantilor = "tipulparticipantilor";
				public const string DataPlecare = "dataplecare";
				public const string DataIntoarcere = "dataintoarcere";
				public const string LocatiePlecare = "locatieplecare";
				public const string OraPlecare = "oraplecare";
				public const string Observatii = "observatii";
			}

			public static class Login
			{
				public const string Username = "username";
				public const string Password = "password";
			}

			public static class Trip
			{
				public const string TipId = "tripid";
			}

            public static class Organizator
            {
                public const string Distanta = "Distanta";
            }
		}
	}
}

