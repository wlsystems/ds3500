using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Collections.Generic;

namespace DependencyGraphTestCases
{
    [TestClass]
    public class UnitTest1
    {
        DependencyGraph dg = new DependencyGraph();

        //simple test to add a dependency
        [TestMethod]
        public void TestAddDependency1()
        {
            dg.AddDependency("s", "a");
            IEnumerable<string> s = dg.GetDependents("s");
            IEnumerator<string> d = s.GetEnumerator();
            d.MoveNext();
            Assert.AreEqual("a", d.Current);
        }

        //add 2 dependencies to the same dependee check the second depenency 
        [TestMethod]
        public void TestAddDependency2()
        {
            dg.AddDependency("s", "a");
            dg.AddDependency("s", "b");
            IEnumerable<string> s = dg.GetDependents("s");
            IEnumerator<string> d = s.GetEnumerator();
            d.MoveNext();
            d.MoveNext();
            Assert.AreEqual("b", d.Current);
        }

        //check 2 dependents 2 dependees
        [TestMethod]
        public void TestAddDependency3()
        {
            dg.AddDependency("s", "a");
            dg.AddDependency("a", "b");
            IEnumerable<string> s = dg.GetDependents("a");
            IEnumerator<string> d = s.GetEnumerator();
            d.MoveNext();
            Assert.AreEqual("b", d.Current);
        }

        //check 2 dependents 2 dependees
        [TestMethod]
        public void TestAddDependency3b()
        {
            dg.AddDependency("s", "a");
            dg.AddDependency("a", "b");
            IEnumerable<string> s = dg.GetDependents("s");
            IEnumerator<string> d = s.GetEnumerator();
            d.MoveNext();
            Assert.AreEqual("a", d.Current);
        }

        //check 2 dependents with duplicate (should do nothing) and check count
        [TestMethod]
        public void TestAddDependency4()
        {
            dg.AddDependency("s", "a");
            dg.AddDependency("s", "a");
            IEnumerable<string> s = dg.GetDependents("a");
            IEnumerator<string> d = s.GetEnumerator();
            Assert.AreEqual(dg.Size, 1);
        }

        //add null
        [TestMethod]
        public void TestAddDependency5()
        {
            dg.AddDependency(" ", "b");
            IEnumerable<string> s = dg.GetDependents(" ");
            IEnumerator<string> d = s.GetEnumerator();
            d.MoveNext();
            Assert.AreEqual("b", d.Current);
        }

        //check 2 dependents with duplicate (should do nothing) and check count
        [TestMethod]
        public void TestRemoveDependency1()
        {
            dg.AddDependency("s", "a");
            dg.AddDependency("s", "1");
            dg.RemoveDependency("s", "a");
            IEnumerable<string> s = dg.GetDependents("s");
            IEnumerator<string> d = s.GetEnumerator();
            Assert.AreEqual(dg.Size, 1);
            d.MoveNext();
            Assert.AreEqual("1", d.Current);
        }

        //check 2 dependents with duplicate (should do nothing) and check count
        [TestMethod]
        public void TestRemoveDependency2()
        {
            dg.AddDependency("s", "a");
            dg.AddDependency("s", "1");
            dg.RemoveDependency("s", "a");
            IEnumerable<string> s = dg.GetDependents("s");
            IEnumerator<string> d = s.GetEnumerator();
            Assert.AreEqual(dg.Size, 1);
            d.MoveNext();
            Assert.AreEqual("1", d.Current);
        }

        //try to remove a dependent that is not there 
        [TestMethod]
        public void TestRemoveDependency3()
        {
            dg.AddDependency("s", "a");
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "d");
            dg.RemoveDependency("s", "x");
            IEnumerable<string> s = dg.GetDependents("s");
            IEnumerator<string> d = s.GetEnumerator();
            Assert.AreEqual(dg.Size, 3);
        }

