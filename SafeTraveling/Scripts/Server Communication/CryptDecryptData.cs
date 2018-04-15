using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SafeTraveling
{
	class CryptDecryptData
	{
		public static string CryptData(string[] Messages)
		{
			string CryptedData = string.Empty;

			foreach (string Text in Messages)
				CryptedData = CryptedData + ConvertToBase64(Text) + " ";

			return CryptedData;
		}

		public static string[] DecryptData(string CryptedData)
		{
			List<string> Messages = new List<string>();
			string[] CryptedTexts = Regex.Split(CryptedData," ");

			foreach (string Text in CryptedTexts) {
				Messages.Add (ConvertFromBase64 (Text));
			}
			Messages.RemoveAt(Messages.Count-1);
			return Messages.ToArray();
		}

		private static string ConvertFromBase64(string Text)
		{
			byte[] TextBytesArray = Convert.FromBase64String(Text);
			return Encoding.ASCII.GetString(TextBytesArray,0,TextBytesArray.Length);
		}

		private static string ConvertToBase64(string Text)
		{
			byte[] TextByteArray = Encoding.ASCII.GetBytes(Text);
			return Convert.ToBase64String(TextByteArray,0,TextByteArray.Length);
		}
	}
}

