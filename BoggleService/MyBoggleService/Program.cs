using Boggle;
using Boggle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyBoggleService
{
    class Program
    {
        static void Main(string[] args)
        {
            BoggleService server = new BoggleService(60000);
            Console.ReadLine();
        }
        
    }
}
