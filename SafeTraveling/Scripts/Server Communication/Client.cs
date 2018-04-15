using System;
using System.Net.Sockets;
using System.Threading;

namespace SafeTraveling
{
	public class Client
	{
		public TcpClient client;
		public NetworkStream ns;

		public Client(TcpClient client,NetworkStream ns)
		{
			this.client = client;
			this.ns = ns;
		}
	}

	public class TripClient
	{
		Client INPUT;
		Client OUTPUT;
		public string NumarTelefon;
		Utilizator_Trip e;

		public TripClient (Client INPUT,Client OUTPUT,string NumarTelefon,Utilizator_Trip e)
		{
			this.INPUT = INPUT;
			this.OUTPUT = OUTPUT;
			this.NumarTelefon = NumarTelefon;
			this.e = e;

			INPUT_GET ();
		}

		private void INPUT_GET()
		{
			new Thread (new ThreadStart (() => {
				while (true)
				{
					string[] Messages = CryptDecryptData.DecryptData(_TcpDataExchange.ReadStreamString(INPUT.ns));
					e.OnClientGetMessage(Messages); 
				}
			})).Start ();
		}

		public void OUTPUT_SEND(string[] Messages)
		{
			_TcpDataExchange.WriteStreamString (OUTPUT.ns,CryptDecryptData.CryptData(Messages));
		}
	}
}