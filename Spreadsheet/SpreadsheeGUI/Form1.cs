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
                throw new NotImplementedException();
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
        public event Action<string> CountEvent;
        public event Action CloseEvent;
        public event Action NewEvent;

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public void DoClose()
        {
            throw new NotImplementedException();
        }

        public void OpenNew()
        {
            throw new NotImplementedException();
        }

        public void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1ApplicationContext.GetContext().RunNew();
        
        }
    }
}
