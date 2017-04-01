using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSGui;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controllable interface of AnalysisWindow
    /// </summary>
    public interface Form1View
    {
        event Action<string> FileChosenEvent;
        event Action CloseEvent;
        event SelectionChangedEventHandler SelectionChangedEvent;
        event SelectionChangedEventHandler2 SelectionChangedEvent2;
        event Action NewEvent;
        
        string Title { set; }

        string Message { set; }


        void DoClose();

        void OpenNew();
        void SetTextBoxContent(string v);
        void SetTextValueBoxContent(string v);
    }
}
