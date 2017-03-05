using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheeGUI
{
    /// <summary>
    /// Controllable interface of AnalysisWindow
    /// </summary>
    public interface Form1View
    {
        event Action<string> FileChosenEvent;

        event Action<string> CountEvent;

        event Action CloseEvent;

        event Action NewEvent;

        string SearchString { set; }

        string CellValue { set; }

        string Title { set; }

        string Message { set; }

        void DoClose();

        void OpenNew();
    }
}
