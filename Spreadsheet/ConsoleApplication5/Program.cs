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
    class Program
    {
        static void Main(string[] args)
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", ("=A2"));
            s.SetContentsOfCell("A2", ("=A11"));
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
