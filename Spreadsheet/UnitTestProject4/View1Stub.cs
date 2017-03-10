using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetGUI;

namespace ControllerTester
{
    class View1Stub : Form1View
    {

        public string Message
        {
            set; get;
        }

        public string Title
        {
            set; get;
        }

        // These four methods cause events to be fired
        public void FireCloseEvent()
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }
        public event Action CloseEvent;
        public event Form1.FileChosenDisplayHandler FileChosenDisplay;
        public event Action<string> FileChosenEvent;
        public event Action<string> FileSaveEvent;
        public event Action NewEvent;
        public event Form1.SelectionChangedEventHandler SelectionChangedEvent;
        public event Form1.SelectionChangedEventHandler2 SelectionChangedEvent2;
        // These two properties record whether a method has been called
        public bool CalledDoClose
        {
            get; private set;
        }

        public bool CalledOpenNew
        {
            get; private set;
        }
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
            SetTextBoxContent(v);
        }

        public void SetTextValueBoxContent(string v)
        {
            SetTextValueBoxContent(v);
        }
    }
}
