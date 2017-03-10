using Formulas;
using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApplication5
{


    public static class MyExtensions
    {
        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }
        public static bool ContainsDuplicates<T>(this IEnumerable<T> e)
        {
            var set = new HashSet<T>();
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var item in e)
            // ReSharper restore LoopCanBeConvertedToQuery
            {
                if (!set.Add(item))
                    return true;
            }
            return false;
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
        // Accepts all strings
        private const string ALL = "^.*$";

        // Verifies cells and their values, which must alternate.
        public void VV(AbstractSpreadsheet sheet, params object[] constraints)
        {
            for (int i = 0; i < constraints.Length; i += 2)
            {
                if (constraints[i + 1] is double)
                {
                    Console.WriteLine((double)constraints[i + 1] + " " + (double)sheet.GetCellValue((string)constraints[i]));
                }
                else
                {
                    Console.WriteLine(constraints[i + 1] + " " + sheet.GetCellValue((string)constraints[i]));
                }
            }
        }

        // For setting a spreadsheet cell.
        public IEnumerable<string> Set(AbstractSpreadsheet sheet, string name, string contents)
        {
            List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
            return result;
        }


        static void Main(string[] args)           
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", ("=a3"));
            Console.Write(s.GetCellContents("Z7").ToString());
        }
        private String randomName(Random rand)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(rand.Next(26), 1) + (rand.Next(99) + 1);
        }
        private String randomFormula(Random rand)
        {
            String f = randomName(rand);
            for (int i = 0; i < 10; i++)
            {
                switch (rand.Next(4))
                {
                    case 0:
                        f += "+";
                        break;
                    case 1:
                        f += "-";
                        break;
                    case 2:
                        f += "*";
                        break;
                    case 3:
                        f += "/";
                        break;
                }
                switch (rand.Next(2))
                {
                    case 0:
                        f += 7.2;
                        break;
                    case 1:
                        f += randomName(rand);
                        break;
                }
            }
            return f;
        }
        public void Stress5()
        {
            int seed = 47;
            int size = 831;
            AbstractSpreadsheet s = new Spreadsheet();
            Random rand = new Random(seed);
            int j = 0;
            List<string> hst = new List<string>();
            for (int i = 0; i < 1; i++)
            {
               if (!hst.Contains(randomName(rand))){
                    hst.Add(randomName(rand));
                }
                try
                {
                    switch (rand.Next(3))
                    {                                               
                        case 0:
                            s.SetContentsOfCell(randomName(rand), "3.14");
                            break;
                        case 1:
                            s.SetContentsOfCell(randomName(rand), "hello");
                            break;
                        case 2:
                            s.SetContentsOfCell(randomName(rand), "=" + randomFormula(rand));
                            break;
                    }
                }

                catch (CircularException)
                {
                }
            }
            ISet<string> set = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
            Console.WriteLine(hst.Count);


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
