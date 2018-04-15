using System;
using Android.Widget;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Util;

namespace SafeTraveling
{
	public class RoundedBitmap
	{
		public static Bitmap MakeRound (Bitmap bmp,int radius)
		{
			Bitmap sbmp;

			if (bmp.Width != radius || bmp.Height != radius) {
				float smallest = Math.Min (bmp.Width, bmp.Height);
				float factor = smallest / radius;
				sbmp = Bitmap.CreateScaledBitmap (bmp, (int)(bmp.Width / factor), (int)(bmp.Height / factor), false);
			} else
				sbmp = bmp;

			Bitmap output = Bitmap.CreateBitmap (radius,radius,Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas (output);

			Paint paint = new Paint ();
			Rect rect = new Rect (0,0,radius,radius);

			paint.AntiAlias = true;
			paint.FilterBitmap = true;
			paint.Dither = true;
			canvas.DrawARGB (0,0,0,0);
			paint.Color = Color.ParseColor ("#BAB399");
			canvas.DrawCircle (
				radius/2 + 0.7f,
				radius/2 + 0.7f,
				radius/2 + 0.1f,
				paint);
			paint.SetXfermode (new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
			canvas.DrawBitmap (sbmp,rect,rect,paint);

			return output;
		}
	}
}

