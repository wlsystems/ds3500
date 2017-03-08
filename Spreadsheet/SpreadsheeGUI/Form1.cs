using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSGui;

namespace SpreadsheetGUI
{
    public partial class Form1 : Form, Form1View
    {
        public Form1()
        {
            InitializeComponent();
            spreadsheetPanel1.SetSelection(0, 0);
            spreadsheetPanel1.SelectionChanged += displaySelection;
            UpdateCellNameTxtBox();
        }

        private void displaySelection(SpreadsheetPanel sender)
        {
            UpdateCellNameTxtBox();
        }
     


        private void UpdateCellNameTxtBox()
        {
            int currRow = spreadsheetPanel1.cellRowCurrent;
            int currCol = spreadsheetPanel1.cellColCurrent;
            txtCellName.Text = string.Format("{0}{1}", (Convert.ToChar(currCol+65)).ToString(), (currRow+1).ToString());
        }
        /// <summary>
        /// Fired when the cell contents is updated.
        /// </summary>
        public event SelectionChangedEventHandler2 SelectionChangedEvent2;


        public delegate void SelectionChangedEventHandler2(SpreadsheetPanel sender);

        /// <summary>
        /// Fired when the cell contents is updated.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChangedEvent;


        public delegate void SelectionChangedEventHandler(SpreadsheetPanel sender);

        public delegate void FileChosenDisplayHandler(SpreadsheetPanel sender);

        public event FileChosenDisplayHandler FileChosenDisplay;

        /// <summary>
        /// Handles the content
        /// </summary>
        /// <param name="str"></param>
        public delegate void TextChangedHandler(String content);

        public event TextChangedHandler TextChangedEvent;

        /// <summary>
        /// Fired when a file is chosen with a file dialog.  The
        /// parameter is the chosen filename
        /// </summary>
        public event Action<string> FileChosenEvent;

        /// <summary>
        /// Fired when a file is chosen for a save.  The parameter
        /// is the chosen filename. 
        /// </summary>
        public event Action<string> FileSaveEvent;

        /// <summary>
        /// Fired when a close action is requested.
        /// </summary>
        public event Action CloseEvent;
        
        /// <summary>
        /// Fired when a new action is requested.
        /// </summary>
        public event Action NewEvent;

        /// <summary>
        /// Fired when a request is made to count occurrences of a string.
        /// The parameter is the string.
        /// </summary>
        public event Action<string> CountEvent;

        
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void menuItem_Close_Click(object sender, EventArgs e)
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }
        


        public void DoClose()
        {
            Close();
        }

        public void OpenNew()
        {
            throw new NotImplementedException();
        }

        public void menuItem_New_Click(object sender, EventArgs e)
        {
            Form1ApplicationContext.GetContext().RunNew();
        }

        public void menuItem_Open_Click(object sender, EventArgs e)
        {
            DialogResult result = openfileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    FileChosenEvent(openfileDialog.FileName);

                }
            }
        }


        private void menuItem_Save_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileSaveEvent != null)
                {
                    FileSaveEvent(saveFileDialog.FileName);
                }

            }
        }

        private void fileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {

        }

        private void txtCellName_TextChanged(object sender, EventArgs e)
        {   
        }


        public string CellValue
        {
            get
          {
                int col = 0;
                int row = 0;
                string str = "";
                SpreadsheetPanel.GetContext().GetValue(col, row, out str);
                return str;
            }
        }

        public string CellContents
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Not sure if we need this
        /// </summary>
        public string Title
        {
           set { Text = value; }
            
        }

        public string Message
        {
            set { MessageBox.Show(value); }
        }

        string Form1View.CellValue
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void spreadsheetPanel1_Load_1(object sender, EventArgs e)
        {

        }

        private void spreadsheetPanel1_SelectionChanged(SpreadsheetPanel sender)
        {
            if (SelectionChangedEvent != null)
            {
                SelectionChangedEvent(sender);
            }
        }

        /// <summary>
        /// captures all keystrokes from user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCellContents_TextChanged(object sender, EventArgs e)
        {
            txtCellContents.KeyPress += TxtCellContents_KeyPress1;
        }

        private void TxtCellContents_KeyPress1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                if (SelectionChangedEvent2 != null)
                {
                    spreadsheetPanel1.cellContent = txtCellContents.Text;
                    SelectionChangedEvent2(spreadsheetPanel1);
                }
            }
        }


        /// <summary>
        /// 
        /// Detect if the enter key is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtCellContents_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
