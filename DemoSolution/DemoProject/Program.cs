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
        int x=3;
        int y=3;
        static void Main(string[] args)
        {
            int x = 3;
            int y = 5;
            GetSelection(out x, out y);
            Console.Write(x + " " + y);
        }

        public static void GetSelection(out int col, out int row)
        {

            col = 1;
            row = 1;
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
