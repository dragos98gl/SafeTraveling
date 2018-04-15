using System;
using System.Text;
using System.Collections.Generic;

namespace SafeTraveling
{
	public class _TcpDataExchange
	{
		public static string ReadStreamString(System.Net.Sockets.NetworkStream ns)
		{
			byte[] bytes = new byte[_Details.BytesCount];
			return Encoding.ASCII.GetString(bytes, 0, ns.Read(bytes, 0, _Details.BytesCount));
		}

		public static void WriteStreamString(System.Net.Sockets.NetworkStream ns, string Message)
		{
			byte[] bytes = AddOffset(Message,1379);

			try
			{
				ns.Write(bytes, 0, bytes.Length);
			}
			catch { }
		}

		private static byte[] AddOffset(string PackString, int maxLength)
		{
			List<byte> FullPack = new List<byte>();
			byte[] PackBytes = Encoding.ASCII.GetBytes(PackString);
			byte[] OffsetBytes = Encoding.ASCII.GetBytes(new string(' ', maxLength - PackBytes.Length));
			FullPack.AddRange(PackBytes);
			FullPack.AddRange(OffsetBytes);

			return FullPack.ToArray();
		}
	}
}