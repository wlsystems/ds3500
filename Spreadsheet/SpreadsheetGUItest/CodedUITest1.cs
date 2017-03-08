using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using SpreadsheetGUI;
using Spreadsheet;

namespace SpreadsheetGUItest
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class CodedUITest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            Form1 window = new Form1();
            Controller controller = new Controller(window);
            controller.HandleFileChosen("hello.ss");
           // Spreadsheet ss = new AbstractSpreadsheet();
           // ss = controller.ReturnSS;

        }