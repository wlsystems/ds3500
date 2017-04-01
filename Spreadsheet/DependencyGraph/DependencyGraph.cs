//Tracy King
//u0040235

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<string, List<string>> dependents;
        private Dictionary<string, List<string>> dependees;


        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, List<string>>();
            dependees = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Creates a DependencyGraph that is a copy of an exisitng dependency. 
        /// </summary>
        public DependencyGraph(DependencyGraph dg1)
        {
            dependents = new Dictionary<string, List<string>>();
            dependees = new Dictionary<string, List<string>>();
            foreach (var kvp in dg1.dependents)
            {
                string dep = kvp.Key;
                foreach (var dee in dg1.dependents[kvp.Key].ToList())
                {
                    AddDependency(dep, dee);           //the AddDependency will add the dependency both ways
                }

            }

        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.  This is a property.
        /// </summary>
        public int Size
        {
            get
            {
                int total = 0;
                foreach (var kvp in dependents)
                {
                    total += dependents[kvp.Key].Count;
                }
                return total;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.  If is null
        /// throw a ArgumentNullException. 
        /// </summary>
        public bool HasDependents(string s)
        {
            if (s == null)
                throw new ArgumentNullException("The key was null.");
            if (dependents.ContainsKey(s))
            {
                return dependents[s].Any();     //returns if any dependent are found
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null. If s is null throws a 
        /// ArgumentNullException. 
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
                throw new ArgumentNullException("The key was null.");

            if (dependees.ContainsKey(s))
            {
                return dependees[s].Any();   //returns if any dependees are found
            }
            return false;

        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null. If s is null throws a ArgumentNullException. 
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
                throw new ArgumentNullException("The key was null.");

            if (dependents.ContainsKey(s))
                return dependents[s];
            else
                return new List<string>();      //no dependents are found, returns empty list
        }

        /// <summary>
        ///  Creates a list of all dependents, direct and indirect, for a cell. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<string> GetAllDependents(string name, int depth)
        {
            if (depth > 1000)
                throw new IndexOutOfRangeException();
            var allDependents = new List<string>();
            if (dependents.ContainsKey(name))
            {
                foreach (var dependent in dependents[name])
                {
                    allDependents.Add(dependent);
                    int myDepth = depth + 1;
                    var subdep = GetAllDependents(dependent, myDepth);
                    // discourage adding duplicate values
                    foreach (var item in subdep)
                    {
                        if (!allDependents.Contains(item))
                            allDependents.Add(item);
                    }
                }

            }
            return allDependents;
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null. If s is null throws a ArgumentNullException. 
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
                throw new ArgumentNullException("The key was null.");

            if (dependees.ContainsKey(s))
                return dependees[s];
            else
                return new List<string>();     //no dependees are found, returns empty list
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.  If s or t are null throws a ArgumentNullException. 
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if ((s == null) || (t == null))
            {
                throw new ArgumentNullException("One of the parts of the dependency was null.");
            }

            //we need to know if s exists and already has dependent of t
            if (dependents.ContainsKey(s))
            {
                if (!dependents[s].Contains(t))       //s is found, checks to see if t is already an dependent
                    dependents[s].Add(t);
            }
            else
                dependents.Add(s, new List<string>() { t });

            //we need to know if t exists and already has dependees of s
            if (dependees.ContainsKey(t))
            {
                if (!dependees[t].Contains(s))
                    dependees[t].Add(s);
            }
            else
                dependees.Add(t, new List<string>() { s });

        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.  If s or t are null throws an ArgumentNullException. 
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if ((s == null) || (t == null))
                throw new ArgumentNullException("One of the elements of the dependency was null.");
            if (!DependencyExist(s, t))
                return;
            else
            {
                dependents[s].Remove(t);
                dependees[t].Remove(s);
            }
        }

        /// <summary>
        ///Checks to see if s exists first as a key and if does, checks to see if t is a
        ///dependent of s.  Returns true if that dependency is found, returns false
        ///for any other case.  The driver method tests for nulls, so not checked here. 
        private bool DependencyExist(string s, string t)
        {
            if (dependents.ContainsKey(s))
                return dependents[s].Contains(t);
            else
                return false;

        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.  If s or an element of newDependents is null, throws a ArgumentNullException. 
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null)
                throw new ArgumentNullException("The key is null.");
            if (dependents.ContainsKey(s))
            {
                List<String> oldlist = new List<string>();
                oldlist = GetDependents(s).ToList();
                foreach (var olddep in oldlist)           //remove all dependencies of form (s,r)
                {
                    RemoveDependency(s, olddep);
                }
            }
            foreach (var item in newDependents)         //adds all dependency, either all new or as a replacement
            {
                if (item == null)
                    throw new ArgumentNullException("One of the new dependents was null.");
                AddDependency(s, item);
            }

        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null. If s or an element in the newDependee list is null throws a ArgumentNullException. 
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t == null)
                throw new ArgumentNullException("The key was null.");
            if (dependees.ContainsKey(t))
            {
                List<String> oldlist = new List<string>();
                oldlist = GetDependees(t).ToList();
                foreach (var olddep in oldlist)
                {
                    RemoveDependency(olddep, t);              //removes all dependency of form (r,t)
                }
            }
            foreach (var newdep in newDependees)             //adds all dependees, either new or as a replacement 
            {
                if (newdep == null)
                    throw new ArgumentNullException("A new dependee was null.");
                AddDependency(newdep, t);
            }
        }
    }
}
