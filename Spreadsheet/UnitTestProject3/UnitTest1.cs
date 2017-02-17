using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using Formulas;
using System.Collections.Generic;

namespace UnitTestProject3
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Simple set constructor should not cause error.
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
        }
        /// <summary>
        /// Simple set cell should not cause error.
        /// </summary>
        [TestMethod]
        public void TestAddCell1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("A1+B2");
            ss.SetCellContents("A1", f);
        }

        /// <summary>
        /// Simple set cell should not cause error.
        /// </summary>
        [TestMethod]
        public void TestAddCell2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("A1+B2");
            ss.SetCellContents("A1", 2);
        }

        /// <summary>
        /// Simple set cell should not cause error.
        /// </summary>
        [TestMethod]
        public void TestAddCell3()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("A1+B2");
            ss.SetCellContents("A1", "2");
        }

        /// <summary>
        /// Simple test to get dependees
        /// </summary>
        [TestMethod]
        public void TestAddCell4()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("A1+B2");
            ISet<string> iSet = ss.SetCellContents("A1", f);
            Assert.AreEqual(1, iSet.Count);
        }

        /// <summary>
        /// Simple test to get dependees
        /// </summary>
        [TestMethod]
        public void TestAddCell5()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", f);
            ss.SetCellContents("C1", f1);
            ISet<string> iSet2 = ss.SetCellContents("A1", "D1");
            Assert.AreEqual(3, iSet2.Count);
        }

        /// <summary>
        /// Simple test to get dependees
        /// </summary>
        [TestMethod]
        public void TestAddCell6()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", f);
            ss.SetCellContents("C1", f1);
            ISet<string> iSet2 = ss.SetCellContents("A1", "D1");
            IEnumerator<string> ies = iSet2.GetEnumerator();
            ies.MoveNext();
            Assert.AreEqual("A1", ies.Current);
        }

        /// <summary>
        /// Simple test to get dependees
        /// </summary>
        [TestMethod]
        public void TestAddCell7()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", f);
            ss.SetCellContents("C1", f1);
            ISet<string> iSet2 = ss.SetCellContents("A1", 3);
            IEnumerator<string> ies = iSet2.GetEnumerator();
            ies.MoveNext();
            Assert.AreEqual("A1", ies.Current);
        }

        /// <summary>
        /// Simple test to get dependees
        /// </summary>
        [TestMethod]
        public void TestAddCell8()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f2 = new Formula("E1+F1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", f);
            ss.SetCellContents("C1", f1);
            ISet<string> iSet2 = ss.SetCellContents("A1", f2);
            IEnumerator<string> ies = iSet2.GetEnumerator();
            ies.MoveNext();
            Assert.AreEqual("A1", ies.Current);
        }

        /// <summary>
        /// Simple test to get dependees
        /// </summary>
        [TestMethod]
        public void TestAddCell9()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f2 = new Formula("E1+F1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", f);
            ss.SetCellContents("C1", f1);
            ISet<string> iSet2 = ss.SetCellContents("A1", 3);
            IEnumerator<string> ies = iSet2.GetEnumerator();
            ies.MoveNext();
            ies.MoveNext();
            Assert.AreEqual("B1", ies.Current);
        }

        /// <summary>
        /// Simple test to get dependees
        /// </summary>
        [TestMethod]
        public void TestAddCell10()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f2 = new Formula("E1+F1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", f);
            ss.SetCellContents("C1", f1);
            ISet<string> iSet2 = ss.SetCellContents("A1", 3);
            IEnumerator<string> ies = iSet2.GetEnumerator();
            ies.MoveNext();
            ies.MoveNext();
            ies.MoveNext();
            Assert.AreEqual("C1", ies.Current);
        }

        /// <summary>
        /// Simple test to get dependees
        /// </summary>
        [TestMethod]
        public void TestAddCell11()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f2 = new Formula("E1+F1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", f);
            ss.SetCellContents("C1", f1);
            ISet<string> iSet2 = ss.SetCellContents("A1", f2);
            IEnumerator<string> ies = iSet2.GetEnumerator();
            ies.MoveNext();
            ies.MoveNext();
            ies.MoveNext();
            ies.MoveNext();
            Assert.AreEqual(null, ies.Current);
        }

        /// <summary>
        /// Simple test to get dependees
        /// </summary>
        [TestMethod]
        public void TestAddCell12()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f2 = new Formula("E1+F1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", f);
            ss.SetCellContents("C1", f1);
            ISet<string> iSet2 = ss.SetCellContents("A1", f2);
            IEnumerator<string> ies = iSet2.GetEnumerator();
            ies.MoveNext();
            ies.MoveNext();
            ies.MoveNext();
            Assert.AreEqual("C1", ies.Current);
        }

        /// <summary>
        /// Simple test to get NONEMTPY
        /// </summary>
        [TestMethod]
        public void TestNamesOfAllNonemptyCells1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f2 = new Formula("E1+F1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", f);
            ss.SetCellContents("C1", f1);
            ss.SetCellContents("A1", f2);
            IEnumerable<string> ieb = ss.GetNamesOfAllNonemptyCells();
            IEnumerator<string> iet = ieb.GetEnumerator();
            iet.MoveNext();
            Assert.AreEqual("B1", iet.Current);
        }

        /// <summary>
        /// Simple test to get NONEMTPY
        /// </summary>
        [TestMethod]
        public void TestNamesOfAllNonemptyCells2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f2 = new Formula("E1+F1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", f);
            ss.SetCellContents("C1", f1);
            ss.SetCellContents("A1", f2);
            IEnumerable<string> ieb = ss.GetNamesOfAllNonemptyCells();
            IEnumerator<string> iet = ieb.GetEnumerator();
            iet.MoveNext();
            iet.MoveNext();
            Assert.AreEqual("C1", iet.Current);
        }

        /// <summary>
        /// Simple test to GetCellContents
        /// </summary>
        [TestMethod]
        public void TestGetCellContents1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula f = new Formula("B1+A1");
            Formula f2 = new Formula("E1+F1");
            Formula f1 = new Formula("A1*2");
            ss.SetCellContents("B1", 1);
            ss.SetCellContents("C1", f1);
            ss.SetCellContents("A1", f2);
            Object ob = ss.GetCellContents("B1");
            Assert.IsTrue(ob.Equals(1));
        }
    }
}
