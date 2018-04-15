
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
using Android.Graphics;
using Android.Provider;
using System.IO;

namespace SafeTraveling
{
	[Activity (Label = "TakePhoto")]			
	public class TakePhoto : Activity
	{
		Java.IO.File file;
		byte[] TempBytes;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			CreateFolder ();
			file = new Java.IO.File(GetPath(),string.Format("temp.jpg",Guid.NewGuid()));

			Intent i = new Intent (MediaStore.ActionImageCapture);
			i.PutExtra (MediaStore.ExtraOutput,Android.Net.Uri.FromFile(file));
			StartActivityForResult (i,0);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			if (resultCode.Equals (Result.Ok)) {
				Bitmap b = BitmapFactory.DecodeFile (file.Path);

				MemoryStream ms = new MemoryStream ();
				b.Compress (Bitmap.CompressFormat.Jpeg, 100, ms);

				TempBytes = ms.ToArray ();

				file.Delete ();
				Finish ();
			} else {
				Finish ();
			}
		}

		public override void Finish ()
		{
			Intent sendBack = new Intent ();
			sendBack.PutExtra ("PhBytes", TempBytes);
			SetResult (Result.Ok,sendBack);

			base.Finish ();
		}

		private void CreateFolder ()
		{
			Java.IO.File Folder = new Java.IO.File (GetPath());
			if (!Folder.Exists ()) {
				Folder.Mkdir();
				Folder.DeleteOnExit ();
			}
		}

		private string GetPath()
		{
			//string Path = Android.OS.Environment.ExternalStorageDirectory.Path+"/Android/data/"+ PackageName +"/temp";
			string Path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures) + "/temp";
			return Path;
		}
	}
}

