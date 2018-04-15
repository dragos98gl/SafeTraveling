using System;
using Android.Widget;
using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using SafeTraveling.Scripts.Organizator.Trip;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SafeTraveling
{
	public class Organizator_Trip_GridViewAdapter:ArrayAdapter <Drawable>
	{
		Activity context;
		Drawable[] Icons;

		public Organizator_Trip_GridViewAdapter (Activity context,Drawable[] Icons):base(context,Resource.Layout.Organizator_Trip_GridViewAdapter,Icons)
		{
			this.context = context;
			this.Icons = Icons;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			LayoutInflater inflater = context.LayoutInflater;
			View v = inflater.Inflate (Resource.Layout.Organizator_Trip_GridViewAdapter,null,true);

			ImageView Icon_iView = v.FindViewById <ImageView> (Resource.Id.imageView1);
			Icon_iView.SetImageDrawable (Icons[position]);

            Icon_iView.Click += (object sender, EventArgs e) => {
                switch (position)
                {
                    case 1: {
                        context.StartActivity(typeof(Organizator_Trip_VizualizareExcursionisti));
                    }break;
                    case 3:
                        {
                            Dialog diag = new Dialog(context);
                            diag.Window.RequestFeature(WindowFeatures.NoTitle);

                            View CostumView = inflater.Inflate(Resource.Layout.Utilizator_Trip_TripInfo_AlertDialogAdapter, null);
                            TextView NumeCamp = CostumView.FindViewById<TextView>(Resource.Id.textView1);
                            ImageView EditInfo = CostumView.FindViewById<ImageView>(Resource.Id.imageView1);
                            EditText NewValue = CostumView.FindViewById<EditText>(Resource.Id.editText1);

                            SaveUsingSharedPreferences Save = new SaveUsingSharedPreferences(context);
                            string Distanta = Save.LoadString(SaveUsingSharedPreferences.Tags.Organizator.Distanta);

                            NumeCamp.Text = "Valoarea actuala este:" + Distanta;
                            EditInfo.Click += (object sender1, EventArgs e1) =>
                            {
                                Save.Save(SaveUsingSharedPreferences.Tags.Organizator.Distanta, NewValue.Text);

                                diag.Cancel();
                            };

                            diag.SetContentView(CostumView);

                            diag.SetCanceledOnTouchOutside(true);
                            diag.Show();
                            diag.Window.SetBackgroundDrawable(context.Resources.GetDrawable(Resource.Drawable.background_MarginiOvaleAlb));
                        } break;
                    case 4: {
                        PopupMenu popup = new PopupMenu(context, v);
                        popup.MenuItemClick += (object s, PopupMenu.MenuItemClickEventArgs e1) =>
                        {
                            switch (e1.Item.ToString())
                            {
                                case "Adauga intrebare noua":
                                    {
                                        context.StartActivity(typeof(Organizator_Trip_NewQuestionPool));
                                    } break;
                                case "Vezi progresul intrebarilor actuale":
                                    {
                                        TcpClient Client = new TcpClient(_Details.ServerIP,_Details.LargeFilesPort);
                                        NetworkStream ns = Client.GetStream();

                                        string TripId = new SaveUsingSharedPreferences(context).LoadString(SaveUsingSharedPreferences.Tags.Trip.TipId);
                                        _TcpDataExchange.WriteStreamString(ns, CryptDecryptData.CryptData(new string[] {_Details.RequestVotesQuestionPool,TripId}));

                                        string[] Response = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns));

                                        string[] Intrebari = Response[0].Split(',');
                                        string[] Ids = Response[1].Split(',');

                                        var Test = new Organizator_Trip_ViewQuestionPool_Adapters.IntrebarVariante[Intrebari.Length];

                                        for (int i = 0; i < Intrebari.Length; i++)
                                        {
                                            Response = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(ns));

                                            List<string> Variante = new List<string>();
                                            List<string> Procente = new List<string>();

                                            for (int j = 0; j < Response.Length; j++)
                                            {
                                                if (Response[j] != "")
                                                {
                                                    Variante.Add(Response[j].Split(',')[0]);
                                                    Procente.Add(Response[j].Split(',')[1]);
                                                }
                                                else
                                                    break;
                                            }

                                            var temp = new Organizator_Trip_ViewQuestionPool_Adapters.IntrebarVariante();
                                            temp.Intrebare = Intrebari[i];
                                            temp.Variante = Variante.ToArray();
                                            temp.Procente = Procente.ToArray();

                                            Test[i] = temp;
                                        }

                                        Organizator_Trip_ViewQuestionPool_DialogFragment diag = new Organizator_Trip_ViewQuestionPool_DialogFragment(Test);
                                        diag.Show(context.FragmentManager,"ViewQuestionPool");
                                    } break;
                            }
                        };
                        MenuInflater menuInflater = popup.MenuInflater;
                        menuInflater.Inflate(Resource.Menu.OptiuniQuestionPool, popup.Menu);
                        popup.Show();
                    }break;
                }
            };

			return v;
		}
	}
}

