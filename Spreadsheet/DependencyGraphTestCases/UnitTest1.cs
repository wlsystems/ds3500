using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace DependencyGraphTestCases
{
    [TestClass]
    public class UnitTest1
    {
        DependencyGraph dg = new DependencyGraph();

        /// <summary>
        /// simple test to add a dependency
        /// </summary>

        [TestMethod]
        public void TestAddDependency1()
        {
            dg.AddDependency("s", "a");
            IEnumerable<string> s = dg.GetDependents("s");
            IEnumerator<string> d = s.GetEnumerator();
            d.MoveNext();
            Assert.AreEqual("a", d.Current);
        }

        ///
        /// add 2 dependencies to the same dependee check the second depenency 
        ///

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

        /// <summary>
        /// check 2 dependents 2 dependees
        /// </summary>
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

        /// <summary>
        /// check 2 dependents 2 dependees
        /// </summary>
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

        /// <summary>
        /// check 2 dependents with duplicate (should do nothing) and check count
        /// </summary>
        [TestMethod]
        public void TestAddDependency4()
        {
            dg.AddDependency("s", "a");
            dg.AddDependency("s", "a");
            IEnumerable<string> s = dg.GetDependents("a");
            IEnumerator<string> d = s.GetEnumerator();
            Assert.AreEqual(dg.Size, 1);
        }

        /// <summary>
        /// add null
        /// </summary>
        [TestMethod]
        public void TestAddDependency5()
        {
            dg.AddDependency(" ", "b");
            IEnumerable<string> s = dg.GetDependents(" ");
            IEnumerator<string> d = s.GetEnumerator();
            d.MoveNext();
            Assert.AreEqual("b", d.Current);
        }

        /// <summary>
        /// check 2 dependents with duplicate (should do nothing) and check count
        /// </summary>
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

        /// <summary>
        /// check 2 dependents with duplicate (should do nothing) and check count
        /// </summary>
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

        /// <summary>
        /// try to remove a dependent that is not there 
        /// </summary>
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

        /// <summary>
        /// try to remove a dependency that is not there 
        /// </summary>
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

        /// <summary>
        /// test if a dependency exists after it is removed
        /// </summary>
        [TestMethod]
        public void TestRemoveDependency5()
        {
            dg.AddDependency("s", "a");
            dg.RemoveDependency("s", "a");
            IEnumerable<string> s = dg.GetDependents("s");
            IEnumerator<string> d = s.GetEnumerator();
            Assert.AreEqual(dg.HasDependents("s"), false);
        }

        /// <summary>
        /// try to remove several dependents 
        /// </summary>
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

        /// <summary>
        /// try to remove several dependents and test size
        /// </summary>
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

        /// <summary>
        /// /try to remove several dependents 
        /// </summary>
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

        /// <summary>
        /// try to remove several dependents 
        /// </summary>
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

        /// <summary>
        /// try to remove several dependents
        /// </summary>
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

        /// <summary>
        /// try to remove several dependents 
        /// </summary>
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

        /// <summary>
        /// simple dependee test 
        /// </summary>
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

        /// <summary>
        /// test dependee count
        /// </summary>
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

        /// <summary>
        /// test the dependee 
        /// </summary>
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

        /// <summary>
        /// /test the dependee 
        /// </summary>
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

        /// <summary>
        /// test the dependents
        /// </summary>
        [TestMethod]
        public void TestHasDependents1()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependents("s"), true);

        }

        /// <summary>
        /// test the dependents
        /// </summary>
        [TestMethod]
        public void TestHasDependents2()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependents("a"), true);
        }

        /// <summary>
        /// test the dependents
        /// </summary>
        [TestMethod]
        public void TestHasDependents3()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependents("1"), false);
        }

        /// <summary>
        /// test the dependents
        /// </summary>
        [TestMethod]
        public void TestHasDependents4()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependents("3"), false);
        }

        /// <summary>
        /// test the dependeees
        /// </summary>
        [TestMethod]
        public void TestHasDependees1()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependees("3"), true);
        }

        /// <summary>
        /// test the dependeees
        /// </summary>
        [TestMethod]
        public void TestHasDependees2()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependees("1"), true);
        }

        /// <summary>
        /// test the dependeees
        /// </summary>
        [TestMethod]
        public void TestHasDependees3()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependees("s"), false);
        }

        /// <summary>
        /// test the dependeees
        /// </summary>
        [TestMethod]
        public void TestHasDependees4()
        {
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("a", "1");
            Assert.AreEqual(dg.HasDependees("a"), false);
        }

        /// <summary>
        /// test the dependee 
        /// </summary>
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

        /// <summary>
        /// test the dependee 
        /// </summary>
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

        /// <summary>
        /// test the dependee 
        /// </summary>
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

        /// <summary>
        /// test the dependee 
        /// </summary>
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

        /// <summary>
        /// test the dependee 
        /// </summary>
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
            Assert.AreEqual(dg.HasDependees("a"), false);
            Assert.AreEqual(dg.HasDependees("b"), false);
            Assert.AreEqual(dg.HasDependees("c"), false);
        }

        /// <summary>
        /// test the dependee size
        /// </summary>
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

        /// <summary>
        /// test the ReplaceDependees 
        /// </summary>
        [TestMethod]
        public void TestReplaceDependees7()
        {
            dg.AddDependency("a", "1");
            IEnumerable<string> s = dg.GetDependees("3");
            dg.ReplaceDependees("1", s);
        }

        /// <summary>
        /// test the ReplaceDependees
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependees8()
        {
            dg.AddDependency("a", "1");
            IEnumerable<string> s = dg.GetDependees("1");
            dg.ReplaceDependees(null, s);
        }

        /// <summary>
        /// test the ReplaceDependees to ensure exception is thrown if null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependees9()
        {
            dg.AddDependency("a", "1");
            IEnumerable<string> s = dg.GetDependees("");
            dg.ReplaceDependees(null, s);
        }

        /// <summary>
        /// Tests the constructor to see if the new object it created was equal to the original.
        /// </summary>
        [TestMethod()]
        public void StressTest20()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 400;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependents
            for (int i = 0; i < SIZE; i += 2)
            {
                HashSet<string> newDents = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 5)
                {
                    newDents.Add(letters[j]);
                }
                t.ReplaceDependents(letters[i], newDents);

                foreach (string s in dents[i])
                {
                    dees[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDents)
                {
                    dees[s[0] - 'a'].Add(letters[i]);
                }

                dents[i] = newDents;
            }
            DependencyGraph t2 = t;
            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                HashSet<string> x1 = new HashSet<string>(t.GetDependents(letters[i]));
                HashSet<string> x2 = new HashSet<string>(t2.GetDependents(letters[i]));
                for (int a = 0; a < x1.Count; a++)
                {
                    Assert.IsTrue(x1.ElementAt(a).Equals(x2.ElementAt(a)));
                }
            }
        }

        /// <summary>
        /// Tests the constructor to see if the new object it created was equal to the original after replacing some elements.
        /// </summary>
        [TestMethod()]
        public void StressTest21()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 400;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            DependencyGraph t2 = t;
            // Replace a bunch of dependents
            for (int i = 0; i < SIZE; i += 2)
            {
                HashSet<string> newDents = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 5)
                {
                    newDents.Add(letters[j]);
                }
                t.ReplaceDependents(letters[i], newDents);
                t2.ReplaceDependents(letters[i], newDents);
                foreach (string s in dents[i])
                {
                    dees[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDents)
                {
                    dees[s[0] - 'a'].Add(letters[i]);
                }

                dents[i] = newDents;
            }
            
            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                HashSet<string> x1 = new HashSet<string>(t.GetDependents(letters[i]));
                HashSet<string> x2 = new HashSet<string>(t2.GetDependents(letters[i]));
                for (int a = 0; a < x1.Count; a++)
                {
                    Assert.IsTrue(x1.ElementAt(a).Equals(x2.ElementAt(a)));
                }
            }
        }

        /// <summary>
        /// Tests the constructor to see if the new object it created was equal to the original after removing and replacing some elements.
        /// </summary>
        [TestMethod()]
        public void StressTest22()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 400;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            DependencyGraph t2 = t;
            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    t2.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            
            // Replace a bunch of dependents
            for (int i = 0; i < SIZE; i += 2)
            {
                HashSet<string> newDents = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 5)
                {
                    newDents.Add(letters[j]);
                }
                t.ReplaceDependents(letters[i], newDents);
                t2.ReplaceDependents(letters[i], newDents);
                foreach (string s in dents[i])
                {
                    dees[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDents)
                {
                    dees[s[0] - 'a'].Add(letters[i]);
                }

                dents[i] = newDents;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                HashSet<string> x1 = new HashSet<string>(t.GetDependents(letters[i]));
                HashSet<string> x2 = new HashSet<string>(t2.GetDependents(letters[i]));
                for (int a = 0; a < x1.Count; a++)
                {
                    Assert.IsTrue(x1.ElementAt(a).Equals(x2.ElementAt(a)));
                }
            }
        }

        /// <summary>
        /// Tests the constructor to see if the new object it created was equal to the original after adding, removingv and replacing some elements.
        /// </summary>
        [TestMethod()]
        public void StressTest23()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 400;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }
            DependencyGraph t2 = t;
            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    t2.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    t2.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependents
            for (int i = 0; i < SIZE; i += 2)
            {
                HashSet<string> newDents = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 5)
                {
                    newDents.Add(letters[j]);
                }
                t.ReplaceDependents(letters[i], newDents);
                t2.ReplaceDependents(letters[i], newDents);
                foreach (string s in dents[i])
                {
                    dees[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDents)
                {
                    dees[s[0] - 'a'].Add(letters[i]);
                }

                dents[i] = newDents;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                HashSet<string> x1 = new HashSet<string>(t.GetDependents(letters[i]));
                HashSet<string> x2 = new HashSet<string>(t2.GetDependents(letters[i]));
                for (int a = 0; a < x1.Count; a++)
                {
                    Assert.IsTrue(x1.ElementAt(a).Equals(x2.ElementAt(a)));
                }
            }
        }

        // ********************************** A THIRD STESS TEST, REPEATED ******************** //
        /// <summary>
        ///Using lots of data with replacement.  Tests 2 identical DG's to ensure the rules are followed.
        ///</summary>
        [TestMethod()]
        public void StressTest15()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            DependencyGraph t2 = t;
            // A bunch of strings to use
            const int SIZE = 800;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    t2.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    t2.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependees
            for (int i = 0; i < SIZE; i += 2)
            {
                HashSet<string> newDees = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 9)
                {
                    newDees.Add(letters[j]);
                }
                t.ReplaceDependees(letters[i], newDees);
                t2.ReplaceDependees(letters[i], newDees);
                foreach (string s in dees[i])
                {
                    dents[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDees)
                {
                    dents[s[0] - 'a'].Add(letters[i]);
                }

                dees[i] = newDees;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(new HashSet<string>(t.GetDependents(letters[i])).SetEquals(new HashSet<string>(t2.GetDependents(letters[i]))));
                Assert.IsTrue(new HashSet<string>(t.GetDependees(letters[i])).SetEquals(new HashSet<string>(t2.GetDependees(letters[i]))));
            }
        }

        // ********************************** A THIRD STESS TEST, REPEATED ******************** //
        /// <summary>
        ///Using lots of data with replacement.  Tests 2 identical DG's to ensure the rules are followed.
        ///</summary>
        [TestMethod()]
        public void StressTest24()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            // A bunch of strings to use
            const int SIZE = 800;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            DependencyGraph t2 = t;
            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    t2.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependees
            for (int i = 0; i < SIZE; i += 2)
            {
                HashSet<string> newDees = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 9)
                {
                    newDees.Add(letters[j]);
                }
                t.ReplaceDependees(letters[i], newDees);
                t2.ReplaceDependees(letters[i], newDees);
                foreach (string s in dees[i])
                {
                    dents[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDees)
                {
                    dents[s[0] - 'a'].Add(letters[i]);
                }

                dees[i] = newDees;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(new HashSet<string>(t.GetDependents(letters[i])).SetEquals(new HashSet<string>(t2.GetDependents(letters[i]))));
                Assert.IsTrue(new HashSet<string>(t.GetDependees(letters[i])).SetEquals(new HashSet<string>(t2.GetDependees(letters[i]))));
            }
        }

        /// <summary>
        /// test the ReplaceDependents
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestHasDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.HasDependents(null);
        }

        /// <summary>
        /// test the ReplaceDependees
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestHasDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.HasDependees(null);
        }

        /// <summary>
        /// test the GetDependents for null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("d", "f");
            IEnumerable<string> ieb =  t.GetDependents(null);
            foreach (string s in ieb)
                Console.WriteLine(s);
        }

        /// <summary>
        /// test the GetDependeees for null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("d", "f");
            IEnumerable<string> ieb = t.GetDependees(null);
            foreach (string s in ieb)
                Console.WriteLine(s);
        }

        /// <summary>
        /// test theAddDependency for null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddDependency()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency(null, "a");
        }

        /// <summary>
        /// test theAddDependency for null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddDependency10()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", null);
        }

        /// <summary>
        /// test theAddDependency for null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveDependency()
        {
            DependencyGraph t = new DependencyGraph();
            t.RemoveDependency("a", null);
        }

        /// <summary>
        /// test theAddDependency for null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveDependency10()
        {
            DependencyGraph t = new DependencyGraph();
            t.RemoveDependency( null,"a");
        }

        /// <summary>
        /// test theAddDependency for null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("d", "f");
            IEnumerable<string> ieb = t.GetDependents("d");
            t.ReplaceDependents(null, ieb);
        }

        /// <summary>
        /// test theAddDependency for null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDepees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("d", "f");
            IEnumerable<string> ieb = t.GetDependents("d");
            t.ReplaceDependees(null, ieb);
        }

        /// <summary>
        /// test Dependency for null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDependencyGraph()
        {
            DependencyGraph t = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph(null);
        }

        /// <summary>
        /// test Dependency 
        /// </summary>
        [TestMethod]
        public void TestDependencyGraph1()
        {
            DependencyGraph t = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph(t);
        }

        /// <summary>
        /// test ReplaceDependees  throw exception if null 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependees10()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("d", "f");
            t.ReplaceDependees("f", null);
        }

        /// <summary>
        /// test ReplaceDependents  throw exception if null 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependents10()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("d", "f");
            IEnumerable<string> ieb = t.GetDependents("d");
            t.ReplaceDependents("f", null);
        }

        /// <summary>
        /// test ReplaceDependents  throw exception if null element in IEnumerable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependents11()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("d", "f");
            HashSet<string> hs = new HashSet<string>();
            hs.Add("a");
            hs.Add(null);
            hs.Add("b");
            t.ReplaceDependents("d", hs);
        }

        /// <summary>
        /// test ReplaceDepees  throw exception if null element in IEnumerable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReplaceDependees11()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("d", "f");
            HashSet<string> hs = new HashSet<string>();
            hs.Add("a");
            hs.Add(null);
            hs.Add("b");
            t.ReplaceDependees("d", hs);
        }

    }
}
