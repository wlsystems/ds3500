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
            AbstractSpreadsheet s = new Spreadsheet();
            ISet<String> cells = new HashSet<string>();
            for (int i = 1; i < 20; i++)
            {
                cells.Add("A" + i);
                s.SetCellContents("A" + i, new Formula("A" + (i + 1)));
            }
        }
    }
}
