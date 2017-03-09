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
        private Form1View Window;
        private SpreadsheetPanel panel;
        private Spreadsheet model;
        // The contents of the open file in the AnalysisWindow, or the
        // empty string if no file is open.

        public delegate void UpdateValueEventHandler(String content);

        public event UpdateValueEventHandler UpdateValueEvent;

        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(Form1View window)
        {
            this.Window = window;
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
                Window.Title = filename;
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
                Window.Message = "Unable to open file\n" + ex.Message;
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
                ISet<string> recalculate = model.SetContentsOfCell(cellName, sender.cellContent);
                panel.SetValue(x, y, model.GetCellValue(cellName).ToString());
                UpdateDepCells(sender, recalculate);
                panel.HideTextBox();
            }
        }

        private void UpdateDepCells(SpreadsheetPanel sender, ISet<string> recalculate)
        {
            foreach (var cell in recalculate)
            {
                int x = Convert.ToInt16(Convert.ToChar(cell.Substring(0, 1)) - 65);
                int y = Convert.ToInt16(cell.Substring(1)) - 1;
                sender.SetValue(x, y, model.GetCellValue(cell).ToString());
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
            ISet<string> recalculate;
            if (panel.cellContent != "")
            
            {
                recalculate= model.SetContentsOfCell(cellName, panel.cellContent);
                panel.SetValue(x, y, model.GetCellValue(cellName).ToString());
                panel.SetTextBox(model.GetCellContents(cell).ToString());
                Window.SetTextBoxContent(model.GetCellContents(cell).ToString());
                Window.SetTextValueBoxContent(model.GetCellValue(cell).ToString());
                UpdateDepCells(sender, recalculate);
            }
            else
            {
                panel.SetTextBox(model.GetCellContents(cell).ToString());
                Window.SetTextBoxContent(model.GetCellContents(cell).ToString());
                Window.SetTextValueBoxContent(model.GetCellValue(cell).ToString());
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
                Window.Title = filename;
                TextWriter t = new StreamWriter(filename);
                model.Save(t);

            }
            catch (Exception ex)
            {
                Window.Message = "Unable to save file\n" + ex.Message;
            }
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            if (model.Changed == true)
            {
                DialogResult result = MessageBox.Show("Warning! You have unsaved changes, click OK to close without saving changes.","Unsaved Changes!",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
                if ( result == DialogResult.OK)
                {

                    Window.DoClose();
                }
                else 
                {
                    //User canceled close. 
                }
                
            }
            else
            {
                Window.DoClose();
            }
        }

    
        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {
            Window.OpenNew();
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
