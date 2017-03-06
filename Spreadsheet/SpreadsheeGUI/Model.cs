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
        // The contents of the open file in the AnalysisWindow, or the
        // empty string if no file is open.
        private string contents;

        /// <summary>
        /// Constructs a Model with an empty contents
        /// </summary>
        public Model() :base()
        {
            contents = "";
        }
        /// <summary>
        /// Constructs a Model with an empty contents
        /// </summary>

        /// <summary>
        /// Makes the contents of the named file the new value of contents
        /// </summary>
        public void ReadFile(string filename)
        {
            contents = File.ReadAllText(filename);
        }

     
    }
}