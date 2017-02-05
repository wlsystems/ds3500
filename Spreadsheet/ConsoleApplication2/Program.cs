using Formulas;
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

            Formula f = new Formula("x5 + x6 + x7 + (x8) +");
            Console.WriteLine(f.Evaluate(p.Lookup4));
        }
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }
    }

    
}
