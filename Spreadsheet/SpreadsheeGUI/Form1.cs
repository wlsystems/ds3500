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
            spreadsheetPanel1.SetSelection(0, 0);          //sets default to A1
            spreadsheetPanel1.SelectionChanged += displaySelection;
            UpdateCellNameTxtBox();                        //sets uneditable cellname text box 
        }

        private void displaySelection(SpreadsheetPanel sender)
        {
            UpdateCellNameTxtBox();
        }
    
        /// <summary>
        /// When a user clicks to a different cell it updates the name of the cell that has the focus. 
        /// </summary>
        private void UpdateCellNameTxtBox()
        {

            int currCol = spreadsheetPanel1.cellColCurrent; //looks up the current column 
            int currRow = spreadsheetPanel1.cellRowCurrent; //looks the current row 
            txtCellName.Text = string.Format("{0}{1}", (Convert.ToChar(currCol+65)).ToString(),   //converts to letter char
                                                                (currRow+1).ToString());    //adds one to adjust from 0 index
        }

        /// <summary>
        /// Fired when the cell contents is updated.
        /// </summary>
        public event SelectionChangedEventHandler2 SelectionChangedEvent2;

        /// <param name="sender"></param>
        public delegate void SelectionChangedEventHandler2(SpreadsheetPanel sender);

        /// <summary>
        /// Fired when the cell contents is updated.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChangedEvent;


        public delegate void SelectionChangedEventHandler(SpreadsheetPanel sender);

        /// <summary>
        /// Fired when the open dialoug window is used and a file to open is selected. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="filename"></param>
        public delegate void FileChosenDisplayHandler(SpreadsheetPanel sender, string filename);

        public event FileChosenDisplayHandler FileChosenDisplay;

        /// <summary>
        /// Handles the content. 
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
        /// <summary>
        /// Open is selected from the file drop down menu, ask the user to select a file and then fires
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuItem_Open_Click(object sender, EventArgs e)
        {
            DialogResult result = openfileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    FileChosenDisplay(spreadsheetPanel1, openfileDialog.FileName);
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

        /// <summary>
        /// Not sure if we need this
        /// </summary>
        public string Title
        {
           set { Text = value; }
           get { return Text; }
            
        }

        public string Message
        {
            set { MessageBox.Show(value); }
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void spreadsheetPanel1_Load_1(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Event fired when cell contents changed .
        /// </summary>
        /// <param name="sender"></param>
        private void spreadsheetPanel1_SelectionChanged(SpreadsheetPanel sender)
        {
            if (SelectionChangedEvent != null)
            {
                SelectionChangedEvent(sender);
            }
        }
        /// <summary>
        /// Update the valuebox when cell is clicked.
        /// </summary>
        /// <param name="s"></param>
        public void SetTextBoxContent(String s)
        {
            txtCellContents.Text = s;
        }

        /// <summary>
        /// Update the contents of the textbox when the cell is clicked.
        /// </summary>
        /// <param name="s"></param>
        public void SetTextValueBoxContent(String s)
        {
            txtValue.Text = s;
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Give us an A!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
