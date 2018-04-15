using System;
using Android.Widget;
using Android.Graphics;
using Android.App;

namespace SafeTraveling
{
	public static class SetTypeface
	{
		public static class Italic
		{		
			public static void SetTypeFace (Activity context,TextView obj)
			{
				Typeface tf = Typeface.CreateFromAsset(context.Assets,"Font/Font_Normal.otf");
				obj.SetTypeface (tf,TypefaceStyle.Italic);
			}

			public static void SetTypeFace (Activity context,Button obj)
			{
				Typeface tf = Typeface.CreateFromAsset(context.Assets,"Font/Font_Normal.otf");
				obj.SetTypeface (tf,TypefaceStyle.Italic);
			}

			public static void SetTypeFace (Activity context,CheckBox obj)
			{
				Typeface tf = Typeface.CreateFromAsset(context.Assets,"Font/Font_Normal.otf");
				obj.SetTypeface (tf,TypefaceStyle.Italic);
			}

		}

		public static class Normal
		{
			public static void SetTypeFace (Activity context,TextView obj)
			{
				Typeface tf = Typeface.CreateFromAsset(context.Assets,"Font/Font_Normal.otf");
				obj.SetTypeface (tf,TypefaceStyle.Normal);
			}

			public static void SetTypeFace (Activity context,Button obj)
			{
				Typeface tf = Typeface.CreateFromAsset(context.Assets,"Font/Font_Normal.otf");
				obj.SetTypeface (tf,TypefaceStyle.Normal);
			}

			public static void SetTypeFace (Activity context,CheckBox obj)
			{
				Typeface tf = Typeface.CreateFromAsset(context.Assets,"Font/Font_Normal.otf");
				obj.SetTypeface (tf,TypefaceStyle.Normal);
			}
		}

		public static class Bold
		{
			public static void SetTypeFace (Activity context,TextView obj)
			{
				Typeface tf = Typeface.CreateFromAsset(context.Assets,"Font/Font_Normal.otf");
				obj.SetTypeface (tf,TypefaceStyle.Bold);
			}

			public static void SetTypeFace (Activity context,Button obj)
			{
				Typeface tf = Typeface.CreateFromAsset(context.Assets,"Font/Font_Normal.otf");
				obj.SetTypeface (tf,TypefaceStyle.Bold);
			}

			public static void SetTypeFace (Activity context,CheckBox obj)
			{
				Typeface tf = Typeface.CreateFromAsset(context.Assets,"Font/Font_Normal.otf");
				obj.SetTypeface (tf,TypefaceStyle.Bold);
			}
		}
	}
}

