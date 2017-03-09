using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetGUI;
using SSGui;

namespace ControllerTester
{
    class ViewStub : Form1View
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

        event Action<SpreadsheetPanel> Form1View.SelectionChangedEvent
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event Action<SpreadsheetPanel> Form1View.SelectionChangedEvent2
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event Action<SpreadsheetPanel, string> Form1View.FileChosenDisplay
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event Action<string> Form1View.FileSaveEvent
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        // These four methods cause events to be fired
        public void FireCloseEvent()
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        public string CellValue
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Message
        {
            set; get;
        }
        public string Title
        {
            set; get;
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

        public Action<SpreadsheetPanel> SelectionChangedEvent2
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

        public event Action CloseEvent;
        public event Action<string> FileChosenEvent;
        public event Action NewEvent;

        // These two methods implement the interface
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
