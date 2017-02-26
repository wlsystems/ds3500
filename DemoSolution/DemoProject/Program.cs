using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex r = new Regex("[a-z]");
            Regex r1 = new Regex("[a-z]");
            Console.WriteLine(r.ToString());
        }
    }
    /// <summary>
    /// test
    /// </summary>
    class Another
    {
        static void h()
        {
        }
    }
}
