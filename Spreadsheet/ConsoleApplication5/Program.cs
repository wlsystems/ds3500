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
 
    public static class MyExtensions
    {
        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }

        
    }
    /// <summary>
    /// 
    /// </summary>
    class Program : TextWriter
    {
        static int i;

        public override Encoding Encoding
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        static void Main(string[] args)
        {
            int a = 1;
            int b = 2;
            Console.WriteLine(Method(ref a, ref b));
        }

        public static int Method(ref int d, ref int n)
        {
            d = 2;
            return d + n;
        }
        public static string Method(double d, int n)
        {
            return (d * n).ToString();
        }
        public static Boolean Even(int x, int y)
        {
            return ((x + y) % 2 == 0);
        }
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
