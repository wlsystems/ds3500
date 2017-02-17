using Formulas;
using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f2 = new Formula("E1+F1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", 1);
            ss.SetCellContents("C1", f1);
            ss.SetCellContents("A1", f2);
            Object ob = ss.GetCellContents("B1");
        }
    }
}
