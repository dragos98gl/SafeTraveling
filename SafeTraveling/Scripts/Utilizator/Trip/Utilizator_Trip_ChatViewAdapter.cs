using System;
using Android.Graphics.Drawables;
using Android.Widget;
using Android.App;
using Android.Views;
using System.Collections.Generic;

namespace SafeTraveling
{
	public class Utilizator_Trip_ChatViewAdapter:ArrayAdapter<Utilizator_Trip_Chat_Message>
	{
		Activity context;
		public Utilizator_Trip_Chat_Message[] Mesaje;

		public Utilizator_Trip_ChatViewAdapter (Activity context,Utilizator_Trip_Chat_Message[] Mesaje):base(context,Resource.Layout.Utilizator_Trip_ChatViewMessagesAdapter,Mesaje)
		{
			this.context = context;
			this.Mesaje = Mesaje;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			LayoutInflater inflater = context.LayoutInflater;
			View v = inflater.Inflate (Resource.Layout.Utilizator_Trip_ChatViewMessagesAdapter,null,true);

			RelativeLayout User = v.FindViewById<RelativeLayout> (Resource.Id.relativeLayout1); 
			RelativeLayout Me = v.FindViewById<RelativeLayout> (Resource.Id.relativeLayout2);

			switch (Mesaje [position].Tip) {
			case "Me":
				{
					User.Visibility = ViewStates.Gone;

					TextView Text = v.FindViewById<TextView> (Resource.Id.textView2);
					ImageView Photo = v.FindViewById<ImageView> (Resource.Id.imageView2);

					Text.Text = Mesaje [position].Mesaj;
					Photo.SetImageDrawable (Mesaje[position].UserPhoto);
				}
				break;
			case "User":
				{
					Me.Visibility = ViewStates.Gone;

					TextView Text = v.FindViewById<TextView> (Resource.Id.textView3);
					ImageView Photo = v.FindViewById<ImageView> (Resource.Id.imageView1);

					Text.Text = Mesaje [position].Mesaj;
					Photo.SetImageDrawable (Mesaje[position].UserPhoto);
				}
				break;
			}

			return v;
		}
	}

	public class Utilizator_Trip_Chat_Message
	{
		public string Tip;
		public Drawable UserPhoto;
		public string Mesaj;

		public Utilizator_Trip_Chat_Message (string Tip,Drawable UserPhoto,string Mesaj)
		{
			this.Tip = Tip;
			this.UserPhoto = UserPhoto;
			this.Mesaj = Mesaj;
		}
	}
}