        //try to remove a dependency that is not there 
        [TestMethod]
        public void TestRemoveDependency4()
        {
            dg.AddDependency("s", "a");
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "d");
            dg.RemoveDependency("x", "x");
            IEnumerable<string> s = dg.GetDependents("s");
            IEnumerator<string> d = s.GetEnumerator();
            Assert.AreEqual(dg.Size, 3);
        }

        //test if a dependency exists after it is removed
        [TestMethod]
        public void TestRemoveDependency5()
        {
            dg.AddDependency("s", "a");
            dg.RemoveDependency("s", "a");
            IEnumerable<string> s = dg.GetDependents("s");
            IEnumerator<string> d = s.GetEnumerator();
            Assert.AreEqual(dg.HasDependents("s"), false);
        }

        //try to remove several dependents 
        [TestMethod]
        public void TestReplaceDependents1()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("s", "5");
            IEnumerable<string> s = dg.GetDependents("s");
            dg.AddDependency("a", "2");
            dg.AddDependency("a", "4");
            dg.AddDependency("a", "6");
            dg.ReplaceDependents("a", s);
            IEnumerable<string> a = dg.GetDependents("a");
            IEnumerator<string> aET = a.GetEnumerator();
            aET.MoveNext();
            Assert.AreEqual(aET.Current, "1");
        }

        //try to remove several dependents and test size
        [TestMethod]
        public void TestReplaceDependents2()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("s", "5");
            IEnumerable<string> s = dg.GetDependents("s");
            dg.AddDependency("a", "2");
            dg.AddDependency("a", "4");
            dg.AddDependency("a", "6");
            dg.ReplaceDependents("a", s);
            Assert.AreEqual(dg.Size, 6);
        }

        //try to remove several dependents 
        [TestMethod]
        public void TestReplaceDependents3()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("s", "5");
            IEnumerable<string> s = dg.GetDependents("s");
            dg.AddDependency("a", "2");
            dg.AddDependency("a", "4");
            dg.AddDependency("a", "6");
            dg.ReplaceDependents("a", s);
            Assert.AreEqual(dg.HasDependents("a"), true);
        }

        //try to remove several dependents 
        [TestMethod]
        public void TestReplaceDependents4()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("s", "5");
            IEnumerable<string> s = dg.GetDependents("s");
            dg.AddDependency("a", "2");
            dg.AddDependency("a", "4");
            dg.AddDependency("a", "6");
            dg.ReplaceDependents("a", s);
            Assert.AreEqual(dg.HasDependents("s"), true);
        }

        //try to remove several dependents
        [TestMethod]
        public void TestReplaceDependents5()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("s", "5");
            IEnumerable<string> s = dg.GetDependents("s");
            dg.AddDependency("a", "2");
            dg.AddDependency("a", "4");
            dg.AddDependency("a", "6");
            dg.ReplaceDependents("a", s);
            Assert.AreEqual(dg.HasDependees("1"), true);
        }

        //try to remove several dependents 
        [TestMethod]
        public void TestReplaceDependents6()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("s", "5");
            IEnumerable<string> s = dg.GetDependents("s");
            dg.AddDependency("a", "2");
            dg.AddDependency("a", "4");
            dg.AddDependency("a", "6");
            dg.ReplaceDependents("a", s);
            Assert.AreEqual(dg.HasDependees("6"), false);
        }

        //simple dependee test 
        [TestMethod]
        public void TestGetDependees1()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            IEnumerable<string> s1 = dg.GetDependees("1");
            IEnumerator<string> aET = s1.GetEnumerator();
            aET.MoveNext();
            Assert.AreEqual(aET.Current, "s");
        }

        //test dependee count
        [TestMethod]
        public void TestGetDependees2()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            //dg.AddDependency("a", "1");
            IEnumerable<string> s1 = dg.GetDependees("1");
            IEnumerator<string> aET = s1.GetEnumerator();
            aET.MoveNext();
            Assert.AreEqual(aET.Current, "s");
        }

        //test the dependee 
        [TestMethod]
        public void TestGetDependees3()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            IEnumerable<string> s1 = dg.GetDependees("1");
            IEnumerator<string> aET = s1.GetEnumerator();
            aET.MoveNext();
            Assert.AreEqual(aET.Current, "s");
        }

        //test the dependee 
        [TestMethod]
        public void TestGetDependees4()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            IEnumerable<string> s1 = dg.GetDependees("1");
            IEnumerator<string> aET = s1.GetEnumerator();
            aET.MoveNext();
            aET.MoveNext();
            Assert.AreEqual(aET.Current, "a");
        }

        //test the dependents
        [TestMethod]
        public void TestHasDependents1()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependents("s"), true);

        }

        //test the dependents
        [TestMethod]
        public void TestHasDependents2()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependents("a"), true);
        }

        //test the dependents
        [TestMethod]
        public void TestHasDependents3()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependents("1"), false);
        }

        //test the dependents
        [TestMethod]
        public void TestHasDependents4()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependents("3"), false);
        }

        //test the dependeees
        [TestMethod]
        public void TestHasDependees1()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependees("3"), true);
        }

        //test the dependeees
        [TestMethod]
        public void TestHasDependees2()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependees("1"), true);
        }

        //test the dependeees
        [TestMethod]
        public void TestHasDependees3()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependees("s"), false);
        }

        //test the dependeees
        [TestMethod]
        public void TestHasDependees4()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependees("a"), false);
        }

        //test the dependee 
        [TestMethod]
        public void TestReplaceDependees1()
        {
            dg.AddDependency("a", "1");
            dg.AddDependency("b", "1");
            dg.AddDependency("c", "1");
            IEnumerable<string> s = dg.GetDependees("1");
            dg.AddDependency("d", "2");
            dg.AddDependency("e", "2");
            dg.AddDependency("f", "2");
            dg.ReplaceDependees("2", s);
            Assert.AreEqual(dg.HasDependents("d"), false);
        }

        //test the dependee 
        [TestMethod]
        public void TestReplaceDependees2()
        {
            dg.AddDependency("a", "1");
            dg.AddDependency("b", "1");
            dg.AddDependency("c", "1");
            IEnumerable<string> s = dg.GetDependees("1");
            dg.AddDependency("d", "2");
            dg.AddDependency("e", "2");
            dg.AddDependency("f", "2");
            dg.ReplaceDependees("2", s);
            Assert.AreEqual(dg.HasDependents("e"), false);
        }

        //test the dependee 
        [TestMethod]
        public void TestReplaceDependees3()
        {
            dg.AddDependency("a", "1");
            dg.AddDependency("b", "1");
            dg.AddDependency("c", "1");
            IEnumerable<string> s = dg.GetDependees("1");
            dg.AddDependency("d", "2");
            dg.AddDependency("e", "2");
            dg.AddDependency("f", "2");
            dg.ReplaceDependees("2", s);
            Assert.AreEqual(dg.HasDependents("f"), false);
        }

        //test the dependee 
        [TestMethod]
        public void TestReplaceDependees4()
        {
            dg.AddDependency("a", "1");
            dg.AddDependency("b", "1");
            dg.AddDependency("c", "1");
            IEnumerable<string> s = dg.GetDependees("1");
            dg.AddDependency("d", "2");
            dg.AddDependency("e", "2");
            dg.AddDependency("f", "2");
            dg.ReplaceDependees("2", s);
            Assert.AreEqual(dg.HasDependents("a"), true);
            Assert.AreEqual(dg.HasDependents("b"), true);
            Assert.AreEqual(dg.HasDependents("c"), true);
        }

        //test the dependee 
        [TestMethod]
        public void TestReplaceDependees5()
        {
            dg.AddDependency("a", "1");
            dg.AddDependency("b", "1");
            dg.AddDependency("c", "1");
            IEnumerable<string> s = dg.GetDependees("1");
            dg.AddDependency("d", "2");
            dg.AddDependency("e", "2");
            dg.AddDependency("f", "2");
            dg.ReplaceDependees("2", s);
            Assert.AreEqual(dg.HasDependees("a"), true);
            Assert.AreEqual(dg.HasDependees("b"), true);
            Assert.AreEqual(dg.HasDependees("c"), true);
        }

            //test the dependee size
        [TestMethod]
        public void TestReplaceDependees6()
        {
            dg.AddDependency("a", "1");
            dg.AddDependency("b", "1");
            dg.AddDependency("c", "1");
            IEnumerable<string> s = dg.GetDependees("1");
            dg.AddDependency("d", "2");
            dg.AddDependency("e", "2");
            dg.AddDependency("f", "2");
            dg.ReplaceDependees("2", s);
            Assert.AreEqual(dg.Size, 6);
        }
    }
}
