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
using System.Net.Sockets;
using Android.Graphics.Drawables;
using Android.Graphics;
using System.IO;
using SafeTraveling.Scripts.Organizator.Trip;
using Android.Provider;
using System.Threading;
using SafeTraveling.Scripts.Games.XandO;
using SafeTraveling.Scripts.Utilizator.Trip.VoteQuestionPool;

namespace SafeTraveling.Scripts.StartUpService
{
    class NotificationService
    {
        public NotificationService(Context context, Client Client, string nrTel)
        {
            while (true)
            {
                string[] Response = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(Client.ns));

                switch (Response[0])
                {
                    case _Details.UpdateLocation:
                        {
                            if (Response[1] == "1")
                            {
                                Excursionist Excursionist = new Excursionist(context, Response, nrTel);
                                Excursionisti.Add(Excursionist);
                            }
                            else
                            {
                                Excursionist ExcursionistCurent = new Excursionist(context, Response, nrTel);
                                Excursionisti.Add(ExcursionistCurent);

                                Organizator_Trip_VizualizareExcursionisti.LocationUpdated(Excursionisti);

                                foreach (Excursionist ex in Excursionisti)
                                    try
                                    {
                                        if (int.Parse(ex.Distanta) > int.Parse(new SaveUsingSharedPreferences(context).LoadString(SaveUsingSharedPreferences.Tags.Organizator.Distanta)))
                                        {
                                            Intent Apel = new Intent(Intent.ActionView, Android.Net.Uri.Parse("tel:" + ex.NumarTelefon));
                                            PendingIntent PendingApel = PendingIntent.GetActivity(context, 0, Apel, PendingIntentFlags.CancelCurrent);

                                            Notification.Builder nBuilder = new Notification.Builder(context)
                                                .SetSmallIcon(Resource.Drawable.NotificationIcon)
                                                .SetAutoCancel(true)
                                                .SetPriority((int)NotificationPriority.High)
                                                .SetSound(Settings.System.DefaultNotificationUri)
                                                .AddAction(Resource.Drawable.Apel, "Suna", PendingApel)
                                                .SetStyle(new Notification.BigTextStyle().BigText("Utilizatorul " + ex.Nume + " " + ex.Prenume + " a depasit distanta!"))
                                                .SetContentTitle("Safe Traveling");

                                            NotificationManager nManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                                            nManager.Notify(0, nBuilder.Build());
                                        }
                                    }
                                    catch { }

                                Excursionisti = new List<Excursionist>();
                            }
                        } break;
                    case _Details.GameInvitation_Bounce:
                        {
                            string GameId = Response[1];

                            Intent Accept = new Intent(context, typeof(BounceGameActivity));
                            Accept.PutExtra(_Details.Game_IntentType, "JOIN");
                            Accept.PutExtra(_Details.Game_Player_NrTel, nrTel);
                            Accept.PutExtra(_Details.Game_GameId, GameId);

                            PendingIntent AcceptIntent = PendingIntent.GetActivity(context, 0, Accept, PendingIntentFlags.CancelCurrent);

                            Notification.Builder nBuilder = new Notification.Builder(context)
                            .SetSmallIcon(Resource.Drawable.NotificationIcon)
                            .SetAutoCancel(true)
                            .SetPriority((int)NotificationPriority.High)
                            .SetSound(Settings.System.DefaultNotificationUri)
                            .SetStyle(new Notification.BigTextStyle().BigText("Ai fost invitat de catre " + Response[2] + " sa jucati Bounce!"))
                            .SetContentTitle("Safe Traveling")
                            .AddAction(Resource.Drawable.AcceptGame, "Accepta", AcceptIntent)
                            .AddAction(Resource.Drawable.Cancel, "Respinge", AcceptIntent);

                            NotificationManager nManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                            nManager.Notify(0, nBuilder.Build());

                            new Thread(() =>
                            {
                                Thread.Sleep(20000);
                                nManager.Cancel(0);
                            }).Start();
                        } break;
                    case _Details.GameInvitation_XandO:
                        {
                            string GameId = Response[1];

                            Intent Accept = new Intent(context, typeof(XandOActivity));
                            Accept.PutExtra(_Details.Game_IntentType, "JOIN");
                            Accept.PutExtra(_Details.Game_Player_NrTel, nrTel);
                            Accept.PutExtra(_Details.Game_GameId, GameId);

                            PendingIntent AcceptIntent = PendingIntent.GetActivity(context, 0, Accept, PendingIntentFlags.CancelCurrent);

                            Notification.Builder nBuilder = new Notification.Builder(context)
                            .SetSmallIcon(Resource.Drawable.NotificationIcon)
                            .SetAutoCancel(true)
                            .SetPriority((int)NotificationPriority.High)
                            .SetSound(Settings.System.DefaultNotificationUri)
                            .SetStyle(new Notification.BigTextStyle().BigText("Ai fost invitat de catre "+Response[2]+" sa jucati X si O!"))
                            .SetContentTitle("Safe Traveling")
                            .AddAction(Resource.Drawable.AcceptGame, "Accepta", AcceptIntent)
                            .AddAction(Resource.Drawable.Cancel, "Respinge", AcceptIntent);

                            NotificationManager nManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                            nManager.Notify(1, nBuilder.Build());

                            new Thread(() =>
                            {
                                Thread.Sleep(20000);
                                nManager.Cancel(0);
                            }).Start();
                        } break;
                    case _Details.QuestionPoolForVote: {
                        string Intrebare = Response[1];
                        string Variante = Response[2];
                        string Id = Response[3];

                        Intent Accept = new Intent(context, typeof(Utilizator_Trip_VoteQuestionPool));
                        Accept.PutExtra(_Details.QuestionPoolId, Id);
                        Accept.PutExtra(_Details.QuestionPoolIntrebare, Intrebare);
                        Accept.PutExtra(_Details.QuestionPoolVariante, Variante);

                        PendingIntent AcceptIntent = PendingIntent.GetActivity(context, 0, Accept, PendingIntentFlags.CancelCurrent);

                        Notification.Builder nBuilder = new Notification.Builder(context)
                        .SetSmallIcon(Resource.Drawable.Vote)
                        .SetAutoCancel(true)
                        .SetPriority((int)NotificationPriority.High)
                        .SetSound(Settings.System.DefaultNotificationUri)
                        .SetStyle(new Notification.BigTextStyle().BigText(Response[1]))
                        .SetContentTitle("Safe Traveling")
                        .AddAction(Resource.Drawable.Vote, "Voteaza", AcceptIntent);

                        NotificationManager nManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                        nManager.Notify(1, nBuilder.Build());
                    } break;
                }
            }
        }

        List<Excursionist> Excursionisti = new List<Excursionist>();
    }
}