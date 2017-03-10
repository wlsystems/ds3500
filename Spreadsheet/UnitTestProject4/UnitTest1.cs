using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControllerTester;
using SpreadsheetGUI;

namespace UnitTestProject4
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ControllerTestMethod1()
        {
            View1Stub stub = new View1Stub();
            Controller controller = new Controller(stub);
            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledDoClose);
        }
    }
}
