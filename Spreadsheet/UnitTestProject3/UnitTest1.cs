﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using Formulas;
using System.Collections.Generic;

namespace UnitTestProject3
{
    using SS;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using Formulas;
    using System.Text.RegularExpressions;
    using System.IO;

    namespace GradingTests
    {
        /// <summary>
        /// These are grading tests for PS5
        ///</summary>
        [TestClass()]
        public class SpreadsheetTest
        {


            [TestMethod()]
            public void TestSave()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "1");
                TextWriter t = new StreamWriter("ss.xml");
                s.Save(t);
                Assert.IsFalse(s.Changed);
            }
            [TestMethod()]
            public void TestSave1()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                Assert.IsFalse(s.Changed);
            }
            [TestMethod()]
            public void TestChanged()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                
                Assert.IsFalse(s.Changed);
            }

            [TestMethod()]
            public void TestGetCellValue13()
            {
                int Sum = 0;
                AbstractSpreadsheet s = new Spreadsheet();
                for (int i = 1; i <= 500; i++)
                {
                    if (i == 100 || i == 200 || i == 300 || i == 400)
                    {
                        s.SetContentsOfCell("B" + i, "0");
                    }
                    else
                    {
                        s.SetContentsOfCell("B" + i, ("=B" + (i + 1)));
                        Sum = Sum + i;
                    }
                }

                s.SetContentsOfCell("B501", Sum.ToString());

                Assert.AreEqual("124250", Convert.ToString(s.GetCellValue("B501")));
                for (int i = 1; i <= 500; i++)
                {
                    if (i == 100 || i == 200 || i == 300 || i == 400)
                    {
                        Assert.AreEqual(0, Convert.ToInt32(s.GetCellValue("B" + i)));
                    }
                    else
                    {
                        Assert.AreNotEqual("0", Convert.ToInt32(s.GetCellValue("B" + i)));

                    }
                }
            }

            [TestMethod()]
            public void TestGetCellValue10()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                for (int i = 1; i <= 500; i++)
                {
                    int m = i * 2;
                    s.SetContentsOfCell("E" + i, m.ToString());
                }

                for (int i = 1; i <= 500; i++)
                {
                    Random rand = new Random();
                    int no = rand.Next(1, 500);
                    Assert.AreEqual(0, Convert.ToInt32(s.GetCellValue("E" + no)) % 2);
                }
            }

            [TestMethod()]
            public void TestGetCellValue11()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                for (int i = 1; i <= 500; i++)
                {
                    s.SetContentsOfCell("B" + i, ("=B" + (i + 1)));
                }

                ISet<string> sss = s.SetContentsOfCell("B250", "25.0");
                for (int i = 1; i <= 500; i++)
                {
                    Random rand = new Random();
                    int num = rand.Next(1, 250);
                    Assert.IsTrue(sss.Contains("B" + num));
                }
            }



            [TestMethod()]
            public void TestGetCellValue15()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", ("=E1"));
                s.SetContentsOfCell("B2", ("=E1"));
                s.SetContentsOfCell("C3", ("=E1"));
                s.SetContentsOfCell("D4", ("=E1"));
                s.SetContentsOfCell("E5", ("=E1"));
                ISet<String> cells = s.SetContentsOfCell("F1", "0");
                AssertSetEqualsIgnoreCase(new HashSet<string> {  "F1" }, cells);
            }

            [TestMethod()]
            public void TestGetCellValue14()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                for (int i = 1; i < 500; i++)
                {
                    int val = i + 1;
                    s.SetContentsOfCell("E" + i, val.ToString());

                    s.SetContentsOfCell("F" + i, val.ToString());
                }

                for (int i = 1; i <= 6; i++)
                {
                    Random rand = new Random();
                    int num = rand.Next(1, 250);

                    Assert.AreEqual(Convert.ToString(s.GetCellValue("E" + num)), Convert.ToString(s.GetCellValue("F" + num)));
                }
            }
            
            /// <summary>
            /// set path to null
            /// </summary>
            [ExpectedException(typeof(IOException))]
            [TestMethod()]
            public void TestSpreadsheet1()
            {
                TextReader source = null;
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*");
                AbstractSpreadsheet s = new Spreadsheet(source, rg1);
            }

            /// <summary>
            /// set path to jpg
            /// </summary>
            [TestMethod()]
            public void TestSpreadsheet3a()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*");
                AbstractSpreadsheet s = new Spreadsheet(rg1);
            }

            /// <summary>
            /// 
            /// write 3 lines to xml
            /// </summary>
            [TestMethod()]
            public void TestSS1()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("W2", "=E2+C2");
                s.SetContentsOfCell("E2", "=D2+3");
                s.SetContentsOfCell("D2", "=C2");
                TextWriter t = new StreamWriter("ss.xml");
                s.Save(t);
                Assert.IsFalse(s.Changed);
            }
            /// <summary>
            /// 
            /// added duplicate entry to source 
            /// </summary>
            [TestMethod()]
            public void TestSS2()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*");
                TextReader t = new StreamReader("ss.xml");
                AbstractSpreadsheet s = new Spreadsheet(t,rg1);
            }

            /// <summary>
            /// Test and invalid formatted xml
            /// </summary>
            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadException))]
            public void TestSS3()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*");
                TextReader t = new StreamReader("states3.xml");
                AbstractSpreadsheet s = new Spreadsheet(t, rg1);
            }

            /// <summary>
            /// Test the GetCellValues
            /// </summary>
            [TestMethod()]
            [ExpectedException(typeof(SpreadsheetReadException))]
            public void TestSSGetCellValues1()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*");
                TextReader t = new StreamReader("s12.xml");
                AbstractSpreadsheet s = new Spreadsheet(t, rg1);
            }

            /// <summary>
            /// Used to make assertions about set equality.  Everything is converted first to
            /// upper case.
            /// </summary>
            public static void AssertSetEqualsIgnoreCase(IEnumerable<string> s1, IEnumerable<string> s2)
            {
                var set1 = new HashSet<String>();
                foreach (string s in s1)
                {
                    set1.Add(s.ToUpper());
                }

                var set2 = new HashSet<String>();
                foreach (string s in s2)
                {
                    set2.Add(s.ToUpper());
                }

                Assert.IsTrue(new HashSet<string>(set1).SetEquals(set2));
            }
            /// <summary>
            /// test getcellvalues
            /// </summary>
            [TestMethod()]
            public void Test1a()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("W2", "=E2+C2");
                s.SetContentsOfCell("E2", "=D2+3");
                s.SetContentsOfCell("D2", "=C2");
                ISet<string> i = s.SetContentsOfCell("C2", "2");
                Assert.AreEqual(7.0, s.GetCellValue("W2"));
            }

            /// <summary>
            /// test getcellvalues
            /// </summary>
            [TestMethod()]
            public void Test1b()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("W2", "=E2+C2");
                s.SetContentsOfCell("E2", "=D2+3");
                s.SetContentsOfCell("D2", "=C2");
                ISet<string> i = s.SetContentsOfCell("C2", "2");
                Assert.AreEqual(2.0, s.GetCellValue("D2"));
            }

            /// <summary>
            /// test getcellvalues
            /// </summary>
            [TestMethod()]
            public void Test1c()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("W2", "=E2+C2");
                s.SetContentsOfCell("E2", "=D2+3");
                s.SetContentsOfCell("D2", "=C2");
                ISet<string> i = s.SetContentsOfCell("C2", "2");
                Assert.AreEqual(5.0, s.GetCellValue("E2"));
            }


            // EMPTY SPREADSHEETS
            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void Test1()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.GetCellContents(null);
            }

            // EMPTY SPREADSHEETS
            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void Test2a()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*$");
                AbstractSpreadsheet s = new Spreadsheet(rg1);
                s.GetCellContents(null);
            }

            // EMPTY SPREADSHEETS
            [TestMethod()]
            public void Test2b()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*$");
                AbstractSpreadsheet s = new Spreadsheet(rg1);
                ISet<string> i = s.SetContentsOfCell("C2", "2");
                Assert.AreEqual(2.0, s.GetCellValue("C2"));
            }

            // EMPTY SPREADSHEETS
            [TestMethod()]
            public void Test2c()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*$");
                AbstractSpreadsheet s = new Spreadsheet(rg1);
                ISet<string> i = s.SetContentsOfCell("C2", "2a");
                Assert.AreEqual("2a", s.GetCellValue("C2"));
            }

            // EMPTY SPREADSHEETS
            [ExpectedException(typeof(ArgumentNullException))]
            [TestMethod()]
            public void Test2d()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*$");
                AbstractSpreadsheet s = new Spreadsheet(rg1);
                ISet<string> i = s.SetContentsOfCell("C2", null);
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void Test2()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*$");
                AbstractSpreadsheet s = new Spreadsheet(rg1);
                s.SetContentsOfCell("AA", null);
            }

            [TestMethod()]
            public void Test3()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                Assert.AreEqual("", s.GetCellContents("A2"));
            }

            // SETTING CELL TO A DOUBLE
            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void Test4()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell(null, "1.5");
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void Test5()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*$");
                AbstractSpreadsheet s = new Spreadsheet(rg1);
                s.SetContentsOfCell("A1A", "1.5");
            }

            [TestMethod()]
            public void Test6()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("Z7", "1.5");
                Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
            }

            // SETTING CELL TO A STRING
            [TestMethod()]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Test7()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A8", (string)null);
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void Test8()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell(null, "hello");
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void Test9()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*");
                AbstractSpreadsheet s = new Spreadsheet(rg1);
                
                s.SetContentsOfCell("AZ", "hello");
            }

            [TestMethod()]
            public void Test10()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("Z7", "hello");
                Assert.AreEqual("hello", s.GetCellContents("Z7"));
            }

            // SETTING CELL TO A FORMULA
            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void Test11()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell(null, ("=2"));
            }

            [TestMethod()]
            [ExpectedException(typeof(InvalidNameException))]
            public void Test12()
            {
                Regex rg1 = new Regex(@"[A-Z]*[1-9][0-9]*");
                AbstractSpreadsheet s = new Spreadsheet(rg1);
                s.SetContentsOfCell("AZ", ("=2"));
            }

            [TestMethod()]
            public void Test13()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("Z7", ("=3"));
                Formula f = (Formula)s.GetCellContents("Z7");
                Assert.AreEqual(3, f.Evaluate(x => 0), 1e-6);
            }

            // CIRCULAR FORMULA DETECTION
            [TestMethod()]
            [ExpectedException(typeof(CircularException))]
            public void Test14()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", ("=A2"));
                s.SetContentsOfCell("A2", ("=A1"));
            }

            [TestMethod()]
            [ExpectedException(typeof(CircularException))]
            public void Test15()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", ("=A2+A3"));
                s.SetContentsOfCell("A3", ("=A4+A5"));
                s.SetContentsOfCell("A5", ("=A6+A7"));
                s.SetContentsOfCell("A7", ("=A1+A1"));
            }

            [TestMethod()]
            [ExpectedException(typeof(CircularException))]
            public void Test16()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                try
                {
                    s.SetContentsOfCell("A1", ("=A2+A3"));
                    s.SetContentsOfCell("A2", "15");
                    s.SetContentsOfCell("A3", "30");
                    s.SetContentsOfCell("A2", ("=A3*A1"));
                }
                catch (CircularException e)
                {
                    //Assert.AreEqual(15, (double)s.GetCellContents("A2"), 1e-9);
                    throw e;
                }
            }

            // NONEMPTY CELLS
            [TestMethod()]
            public void Test17()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
            }

            [TestMethod()]
            public void Test18()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "");
                Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
            }

            [TestMethod()]
            public void Test19()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("C2", "hello");
                s.SetContentsOfCell("C2", "");
                Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
            }

            [TestMethod()]
            public void Test20()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "hello");
                AssertSetEqualsIgnoreCase(s.GetNamesOfAllNonemptyCells(), new string[] { "B1" });
            }

            [TestMethod()]
            public void Test21()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "52.25");
                AssertSetEqualsIgnoreCase(s.GetNamesOfAllNonemptyCells(), new string[] { "B1" });
            }

            [TestMethod()]
            public void Test22()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", ("=3.5"));
                AssertSetEqualsIgnoreCase(s.GetNamesOfAllNonemptyCells(), new string[] { "B1" });
            }

            [TestMethod()]
            public void Test23()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "17.2");
                s.SetContentsOfCell("C1", "hello");
                s.SetContentsOfCell("B1", ("=3.5"));
                AssertSetEqualsIgnoreCase(s.GetNamesOfAllNonemptyCells(), new string[] { "A1", "B1", "C1" });
            }

            // RETURN VALUE OF SET CELL CONTENTS
            [TestMethod()]
            public void Test24()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("B1", "hello");
                s.SetContentsOfCell("C1", ("=5"));
                AssertSetEqualsIgnoreCase(s.SetContentsOfCell("A1", "17.2"), new string[] { "A1" });
            }

            [TestMethod()]
            public void Test25()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "17.2");
                s.SetContentsOfCell("C1", ("=5"));
                AssertSetEqualsIgnoreCase(s.SetContentsOfCell("B1", "hello"), new string[] { "B1" });
            }

            [TestMethod()]
            public void Test26()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "17.2");
                s.SetContentsOfCell("B1", "hello");
                AssertSetEqualsIgnoreCase(s.SetContentsOfCell("C1", ("5")), new string[] { "C1" });
            }

            [TestMethod()]
            public void Test27()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", ("=A2+A3"));
                s.SetContentsOfCell("A2", "6");
                s.SetContentsOfCell("A3", ("=A2+A4"));
                s.SetContentsOfCell("A4", ("=A2+A5"));
                HashSet<string> result = new HashSet<string>(s.SetContentsOfCell("A5", "82.5"));
                AssertSetEqualsIgnoreCase(result, new string[] { "A5", "A4", "A3", "A1" });
            }

            // CHANGING CELLS
            [TestMethod()]
            public void Test28()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", ("=A2+A3"));
                s.SetContentsOfCell("A1", "2.5");
                Assert.AreEqual(2.5, (double)s.GetCellContents("A1"), 1e-9);
            }

            [TestMethod()]
            public void Test29()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", ("=A2+A3"));
                s.SetContentsOfCell("A1", "Hello");
                Assert.AreEqual("Hello", (string)s.GetCellContents("A1"));
            }

            [TestMethod()]
            public void Test30()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", "Hello");
                s.SetContentsOfCell("A1",  ("=23"));
                Assert.AreEqual(23, ((Formula)s.GetCellContents("A1")).Evaluate(x => 0));
            }

            // STRESS TESTS
            [TestMethod()]
            public void Test31()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("A1", ("=B1+B2"));
                s.SetContentsOfCell("B1",  ("=C1-C2"));
                s.SetContentsOfCell("B2",  ("=C3*C4"));
                s.SetContentsOfCell("C1",  ("=D1*D2"));
                s.SetContentsOfCell("C2",  ("=D3*D4"));
                s.SetContentsOfCell("C3",  ("=D5*D6"));
                s.SetContentsOfCell("C4",  ("=D7*D8"));
                s.SetContentsOfCell("D1",  ("=E1"));
                s.SetContentsOfCell("D2",  ("=E1"));
                s.SetContentsOfCell("D3",  ("=E1"));
                s.SetContentsOfCell("D4",  ("=E1"));
                s.SetContentsOfCell("D5",  ("=E1"));
                s.SetContentsOfCell("D6",  ("=E1"));
                s.SetContentsOfCell("D7",  ("=E1"));
                s.SetContentsOfCell("D8",  ("=E1"));
                ISet<String> cells = s.SetContentsOfCell("E1", "0");
                AssertSetEqualsIgnoreCase(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }, cells);
            }
            [TestMethod()]
            public void Test32()
            {
                Test31();
            }
            [TestMethod()]
            public void Test33()
            {
                Test31();
            }
            [TestMethod()]
            public void Test34()
            {
                Test31();
            }

            [TestMethod()]
            public void Test35()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                ISet<String> cells = new HashSet<string>();
                for (int i = 1; i < 200; i++)
                {
                    cells.Add("A" + i);
                    AssertSetEqualsIgnoreCase(cells, s.SetContentsOfCell("A" + i,  ("=A" + (i + 1))));
                }
            }
            [TestMethod()]
            public void Test36()
            {
                Test35();
            }
            [TestMethod()]
            public void Test37()
            {
                Test35();
            }
            [TestMethod()]
            public void Test38()
            {
                Test35();
            }
            [TestMethod()]
            public void Test39()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                for (int i = 1; i < 200; i++)
                {
                    s.SetContentsOfCell("A" + i,  ("=A" + (i + 1)));
                }
                try
                {
                    s.SetContentsOfCell("A150",  ("=A50"));
                    Assert.Fail();
                }
                catch (CircularException)
                {
                }
            }
            [TestMethod()]
            public void Test40()
            {
                Test39();
            }
            [TestMethod()]
            public void Test41()
            {
                Test39();
            }
            [TestMethod()]
            public void Test42()
            {
                Test39();
            }

            [TestMethod()]
            public void Test43()
            {
                AbstractSpreadsheet s = new Spreadsheet();
                for (int i = 0; i < 500; i++)
                {
                    s.SetContentsOfCell("A1" + i,  ("=A1" + (i + 1)));
                }

                ISet<string> sss = s.SetContentsOfCell("A1499", "25.0");
                Assert.AreEqual(500, sss.Count);
                for (int i = 0; i < 500; i++)
                {
                    Assert.IsTrue(sss.Contains("A1" + i) || sss.Contains("a1" + i));
                }

                sss = s.SetContentsOfCell("A1249", "25.0");
                Assert.AreEqual(250, sss.Count);
                for (int i = 0; i < 250; i++)
                {
                    Assert.IsTrue(sss.Contains("A1" + i) || sss.Contains("a1" + i));
                }


            }

            [TestMethod()]
            public void Test44()
            {
                Test43();
            }
            [TestMethod()]
            public void Test45()
            {
                Test43();
            }
            [TestMethod()]
            public void Test46()
            {
                Test43();
            }

            [TestMethod()]
            public void Test47()
            {
                RunRandomizedTest(47, 2519);
            }
            [TestMethod()]
            public void Test48()
            {
                RunRandomizedTest(48, 2521);
            }
            [TestMethod()]
            public void Test49()
            {
                RunRandomizedTest(49, 2526);
            }
            [TestMethod()]
            public void Test50()
            {
                RunRandomizedTest(50, 2521);
            }

            public void RunRandomizedTest(int seed, int size)
            {
                AbstractSpreadsheet s = new Spreadsheet();
                Random rand = new Random(seed);
                for (int i = 0; i < 10000; i++)
                {
                    try
                    {
                        switch (rand.Next(3))
                        {
                            case 0:
                                s.SetContentsOfCell(randomName(rand), "3.14");
                                break;
                            case 1:
                                s.SetContentsOfCell(randomName(rand), "hello");
                                break;
                            case 2:
                                s.SetContentsOfCell(randomName(rand), randomFormula(rand));
                                break;
                        }
                    }
                    catch (CircularException)
                    {
                    }
                }
                ISet<string> set = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
                Assert.AreEqual(size, set.Count);
            }

            private String randomName(Random rand)
            {
                return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(rand.Next(26), 1) + (rand.Next(99) + 1);
            }

            private String randomFormula(Random rand)
            {
                String f = randomName(rand);
                for (int i = 0; i < 10; i++)
                {
                    switch (rand.Next(4))
                    {
                        case 0:
                            f += "+";
                            break;
                        case 1:
                            f += "-";
                            break;
                        case 2:
                            f += "*";
                            break;
                        case 3:
                            f += "/";
                            break;
                    }
                    switch (rand.Next(2))
                    {
                        case 0:
                            f += 7.2;
                            break;
                        case 1:
                            f += randomName(rand);
                            break;
                    }
                }
                return f;
            }

        }
    }
}