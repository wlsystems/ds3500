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
using System.Windows.Forms;

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

        public delegate void UpdateValueEventHandler(String content);

        public event UpdateValueEventHandler UpdateValueEvent;

        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(Form1 window)
        {
            this.window = window;
            this.model = new Spreadsheet();
            this.panel = new SpreadsheetPanel();
            window.CloseEvent += HandleClose;
            window.FileChosenEvent += HandleFileChosen;
            window.FileSaveEvent += HandleFileSave;
            window.SelectionChangedEvent += HandleSelectionChangedEvent;
            window.SelectionChangedEvent2 += Window_SelectionChangedEvent2;
            window.FileChosenDisplay += Window_FileChosenDisplay;
        }

        private void Window_FileChosenDisplay(SpreadsheetPanel sender, string filename)
        {
            try
            {
                panel.Clear();
                window.Title = filename;
                TextReader t = new StreamReader(filename);
                model = new Spreadsheet(t, new Regex(@"[A-Z]+[1-9][0-9]*"));
                IEnumerable<string> allCell = model.GetNamesOfAllNonemptyCells();
                foreach (string cell in allCell)
                {
                    int x = Convert.ToInt16(Convert.ToChar(cell.Substring(0, 1)) - 65);
                    int y = Convert.ToInt16(cell.Substring(1)) - 1;
                    sender.SetValue(x, y, model.GetCellValue(cell).ToString());
                }

            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
        }

        private void Window_SelectionChangedEvent2(SpreadsheetPanel sender)
        {
            panel = sender;
            int x = panel.cellColCurrent;
            int y = panel.cellRowCurrent;
            string cellName = ConvertCellName(x, y);
            if (panel.cellContent != "")
            {
                model.SetContentsOfCell(cellName, sender.cellContent);
                panel.SetValue(x, y, model.GetCellValue(cellName).ToString());
                panel.HideTextBox();
            }
        }


        /// <summary>
        /// Fired when the contents is updated. 
        /// </summary>
        private void HandleSelectionChangedEvent(SpreadsheetPanel sender)
        {
            panel = sender;
            int x = panel.cellCol;
            int y = panel.cellRow;
            int thisx = panel.cellColCurrent;
            int thisy = panel.cellRowCurrent;
            string cell = ConvertCellName(thisx, thisy);
            string cellName = ConvertCellName(x, y);
            if (panel.cellContent != "")
            {
                model.SetContentsOfCell(cellName, panel.cellContent);
                panel.SetValue(x, y, model.GetCellValue(cellName).ToString());
                panel.SetTextBox(model.GetCellContents(cell).ToString());
                window.SetTextBoxContent(model.GetCellContents(cell).ToString());
                window.SetTextValueBoxContent(model.GetCellValue(cell).ToString());
            }
            else
            {
                panel.SetTextBox(model.GetCellContents(cell).ToString());
                window.SetTextBoxContent(model.GetCellContents(cell).ToString());
                window.SetTextValueBoxContent(model.GetCellValue(cell).ToString());
            }
        }


        public void Window_FileChoosenDisplay()
        {
            IEnumerable<string> allCell = model.GetNamesOfAllNonemptyCells();
            foreach (var cell in allCell)
            {
                int x = Convert.ToInt16(Convert.ToChar(cell.Substring(0, 1)) - 65);
                int y = Convert.ToInt16(cell.Substring(1)) - 1;
                panel.SetValue(x, y, "8");
            }
        }

        /// <summary>
        /// Handles a request to open a file.
        /// </summary>
        public void HandleFileChosen(string filename)
        {
                      
        }

        /// <summary>
        /// Handles a request to save a file.
        /// </summary>
        private void HandleFileSave(string filename)
        {
            try
            {
                window.Title = filename;
                TextWriter t = new StreamWriter(filename);
                model.Save(t);

            }
            catch (Exception ex)
            {
                window.Message = "Unable to save file\n" + ex.Message;
            }
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            if (model.Changed == true)
            {
                MessageBox.Show("Warning! You have unsaved changes.");
            }
            window.DoClose();
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClick()
        {
            if (model.Changed == true)
            {
                MessageBox.Show("Warning! You have unsaved changes.");
            }
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

        public Spreadsheet ReturnSS()
        {
            return model;
        }
    }
}
