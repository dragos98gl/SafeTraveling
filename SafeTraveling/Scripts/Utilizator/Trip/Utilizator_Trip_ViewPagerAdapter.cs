using System;
using Android.Support.V4.App;
using Android.Views;
using Android.Provider;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using System.Collections;
using Java.Util;
using Android.Graphics.Drawables;

namespace SafeTraveling
{
	public class Utilizator_Trip_ViewPagerAdapter:FragmentStatePagerAdapter
	{
		Activity context;
		int LayoutsCount = 3;

		public Utilizator_Trip_ViewPagerAdapter(Android.Support.V4.App.FragmentManager fm,Activity context):base(fm)
		{
			this.context = context;
		}

		public override int Count {
			get {
				return LayoutsCount;
			}
		}

		public override Android.Support.V4.App.Fragment GetItem (int position)
		{
			return new Utilizator_Trip_ViewPagerFragment (position,context);
		}

	}

	public class Utilizator_Trip_ViewPagerFragment: Android.Support.V4.App.Fragment
	{
		int position;
		Activity context;

		int[] Layouturi = new int[] {
			Resource.Layout.Utilizator_Trip_ChatView,
			Resource.Layout.Utilizator_Trip_MaskB,
			Resource.Layout.Utilizator_Trip_MaskC
		};

		public override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public Utilizator_Trip_ViewPagerFragment(int position,Activity context)
		{
			this.position = position;	
			this.context = context;
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Layouturi[position],null,true);

			switch (position) {
			case 0:
				{
					Utilizator_Trip e = (Utilizator_Trip)context; 
                    
                    ListView ChatView = v.FindViewById<ListView> (Resource.Id.listView1);
					Button Send = v.FindViewById<Button> (Resource.Id.button1);
					EditText MessageToSend = v.FindViewById<EditText>(Resource.Id.editText1);

					Send.Click+= (object sender, EventArgs ev) => {
						if (MessageToSend.Text != string.Empty)
						{
							Utilizator_Trip.Me.OUTPUT_SEND (new String [] {"CHAT",Utilizator_Trip.Me.NumarTelefon,MessageToSend.Text.ToString()});
							MessageToSend.Text = string.Empty;
						}
					};

					e.ClientGetMessage += (object sender, ClientGetMessageEventArgs ev) => {
						if (ev.Messages [0] == "CHAT") {
							List<Utilizator_Trip_Chat_Message> MesajeActuale = new List<Utilizator_Trip_Chat_Message> ();
							
                            if (ChatView.Adapter!=null)
                                MesajeActuale.AddRange (((Utilizator_Trip_ChatViewAdapter)ChatView.Adapter).Mesaje);

                            if (ev.Messages[1].Equals(Utilizator_Trip.Me.NumarTelefon))
                            {
                                Drawable Photo = context.Resources.GetDrawable(Resource.Drawable.Icon);
                                if (e.RightDrawerListView.Adapter != null)
                                    foreach (OnlineClient oc in ((Utilizator_Trip_RightDrawerAdapter)e.RightDrawerListView.Adapter).Clients)
                                        if (oc.NumarTelefon == ev.Messages[1])
                                        {
                                            Photo = oc.Photo;
                                            break;
                                        }

                                MesajeActuale.Add(new Utilizator_Trip_Chat_Message("Me", Photo, ev.Messages[2]));
                            }
                            else
                            {
                                Drawable Photo = context.Resources.GetDrawable(Resource.Drawable.Icon);
                                if (e.RightDrawerListView.Adapter != null)
                                    foreach (OnlineClient oc in ((Utilizator_Trip_RightDrawerAdapter)e.RightDrawerListView.Adapter).Clients)
                                        if (oc.NumarTelefon == ev.Messages[1])
                                        {
                                            Photo = oc.Photo;
                                            break;
                                        }

                                MesajeActuale.Add(new Utilizator_Trip_Chat_Message("User", Photo, ev.Messages[2]));
                            }

							context.RunOnUiThread (() => {
								ChatView.Adapter = new Utilizator_Trip_ChatViewAdapter (context, MesajeActuale.ToArray ());
								ChatView.SetSelection (ChatView.Count - 1);
							});
						}
					};
				}
				break;
			case 1:
				{

				}
				break;
			}

			return v;
		}
	}
}

