using Dependencies;
using Formulas;
using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Console.WriteLine("d");
            const string pat = @"[A-Z]+[1-9]+$";
            Regex r = new Regex(pat);
            Match m = r.Match("A1");
            Console.WriteLine(m.Success);
            ss.ToString();
            Formula f = new Formula("B1+A2");
            HashSet<string> hs = new HashSet<string>();
            IEnumerable<string> ib = ss.SetCellContents("A1", f);
            IEnumerator<string> iet = ib.GetEnumerator();
            iet.MoveNext();
            Console.WriteLine(iet.Current);
            iet.MoveNext();
            Console.WriteLine(iet.Current);
        }
    }

}
