using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controllable interface of AnalysisWindow
    /// </summary>
    public interface Form1View
    {
        event Action<string> FileChosenEvent;

        event Action CloseEvent;

        event Action NewEvent;
        event Form1.SelectionChangedEventHandler SelectionChangedEvent;
        event Form1.SelectionChangedEventHandler2 SelectionChangedEvent2;
        event Form1.FileChosenDisplayHandler FileChosenDisplay;
        event Action<string> FileSaveEvent;

        string Title { set; }

        string Message { set; }

        void DoClose();

        void OpenNew();
        void SetTextValueBoxContent(string v);
        void SetTextBoxContent(string v);
    }
}
