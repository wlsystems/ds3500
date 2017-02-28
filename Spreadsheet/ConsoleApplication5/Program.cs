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
            Func <double,int , string> v;
            int[] n = new int[10];
            n[0] = 1;
            n[1] = 1;
            n[2] = 2;
            Func<int,int,int> func2 = (x, y) => (x + y*0);
            v = Method;
            Console.Write(v(2, 3));
        }
        public static string Method(double d, int n) { return (d * n).ToString(); }

        public static int Choose(int[] numbers, Func<int, int, int> f)
        {
            int result = numbers[0];
            for (int i = 1; i < numbers.Length; i++) {
                result = f(result, numbers[i]);
            }
            return result;
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
