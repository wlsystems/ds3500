using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        public static HashSet<string> wordset { get; set; }
        static void Main(string[] args)
        {
            string[] FileLines = File.ReadAllLines("../../dictionary.txt");
            foreach (string s in FileLines)
                wordset.Add("s");
        }
    }
}
