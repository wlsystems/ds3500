using Formulas;
using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ConsoleApplication5
{
    /// <summary>
    /// 
    /// </summary>
    class Program : TextWriter
    {
        public override Encoding Encoding
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        static void Main(string[] args)
        {
            
            try
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*");
                TextReader t = new StreamReader("ss.xml");
                AbstractSpreadsheet s = new Spreadsheet(t, rg1);
                IEnumerable<string> ieb = (s.GetNamesOfAllNonemptyCells());
                foreach (string st in ieb)
                    Console.WriteLine(st);
            }
            catch(Exception e)
            {
                Console.WriteLine((e.GetType().ToString()));
            }          
        }

        static string replace(string s){
            return s;
        }

        public double Lookup4(String v)
        {
            switch (v)
            {
                case "A1": return 4.0;
                case "C1": return 6.0;
                case "Z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        public bool Lookup2(String v)
        {
                return true;
        }
        public string Lookup3(String v)
        {
            return v;
        }

    }
}
