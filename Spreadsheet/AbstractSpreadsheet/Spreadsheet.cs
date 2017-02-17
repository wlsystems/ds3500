/// <summary>
/// DustinShiozaki u0054455 2/2017
/// </summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;
using System.Text.RegularExpressions;
using SS;

/// <summary>
/// Contains cell to contain the contents of the Cell and Spreadsheet which contains the DependencyGraph.
/// The type is stored in an integer for future reference but it was designed as a future safeguard and 
/// can easily be removed if necessary. All 3 of the data types are contained in properties of the class.
/// </summary>
namespace SS
{
    /// <summary>
    /// Contains the cell which stores a type, and value
    /// the cells will form a dicitionary in the Spreadsheet class.
    /// </summary>
    public class Cell
    {
        private int type;
        private double dbl;
        private string str;
        private Formula frm;
       
        /// <summary>
        /// Type double
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dbl"></param>
        public Cell(int type, double dbl)
        {
            this.type = type;
            this.dbl = dbl;
        }

        /// <summary>
        /// Type int
        /// </summary>
        public Cell(int type, string str)
        {
            this.type = type;
            this.str = str;
        }

        /// <summary>
        /// Type Formula
        /// </summary>
        public Cell(int type, Formula frm)
        {
            this.type = type;
            this.frm = frm;
        }
    }

    /// <summary>
    /// 
    /// Inherits the AbstractSpreadsheet interface, but doesn't check for circular errors that is
    /// done in the superclass.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private DependencyGraph dg;
        private Dictionary<string, Cell> dic;
        /// <summary>
        /// 
        /// Construct and empty SpreadSheet using zero constructor.
        /// </summary>
        public Spreadsheet() : base()
        {
            this.dg = new DependencyGraph();
            this.dic = new Dictionary<string, Cell>();

        }
        /// <summary>
        /// 
        /// Get the contents of a single cell.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetCellContents(string name)
        {
            if (dic.ContainsKey(name))
                return dic[name];
            else
                throw new InvalidNameException(name);
        }
        /// <summary>
        /// Get an IEnumerableo of all cells in a dictionary that aren't empty.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (KeyValuePair<string, Cell> vals in this.dic)
                yield return vals.Key;
        }
        /// <summary>
        /// 
        /// Helper method to check if cells have a valid format.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static Boolean validName(string name)
        {
            const string pat = @"[A-Z]+[1-9]+$";
            Regex r = new Regex(pat);
            Match m = r.Match(name);
            return m.Success;
        }

        /// <summary>
        /// checks if the name is valid and returns dependencies of that name.
        /// If there is a circular problem it returns a 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (name == null | !validName(name))
                throw new InvalidNameException(name);
            else
            {
                Cell cl = new SS.Cell(3, formula);
                HashSet<string> hs = new HashSet<string>();
                hs.Add(name);
                if (this.dic.ContainsKey(name))
                    this.dic.Remove(name);
                this.dic.Add(name, cl);

                IEnumerable<string> tokens = formula.GetVariables();
                foreach (string s in tokens)
                    this.dg.AddDependency(name, s);

                IEnumerable<string> ib = dg.GetDependees(name);
                IEnumerator<string> iet = ib.GetEnumerator();
                while (iet.MoveNext())
                    hs.Add(iet.Current);
                return hs;
            }
        }
        /// <summary>
        /// Set the contents of a cell to string and return all cells that are dependent on this cell
        /// plus the name of the cell itself.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            if (name == null | !validName(name))
                throw new InvalidNameException(name);
            else
            {
                HashSet<string> hs = new HashSet<string>();
                hs.Add(name);
                Cell cl = new SS.Cell(2, text);
                if (this.dic.ContainsKey(name))
                    this.dic.Remove(name);
                this.dic.Add(name, cl);
                IEnumerable<string> ib = dg.GetDependees(name);
                IEnumerator<string> iet = ib.GetEnumerator();
                while (iet.MoveNext())
                    hs.Add(iet.Current);
                return hs;
            }
        }
        /// <summary>
        /// Set the contents of a cell to double return all cells that are dependent on this cell
        /// plus the name of the cell itself.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            if (name == null | !validName(name))
                throw new InvalidNameException(name);
            else
            {
                HashSet<string> hs = new HashSet<string>();
                hs.Add(name);
                Cell cl = new SS.Cell(1, number);
                if (this.dic.ContainsKey(name))
                    this.dic.Remove(name);
                this.dic.Add(name, cl);
                IEnumerable<string> ib = dg.GetDependees(name);
                IEnumerator<string> iet = ib.GetEnumerator();
                while (iet.MoveNext())
                    hs.Add(iet.Current);
                return hs;
            }  
        }

        /// <summary>
        /// Set the contents of a cell to string and return all cells that are dependent on this cell
        /// plus the name of the cell itself.
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            HashSet<string> hs = new HashSet<string>();
            if (name == null | !validName(name))
                throw new InvalidNameException(name);
            else
            {
                IEnumerable<string> ib = dg.GetDependents(name);
                IEnumerator<string> iet = ib.GetEnumerator();
                while (iet.MoveNext())
                    hs.Add(iet.Current);
                return hs;
            }         
        }
    }
}
