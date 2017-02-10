using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Collections.Generic;

namespace UnitTestProject1
{

    [TestClass]
    public class GradingTests
    {
        /// <summary>
        /// Tests of syntax errors detected by the constructor
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFormula1()
        {
            Formula f = new Formula(null);
        }

        /// <summary>
        /// Tests of syntax errors detected by the constructor 2
        /// </summary>

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFormula2()
        {
            Formula f = new Formula(null, Lookup2, Lookup3);
        }

        /// <summary>
        /// Tests of syntax errors detected by the constructor 2
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFormula3()
        {
            Formula f = new Formula("1", null, Lookup3);
        }

        /// <summary>
        /// Tests of syntax errors detected by the constructor 2
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFormula4()
        {
            Formula f = new Formula("1", Lookup2, null);
        }

        /// <summary>
        /// Tests of syntax errors detected by the normalizer
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNormalizer1()
        {
            Formula f = new Formula("X", Lookup2, Lookup3);
        }

        /// <summary>
        /// Tests of syntax errors detected by the normalizer
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNormalizer2()
        {
            Formula f = new Formula("", Lookup2, Lookup3);
        }

        /// <summary>
        /// Tests of syntax errors detected by the validator
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidator1()
        {
            Formula f = new Formula("y", Lookup2, Lookup3);
        }

        /// <summary>
        /// Simple test by GetVariables()
        /// </summary>
        [TestMethod()]
        public void TestGetVariables1()
        {
            Formula f = new Formula("x", Lookup2, Lookup3);
            ISet <string> iss = f.GetVariables();
        }

        /// <summary>
        /// Simple test by GetVariables()
        /// </summary>
        [TestMethod()]
        public void TestGetVariables2()
        {
            Formula f = new Formula("x", Lookup2, Lookup3);
            ISet<string> iss = f.GetVariables();
            Assert.IsTrue(iss.Count == 1);
        }

        /// <summary>
        /// Simple test by GetVariables()
        /// </summary>
        [TestMethod()]
        public void TestGetVariables3()
        {
            Formula f = new Formula("x", Lookup2, Lookup3);
            ISet<string> iss = f.GetVariables();
            Assert.IsTrue(iss.Contains("X"));
        }

        /// <summary>
        /// Simple test by GetVariables()
        /// </summary>
        [TestMethod()]
        public void TestToString1()
        {
            Formula f = new Formula("x + x", Lookup2, Lookup3);
            Assert.AreEqual(f.ToString(), ("X+X"));
        }

        /// <summary>
        /// Test zero parameter constructor
        /// </summary>
        [TestMethod()]
        public void TestFormula5()
        {
            Formula f = new Formula();
            Assert.AreEqual(f.Evaluate(Lookup4), (0));
        }

        public double Lookup4(String v)
        {
            switch (v)
            {
                case "X": return 4.0;
                case "Y": return 6.0;
                case "Z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }
        public string Lookup2(String v)
        {
            switch (v)
            {
                case "x": return "X";
                case "y": return "Y";
                case "z": return "Z";
                default: throw new UndefinedVariableException(v);
            }
        }

        public bool Lookup3(String v)
        {
            if (v.Equals("X"))
                return true;
            else return false;
        }
    }
}