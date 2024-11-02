using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullServer
{
    internal class Program
    {
        public static String name = "NullBotNet";
        public static String prefix = "[NullNet] ";
        public static String version = "1.0 BETA";
        static void Main(string[] args)
        {
            new ServerHandler().Start();
        }
    }
}
