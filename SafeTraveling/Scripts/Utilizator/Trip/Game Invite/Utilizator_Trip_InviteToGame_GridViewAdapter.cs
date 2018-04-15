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
using Android.Graphics.Drawables;
using SafeTraveling.Scripts.Games.XandO;

namespace SafeTraveling.Scripts.Utilizator.Trip.Game_Invite
{
    class Utilizator_Trip_InviteToGame_GridViewAdapter:ArrayAdapter<string>
    {
        Activity context;
        string GamePlayerNrTel;
        string[] Games;
        Drawable[] GamesIcon;

        public Utilizator_Trip_InviteToGame_GridViewAdapter(Activity context, string[] Games, string GamePlayerNrTel)
            : base(context, Resource.Layout.Utilizator_Trip_GameInvite_Adapter, Games)
        {
            this.context = context;
            this.Games = Games;
            this.GamePlayerNrTel = GamePlayerNrTel;

            GamesIcon = new Drawable[] { 
            context.Resources.GetDrawable(Resource.Drawable.X_O_Icon), 
            context.Resources.GetDrawable(Resource.Drawable.BounceGame_Icon)
            };
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = context.LayoutInflater;
            View v = inflater.Inflate(Resource.Layout.Utilizator_Trip_GameInvite_Adapter, null, false);

            RelativeLayout Container = v.FindViewById<RelativeLayout>(Resource.Id.ViewRelLay);
            ImageView GameImage = v.FindViewById<ImageView>(Resource.Id.imageView1);
            TextView GameName = v.FindViewById<TextView>(Resource.Id.textView1);

            GameName.Text = Games[position];
            GameImage.SetBackgroundDrawable(GamesIcon[position]);

            SetTypeface.Normal.SetTypeFace(context, GameName);

            Container.Click += ((object sender, EventArgs e) =>
            {
                switch (position)
                {
                    case 0:
                        {
                            Intent Create = new Intent(context, typeof(XandOActivity));
                            Create.PutExtra(_Details.Game_IntentType, "CREATE");
                            Create.PutExtra(_Details.Game_Player_NrTel, GamePlayerNrTel);

                            context.StartActivity(Create);
                        } break;
                    case 1:
                        {
                            Intent Create = new Intent(context, typeof(BounceGameActivity));
                            Create.PutExtra(_Details.Game_IntentType, "CREATE");
                            Create.PutExtra(_Details.Game_Player_NrTel, GamePlayerNrTel);

                            context.StartActivity(Create);
                        } break;
                }
            });

            return v;
        }
    }
}