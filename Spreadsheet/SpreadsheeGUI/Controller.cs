using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSGui;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.ObjectModel;
using SS;

namespace SpreadsheetGUI
{

    public class Controller
    {


        // The window being controlled
        private Form1 window;
        private SpreadsheetPanel panel;
        private Spreadsheet model;
        // The contents of the open file in the AnalysisWindow, or the
        // empty string if no file is open.
        private string fileContents = "";

        public delegate void UpdateValueEventHandler(String content);

        public event UpdateValueEventHandler UpdateValueEvent;

        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(Form1 window)
        {
            this.window = window;
            this.model = new AbstractSpreadsheet();
            window.CloseEvent += HandleClose;
            window.SelectionChangedEvent += HandleSelectionChangedEvent;
            window.TextChangedEvent += HandletxtContentsChangedEvent;
        }

        /// <summary>
        /// when the contents is changed.
        /// </summary>
        private void HandletxtContentsChangedEvent(String s)
        {
            
        }

        /// <summary>
        /// Fired when the contents is updated. 
        /// </summary>
        private void HandleSelectionChangedEvent(SpreadsheetPanel sender)
        {
            panel = sender;
            int x = panel.cellCol;
            int y = panel.cellRow;
            string cellName = ConvertCellName(x, y);

            if (panel.cellContent != "")
            {
                model.SetContentsOfCell(cellName, panel.cellContent);
                panel.SetValue(x, y, model.GetCellValue(cellName).ToString());
            }
        }

        /// <summary>
        /// Handles a request to open a file.
        /// </summary>
        private void HandleFileChosen(string filename)
        {
            try
            {
               window.Title = filename;
                TextReader t = new StreamReader(filename); 
                Spreadsheet model = new AbstractSpreadsheet(t, new Regex(@"[A-Z]+[1-9][0-9]*"));
                IEnumerable<string> allCell = model.GetNamesOfAllNonemptyCells();
                foreach (var cell in allCell)
                {
                    int x = Convert.ToInt32(cell.Substring(0, 1));
                    int y = Convert.ToInt32(cell.Substring(1))-1;
                    Console.WriteLine(x.ToString(), y);
                    panel.SetValue(x, y, model.GetCellValue(cell).ToString());
                }
            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            window.DoClose();
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClick()
        {
            window.DoClose();
        }
        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {
            window.OpenNew();
        }

        private string ConvertCellName(int x, int y)
        {
            return string.Format("{0}{1}", (Convert.ToChar(x + 65)).ToString(), (y + 1).ToString());
        } 

    }
}
