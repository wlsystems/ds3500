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
        public string SearchString
        {
            set
            {
                throw new NotImplementedException();
            }
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
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Message
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        string Form1View.CellValue
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Fired when a file is chosen with a file dialog.  The
        /// parameter is the chosen filename
        /// </summary>
        public event Action<string> FileChosenEvent;


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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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

        private void menuItem_Open_Click(object sender, EventArgs e)
        {
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    FileChosenEvent(fileDialog.FileName);
                }
            }
        }

        private void fileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {

        }
        
    }
}
