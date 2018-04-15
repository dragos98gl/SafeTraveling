using System;
using System.Drawing;
using Android.Graphics;
using System.Collections.Generic;

namespace SafeTraveling
{
	public class DisplayChapta
	{
		private Bitmap b;
		private int CharNr;
		private string Text = string.Empty;

		public DisplayChapta (int CharNr)
		{
			this.CharNr = CharNr;
		}

		public void Create()
		{
			List <Bitmap> Caractere = new List<Bitmap> ();

			for (int i = 0; i < CharNr; i++) {
				Caracter c = new Caracter (GetRandChar(),GetRandSize(),GetRandColor(),GetRandAngle());
				Caractere.Add (c.GetBitmap());
			}

			b = Bitmap.CreateBitmap (Caracter.BitmapX*CharNr,Caracter.BitmapY,Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas (b);

			for (int i = 0; i < CharNr; i++) {
				
				canvas.DrawBitmap (Caractere[i], null, new Rect (70*i, 0, 70*(i+1), 70), null);
			}
		}

		public Bitmap GetBitmap()
		{
			return b;
		}

		public string GetText()
		{
			return Text;
		}

		private string[] Litere = new string[] { 
			"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
			"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
			"1","2","3","4","5","6","7","8","9","0"
		};

		private int[] Dimensiuni = new int[] {
			50,55,60,65,70
		};

		private Android.Graphics.Color[] Culori = new Android.Graphics.Color[] {
			Android.Graphics.Color.Black,Android.Graphics.Color.Red,Android.Graphics.Color.Yellow,
			Android.Graphics.Color.Blue,Android.Graphics.Color.Green,Android.Graphics.Color.Orange
		};

		private int[] Unghiuri = new int[] {
			-5,-4,-3,-2,-1,1,2,3,4,5,6,7,8,9,10,
			11,12,13,14,15,16,17,18,19,20
		};

		private string GetRandChar ()
		{
			int RandId = new Random ().Next (0, Litere.Length);
			Text = Text + Litere [RandId];

			return Litere [RandId];
		}

		private int GetRandSize()
		{
			int RandId = new Random ().Next (0,Dimensiuni.Length);
			return Dimensiuni [RandId];
		}

		private Android.Graphics.Color GetRandColor()
		{
			int RandId = new Random ().Next (0,Culori.Length);
			return Culori [RandId];
		}

		private int GetRandAngle()
		{
			int RandId = new Random ().Next (0,Unghiuri.Length);
			return Unghiuri[RandId];
		}
	}

	public class Caracter
	{
		string Char;
		int Size;
		Android.Graphics.Color Color;
		int Angle;

		Bitmap CharImg;
		Rect DimensiuniCaracter;

		public const int BitmapX=70;
		public const int BitmapY=70;

		public Caracter(string Char,int Size,Android.Graphics.Color Color,int Angle)
		{
			this.Char = Char;
			this.Size = Size;
			this.Color = Color;
			this.Angle = Angle;

			CreateBitmap (ref CharImg);
		}

		private void CreateBitmap (ref Bitmap b)
		{
			Paint p = new Paint ();
			p.TextSize = Size;
			p.Color = Color;
			p.SetStyle (Paint.Style.Fill);

			DimensiuniCaracter = new Rect ();
			p.GetTextBounds (Char,0,1,DimensiuniCaracter);

			b = Bitmap.CreateBitmap (BitmapX,BitmapY,Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas (b);

			canvas.Rotate (Angle);
			canvas.DrawText (Char,20,DimensiuniCaracter.Height(),p);
		}

		public Bitmap GetBitmap()
		{
			return CharImg;
		}
	}
}

