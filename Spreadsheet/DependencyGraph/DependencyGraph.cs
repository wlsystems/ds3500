﻿// Skeleton implementation written by Joe Zachary for CS 3500, January 2017.
// Modified by Dustin Shiozaki 1/30/2017 u0054455

using System;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered
    /// pair of strings..  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
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
    /// 

    

    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> dd; //a directed graph going from dependents to dependees (s,t)
        private Dictionary<string, HashSet<string>> de; //a directed graph going from  dependees todependents  (t, s)
        private int count; //number of dependencies

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dd = new Dictionary<string, HashSet<string>>(); 
            de = new Dictionary<string, HashSet<string>>();
            count = 0;
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get {
                return count;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (dd.ContainsKey(s))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (de.ContainsKey(s))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s) 
        {
            if (HasDependents(s))
            {
                IEnumerator<string> iet = dd[s].GetEnumerator();
                while (iet.MoveNext())
                {
                    yield return iet.Current;
                }
            }
            else
                yield return null;
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (HasDependees(s))
            {
                IEnumerator<string> iet = de[s].GetEnumerator();
                while (iet.MoveNext())
                {
                    yield return iet.Current;
                }
            }
            else
                yield return null;
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            List<string> bag = new List<string>();
            if (dd.ContainsKey(s))
            {
                if (!dd[s].Contains(t))//only add it if it doesn't contain t
                { 
                    dd[s].Add(t);
                    de[t].Add(s);
                    count++;
                }                
            }
            else
            {
                HashSet<string> dependents = new HashSet<string>();
                HashSet<string> dependees = new HashSet<string>();
                dependents.Add(t);
                dependees.Add(s);
                dd.Add(s, dependents);
                de.Add(t, dependees);
                count++;
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if(dd.ContainsKey(s))
            {
                dd[s].Remove(t);
                de[t].Remove(s);
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            
            if (dd.ContainsKey(s))
            {
                IEnumerator<string> iet = dd[s].GetEnumerator(); //get all dependents using the key s. must save them to remove from the reverse graph.
                while (iet.MoveNext()) //iterate over the dependents
                {
                    de[iet.Current].Remove(s); //remove the dependents from the reverse graph
                    count--;
                }                   
                dd[s].Clear(); //clear the dependents from the regular graph.
                foreach (string str in newDependents)
                    AddDependency(s, str); //add the dependents to both graphs         
            }
                    
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {

        }
    }
}
