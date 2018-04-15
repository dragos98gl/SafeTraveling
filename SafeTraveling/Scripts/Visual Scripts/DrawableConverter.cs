using System;
using Android.Graphics.Drawables;
using Android.Content.Res;
using Android.App;
using Android.Graphics;
using Java.IO;
using System.IO;
using Android.Content;

namespace SafeTraveling
{
	public static class DrawableConverter
	{
		public static Drawable GetDrawableFromAssets(string FileName,Activity context)
		{
			AssetManager assetManager = context.Assets;
			Drawable d = null;

			try {
				d = Drawable.CreateFromStream (assetManager.Open (FileName),"");
			}
			catch 
			{
				
			}
			return d;
		}

		public static byte[] DrawableToByteArray(Drawable DrawableImage)
		{
			Bitmap b = ((BitmapDrawable)DrawableImage).Bitmap;
			MemoryStream stream = new MemoryStream ();

			b.Compress (Bitmap.CompressFormat.Jpeg,100,stream);

			return stream.ToArray();
		}

        public static Drawable ByteArrayToDrawable(byte[] Bytes, Context context)
        {
            ByteArrayInputStream stream = new ByteArrayInputStream(Bytes, 0, Bytes.Length);
            Bitmap b = BitmapFactory.DecodeByteArray(Bytes, 0, Bytes.Length);

            return new BitmapDrawable(context.Resources, b);
        }
	}
}

