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
        

        string CellValue { set; }

        string Title { set; }

        string Message { set; }

        void DoClose();

        void OpenNew();
    }
}
