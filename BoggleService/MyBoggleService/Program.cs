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
            //HttpStatusCode status;
            //Player name = new Player { Nickname = "Joe" };
            //BoggleService service = new BoggleService(60000);
            Callie niceGirl = new Callie(60000);
            //Person user = service.Register(name, out status);
            //Console.WriteLine(user.UserToken);
            //Console.WriteLine(status.ToString());
            // This is our way of preventing the main thread from
            // exiting while the server is in use
            Console.ReadLine();
        }
        
    }
}
