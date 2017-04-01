using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControllerTester;
using SpreadsheetGUI;
using SSGui;

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

        [TestMethod]
        public void ControllerTestConvertCellName()
        {
            View1Stub stub = new View1Stub();
            Controller controller = new Controller(stub);
            PrivateObject obj = new PrivateObject(controller);
            object[] args = new object[2] {0,0};
            var a = obj.Invoke("ConvertCellName", args);
            var b = ("A1");
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void ControllerTestOpen()
        {
            View1Stub stub = new View1Stub();
            Controller controller = new Controller(stub);
            stub.OpenNew();
            Assert.IsTrue(stub.CalledOpenNew);
        }


        [TestMethod]
        public void ControllerTestFileChosen1()
        {
            View1Stub stub = new View1Stub();
            Controller controller = new Controller(stub);
            stub.FireFileChosenEvent();
            Assert.IsTrue(stub.CalledFileChosen);
        }

        [TestMethod]
        public void ControllerTestSelectionChanged()
        {
            View1Stub stub = new View1Stub();
            Controller controller = new Controller(stub);

          

        }

    }
}
