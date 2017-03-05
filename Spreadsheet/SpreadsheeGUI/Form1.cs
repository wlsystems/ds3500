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

namespace SpreadsheeGUI
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

        public event Action<string> FileChosenEvent;
        public event Action CloseEvent;
        public event Action NewEvent;
        public event Action<string> CountEvent;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {
            
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void spreadsheetPanel2_Load(object sender, EventArgs e)
        {

        }

        public void DoClose()
        {
            Close();
        }

        public void OpenNew()
        {
            throw new NotImplementedException();
        }

        public void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1ApplicationContext.GetContext().RunNew();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String s = sender.ToString();
            FileChosenEvent(s);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

    }
}
