using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheeGUI
{

    public class Controller
    {
        // The window being controlled
        private Form1 window;

        // The contents of the open file in the AnalysisWindow, or the
        // empty string if no file is open.
        private string fileContents = "";

        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(Form1 window)
        {
            this.window = window;
            window.CloseEvent += HandleClose;                  
        }

        /// <summary>
        /// Handles a request to open a file.
        /// </summary>
        private void HandleFileChosen(String filename)
        {
            try
            {
                fileContents = File.ReadAllText(filename);

            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {

            window.DoClose();
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClick()
        {
            window.DoClose();
        }
        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {
            window.OpenNew();
        }

    }
}
