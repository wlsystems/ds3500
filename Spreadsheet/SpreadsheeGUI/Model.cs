using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public class Model : Spreadsheet
    {
        /// <summary>
        /// Constructs a Model with an empty contents
        /// </summary>
        public Model() 
        {
            Model m = new Model();
        }


        /// <summary>
        /// Constructs a Model with a regex parameter
        /// </summary>
        public Model(Regex isValid) 
        {
            Model M = new Model();
        }

        /// <summary>
        /// Constructs a Model with a regex parameter and source file 
        /// </summary>
        public Model(TextReader source, Regex newIsValid)
        {
            Model m = new Model();
        }

    }
}