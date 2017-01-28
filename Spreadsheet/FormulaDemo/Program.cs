using Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Formulas
{
    
    class Program
    {
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
        static void Main(string[] args)
        {
            
            Program p = new Formulas.Program();
            Formula f = new Formula("(1+1) + 1");
            Console.Write(f.Evaluate(p.Lookup4));

        }
        
    }
    
}