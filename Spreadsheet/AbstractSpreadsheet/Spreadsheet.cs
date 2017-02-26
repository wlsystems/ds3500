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
using System.IO;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;

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
        private string val;
        public string v
        {
            get
            {
                return val;
            }
        }
        public int variety
        {
            get
            {
                return type;
            }
        }
        public string s
        {
            get
            {
                return str;
            }
        }
        public double d
        {
            get
            {
                return dbl;
            }
        }
        public Formula f
        {
            get
            {
                return frm;
            }
        }
        /// <summary>
        /// Type double
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dbl"></param>
        public Cell(int type, double dbl, string val)
        {
            this.type = type;
            this.dbl = dbl;
            this.val = val;
        }

        /// <summary>
        /// Type int
        /// </summary>
        public Cell(int type, string str, string val)
        {
            this.type = type;
            this.str = str;
            this.val = val;
        }

        /// <summary>
        /// Type Formula
        /// </summary>
        public Cell(int type, Formula frm, string val)
        {
            this.type = type;
            this.frm = frm;
            this.val = val;
        }
        public override string ToString() {
            if (variety == 1)
                return "" + dbl;
            else if (variety == 2)
                return s;
            else
                return f.ToString().Substring(1);
        }

    }

    /// <summary>
    /// 
    /// Inherits the AbstractSpreadsheet interface, but doesn't check for circular errors that is
    /// done in the superclass.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Regex rg;
        private DependencyGraph dg;
        private Dictionary<string, Cell> dic;

        public override bool Changed
        {
            get
            {
                throw new NotImplementedException();
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        ///
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  
        ///
        /// If there's a problem reading source, throws an IOException.
        ///
        /// Else if the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  
        ///
        /// Else if the IsValid string contained in source is not a valid C# regular expression, throws
        /// a SpreadsheetReadException.  (If the exception is not thrown, this regex is referred to
        /// below as oldIsValid.)
        ///
        /// Else if there is a duplicate cell name in the source, throws a SpreadsheetReadException.
        /// (Two cell names are duplicates if they are identical after being converted to upper case.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a 
        /// SpreadsheetReadException.  (Use oldIsValid in place of IsValid in the definition of 
        /// cell name validity.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a
        /// SpreadsheetVersionException.  (Use newIsValid in place of IsValid in the definition of
        /// cell name validity.)
        ///
        /// Else if there's a formula that causes a circular dependency, throws a SpreadsheetReadException. 
        ///
        /// Else, create a Spreadsheet that is a duplicate of the one encoded in source except that
        /// the new Spreadsheet's IsValid regular expression should be newIsValid.
        public Spreadsheet(TextReader source, Regex newIsValid)
        {
            
        }

        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        public Spreadsheet(Regex isValid) : base()
        {
            this.dg = new DependencyGraph();
            this.dic = new Dictionary<string, Cell>();
            this.rg = isValid;
        }

        /// <summary>
        /// 
        /// Construct and empty SpreadSheet using zero constructor.
        /// </summary>
        public Spreadsheet() : base()
        {
            this.dg = new DependencyGraph();
            this.dic = new Dictionary<string, Cell>();
            this.rg = new Regex(@"[\s\S]*[\w\W]*[\d\D]*$");
        }
        /// <summary>
        /// 
        /// Get the contents of a single cell.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// 
   


        public override object GetCellContents(string name)
        {
            if (name == null | !validName(name, rg))
                throw new InvalidNameException();
            if (!dic.ContainsKey(name))
                return "";
            else if (dic[name].variety == 1)
                return dic[name].d;
            else if (dic[name].variety == 2)
                return dic[name].s;
            else return dic[name].f;
        }
        /// <summary>
        /// Get an IEnumerableo of all cells in a dictionary that aren't empty.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (KeyValuePair<string, Cell> vals in this.dic) {
                if (vals.Value.variety == 2)
                {
                    if (vals.Value.s == "")
                        yield break;
                    else
                        yield return vals.Key;
                }   
                else
                    yield return vals.Key;
            }
        }
        /// <summary>
        /// 
        /// Helper method to check if cells have a valid format.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static Boolean validName(string name, Regex r)
        {
            if (name == null)
                throw new InvalidNameException();
            Match m = r.Match(name);
            return m.Success;
        }

       

        /// <summary>
        /// Set the contents of a cell to string and return all cells that are dependent on this cell
        /// plus the name of the cell itself.
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            HashSet<string> hs = new HashSet<string>();
            if (name == null | !validName(name, rg))
                throw new InvalidNameException();
            else
            {
                IEnumerable<string> ib = dg.GetDependees(name);
                IEnumerator<string> iet = ib.GetEnumerator();
                while (iet.MoveNext())
                    hs.Add(iet.Current);
                return hs;
            }         
        }

        public override void Save(TextWriter dest)
        {
            using (XmlWriter writer = XmlWriter.Create(dest))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Name");

                for (int i = 0; i < dic.Count; i++)
                {
                    writer.WriteStartElement("Name");
                    writer.WriteAttributeString("Name", dic.ElementAt(i).Key);
                    writer.WriteAttributeString("Contents", dic.ElementAt(i).Value.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public override object GetCellValue(string name)
        {
            object j = new object();
            if (dic[name].variety == 3)
            {
                Formula f = new Formula(dic[name].v);
                return f.Evaluate(s => (double)(GetCellValue(s)));
            }
            else if (dic[name].variety == 1)
                return Double.Parse(dic[name].v);
            else
                return dic[name].v;
        }

        public string Lookup4(String v)
        {
            v = v.ToUpper();
            return v;
        }

        public bool Lookup5(String v)
        {
            Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*");
            if (validName(v, rg1))
                return true;
            else
                return false;
        }

        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (name == null | !validName(name, rg))
                throw new InvalidNameException();
            if (content == null)
                throw new ArgumentNullException();
            Double d;
            if (Double.TryParse(content, out d)) {
                ISet<string> iss = SetCellContents(name.ToUpper(), d);
                return iss;
            }

            else if (!content.Equals("") && content.ElementAt<char>(0) == '=')
            {
                Formula f = new Formula(content.Substring(1), Lookup4, Lookup5);
                ISet<string> iss = SetCellContents(name, f);
                return iss;
            }
            ISet<string> iss1 = SetCellContents(name, content);
            return iss1;
        }

        protected override ISet<string> SetCellContents(string name, double number)
        {
            if (name == null | !validName(name, rg))
                throw new InvalidNameException();
            else
            {
                IEnumerable<string> ib = GetCellsToRecalculate(name);
                IEnumerator<string> iet = ib.GetEnumerator();
                HashSet<string> hs = new HashSet<string>();
                hs.Add(name);
                Cell cl = new SS.Cell(1, number, "" + number);
                if (this.dic.ContainsKey(name))
                    this.dic.Remove(name);
                this.dic.Add(name, cl);
                while (iet.MoveNext())
                    hs.Add(iet.Current);
                UpdateCellValues(ib);
                return hs;
            }
        }

        protected override ISet<string> SetCellContents(string name, string text)
        {
            if (!validName(name, rg))
                throw new InvalidNameException();
            if (text == (string)null)
                throw new ArgumentNullException(name);
            else
            {
                IEnumerable<string> ib = GetCellsToRecalculate(name);
                IEnumerator<string> iet = ib.GetEnumerator();
                HashSet<string> hs = new HashSet<string>();
                hs.Add(name); 
                Cell cl = new SS.Cell(2, text, text);
                if (this.dic.ContainsKey(name))
                    this.dic.Remove(name);
                this.dic.Add(name, cl);
                while (iet.MoveNext())
                    hs.Add(iet.Current);
                UpdateCellValues(ib);
                return hs;
            }
        }

        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (!validName(name, rg))
                throw new InvalidNameException();
            else
            {            
                IEnumerable<string> tokens = formula.GetVariables();
                foreach (string s in tokens)
                    this.dg.AddDependency(name, s);
                IEnumerable<string> ib = GetCellsToRecalculate(name);
                if (this.dic.ContainsKey(name))
                    this.dic.Remove(name);

                Cell cl = new SS.Cell(3, formula, "");
                this.dic.Add(name, cl);
                HashSet<string> hs = new HashSet<string>();
                hs.Add(name);
                IEnumerator<string> iet = ib.GetEnumerator();
                while (iet.MoveNext())
                    hs.Add(iet.Current);
                UpdateCellValues(ib);
                return hs;
            }
        }

        private void UpdateCellValues(IEnumerable<string> cellnames)
        {
            string pattern = "";
            string replacement = "";
            int type = 0;
            string result = "";
            Regex rgx = new Regex(pattern);
            foreach (string cell in cellnames)
            {
                Formula f;
                type = dic[cell].variety; //determine if it is string or double
                if (type == 1)
                    replacement = "" + dic[cell].d;
                if (type == 2)
                    replacement = dic[cell].s;
                if (type == 3)
                {
                    f = dic[cell].f;
                    string formula = f.ToString();
                    IEnumerable<string> x = f.GetVariables();
                    result = formula; 
                    foreach (string str in x)
                    {
                        if (dic.ContainsKey(str))
                        {

                            pattern = str + "+";                         
                            rgx = new Regex(pattern);
                            result = rgx.Replace(formula, dic[str].v);                         
                        }
                        
                    }

                    Formula f1 = dic[cell].f;
                    Cell c0 = new Cell(3, f1, result);
                    this.dic.Remove(cell);
                    dic.Add(cell, c0);
                }
                pattern = cell + "+";

                if (type == 1)
                {
                    double d = dic[cell].d;
                    Cell c0 = new Cell(1, d, replacement);
                    this.dic.Remove(cell);
                    dic.Add(cell, c0);
                }
                if (type == 2)
                {
                    string s = dic[cell].s;
                    Cell c0 = new Cell(2, s, replacement);
                    this.dic.Remove(cell);
                    dic.Add(cell, c0);
                }
            }
        }
    }
}
