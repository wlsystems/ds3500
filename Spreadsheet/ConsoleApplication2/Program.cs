﻿using Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            string str = "x + x + x";
            //Formula f = new Formula(str);
            Formula f1 = new Formula(str, p.Lookup2, p.Lookup3);
            Console.WriteLine(f1.ToString());
        }
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "X": return 4.0;
                case "Y": return 6.0;
                case "Z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }
        public string Lookup2(String v)
        {
            switch (v)
            {
                case "x": return "X";
                case "y": return "Y";
                case "z": return "Z";
                default: throw new UndefinedVariableException(v);
            }
        }

        public bool Lookup3(String v)
        {
            if (v.Equals("X"))
                return true;
            else return false;
        }
    }
}
