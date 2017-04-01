﻿using PS8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS8
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BoggleForm view = new BoggleForm();
            //new Controller1(view);
            //new Controller2(view);
            new Controller1(view);
            Application.Run(view);
        }
    }
}
