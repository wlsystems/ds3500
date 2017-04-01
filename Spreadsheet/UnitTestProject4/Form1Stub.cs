using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetGUI;
using SSGui;

namespace ControllerTester
{
    class Form1Stub : Form1View
    {
        // These two properties record whether a method has been called
        public bool CalledDoClose
        {
            get; private set;
        }

        public bool CalledOpenNew
        {
            get; private set;
        }

        public Action<SpreadsheetPanel, string> FileChosenDisplay
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Action<string> FileSaveEvent
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Message
        {
            set; get;
        }

        public Action<SpreadsheetPanel> SelectionChangedEvent
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Title
        {
            set;
            get;
        }

        public event Action CloseEvent;
        public event Action<string> FileChosenEvent;

        public void DoClose()
        {
            CalledDoClose = true;
        }

        public void OpenNew()
        {
            CalledOpenNew = true;
        }

        public void SetTextBoxContent(string v)
        {
            throw new NotImplementedException();
        }

        public void SetTextValueBoxContent(string v)
        {
            throw new NotImplementedException();
        }
    }
}
