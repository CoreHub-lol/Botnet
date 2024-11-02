using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace reshellsv
{
    public static class TCS
    {
        public static void Send(TcpClient client, byte[] data)
        {
            client.GetStream().Write(data, 0, data.Length);
        }

        public static void Send(TcpClient client, string data)
        {
            if (data == null) { return; }
            Send(client, Encoding.UTF8.GetBytes(data));
        }

        public static void SendLine(TcpClient client, string data, byte Delimiter)
        {
            if (string.IsNullOrEmpty(data)) { return; }
            if (data.LastOrDefault() != Delimiter)
            {
                Send(client, data + Encoding.UTF8.GetString(new byte[] { Delimiter }));
            }
            else
            {
                Send(client, data);
            }
        }
    }
}
