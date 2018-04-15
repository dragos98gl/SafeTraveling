using Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.TripServices.QuestionPool
{
    class QuestionPool : IDisposable
    {
        public string Id;
        public int[] NumarVoturi;
        public string[] Variante;
        public string Intrebare;

        Excursie e;
        string NrTel;

        Thread WaitForVotes;

        public QuestionPool(Excursie e, string[] Data)
        {
            string Durata = Data[5];
            this.e = e;

            Id=Data[6];
            NrTel = Data[1];
            Intrebare = Data[3];
            Variante = Data[4].Split(',');
            NumarVoturi = new int[Variante.Length];

            foreach (NotificableClient nc in e.NotificableClients)
               // if (nc.NumarTelefon != NrTel)
                    nc.Send(new string[] { _Details.QuestionPoolForVote, Intrebare,string.Join(",",Variante),Id});

            WaitForVotes = new Thread(() => {
                Thread.Sleep(int.Parse(Durata)*1000);
                Dispose();
            });
            WaitForVotes.IsBackground = true;
            WaitForVotes.Start();
        }

        public string GetPercentageOf(int index)
        {
            int sum = NumarVoturi.Sum();
            int val = NumarVoturi[index];

            sum = sum.Equals(0) ? 1 : sum;

            int percentage = (val / sum) * 100;

            return percentage.ToString()+"%";
        }

        public void Dispose()
        {
            WaitForVotes.Abort();
            e.QuestionPools.Remove(this);
        }

        public void Vote(int index)
        {
            NumarVoturi[index]++;
        }
    }
}