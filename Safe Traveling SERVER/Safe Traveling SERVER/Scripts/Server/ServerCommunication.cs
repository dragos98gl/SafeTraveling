using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server
{
    class _Utils
    {
        const int BytesCount=1379;

        public static string ReadStreamString(System.Net.Sockets.NetworkStream ns)
        {
            string resoult = string.Empty;
            try
            {
                byte[] bytes = new byte[BytesCount];
                resoult = Encoding.ASCII.GetString(bytes, 0, ns.Read(bytes, 0, BytesCount));
            }
            catch { }

            return resoult;
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
