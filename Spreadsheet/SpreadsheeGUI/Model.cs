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
    public class AbstractSpreadsheet : Spreadsheet
    {
        /// <summary>
        /// Constructs a Model with an empty contents
        /// </summary>
        public AbstractSpreadsheet() :base()
        {
        }


        /// <summary>
        /// Constructs a Model with a regex parameter
        /// </summary>
        public AbstractSpreadsheet(Regex isValid) 
        {
        }

        /// <summary>
        /// Constructs a Model with a regex parameter and source file 
        /// </summary>
        public AbstractSpreadsheet(TextReader source, Regex newIsValid)
        {
        }

    }
}