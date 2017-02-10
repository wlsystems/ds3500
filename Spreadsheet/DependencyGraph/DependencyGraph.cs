// Skeleton implementation written by Joe Zachary for CS 3500, January 2017.
// Modified by Dustin Shiozaki 1/30/2017 u0054455

using System;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered
    /// pair of strings..  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2..
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
    /// 
    /// Uses two graphs to improve performance.  One is the reverse of the other.  So for every s,t in
    /// graph there is a t,s in reverse graph.   Both dicitonaries have a hashset of strings which 
    /// allows each key to have multiple dependents. 
    /// Throws ArgumentNullException if any of their parameters are null.
    /// </summary>
    /// 



    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> dd; //a dictionary going from dependents to dependees (s,t)
        private Dictionary<string, HashSet<string>> de; //a dictionary going from  dependees todependents  (t, s)
        private int count; //number of dependencies

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// Throws ArgumentNullException if any of their parameters are null.
        /// </summary>
        public DependencyGraph()
        {
            dd = new Dictionary<string, HashSet<string>>();
            de = new Dictionary<string, HashSet<string>>();
            count = 0;
        }

        public DependencyGraph(DependencyGraph dg)
        {
            if (dg == null)
                throw new ArgumentNullException("null");
            dd = new Dictionary<string, HashSet<string>>();
            de = new Dictionary<string, HashSet<string>>();
            dd = dg.dd;
            de = dg.de;
            count = dg.count;
        }
        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// Throws ArgumentNullException if any of their parameters are null.
        /// </summary>
        public int Size
        {
            get {
                return count;
            }
        }

        /// <summary>
        /// Throws ArgumentNullException if any of their parameters are null.
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (s == null)
                throw new ArgumentNullException("null argument not allowed");     
            if (dd.ContainsKey(s))
                return true;
            else
                return false;  
        }

        /// <summary>
        /// Throws ArgumentNullException if any of their parameters are null.
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
                throw new ArgumentNullException("null argument not allowed");
            if (de.ContainsKey(s))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Throws ArgumentNullException if any of their parameters are null.
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s) 
        {
            if (s == null)
                throw new ArgumentNullException("null");
            if  (HasDependents(s))
            {
                HashSet<string> hs = new HashSet<string>();
                IEnumerator<string> iet = dd[s].GetEnumerator();
                while (iet.MoveNext())
                {
                    yield return iet.Current;
                }
            }
            else
                yield break;
        }

        /// <summary>
        /// Throws ArgumentNullException if any of their parameters are null.
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
                throw new ArgumentNullException("null argument not allowed");
            if (HasDependees(s))
            {
                IEnumerator<string> iet = de[s].GetEnumerator();
                
                while (iet.MoveNext())
                {
                    yield return iet.Current;
                }
            }
            else
                yield break;
        }

        /// <summary>
        /// Throws ArgumentNullException if any of their parameters are null.
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s==null | t==null)
                throw new ArgumentNullException("null argument not allowed");
            HashSet<string> dependents; 
            HashSet<string> dependees;
            if (dd.ContainsKey(s)) //first check if the regular dictionary contains the key or dependent
            {
                if (!dd[s].Contains(t))//only add the dependee it if it doesn't contain it. It doesn't really matter with hashsets but it would throw off the count. This way the count is incremented twice in the case that a duplicate was added.
                {
                    dd[s].Add(t); //add the dependee to the dependent graph
                    if (de.ContainsKey(t)) //this is necessary because the key could already exist in the reverse graph even though the key in the regular graph may not exist (because there can be multiple instances of the same dependee under different keys)
                        de[t].Add(s);
                    else  //create a new hashset and add the string to the set and the set to the dictionary
                    {
                        dependees = new HashSet<string>();
                        dependees.Add(s);
                        de.Add(t, dependees);
                    }
                    count++;
                }                            
            }
            else
            {
                //first create a new hashset of dependees and then add the new set to the dictionary
                dependents = new HashSet<string>();
                dependents.Add(t);
                dd.Add(s, dependents);
                if (de.ContainsKey(t))
                    de[t].Add(s);
                else
                {
                    dependees = new HashSet<string>(); //next add the dependees to the reverse dictionary
                    dependees.Add(s);
                    de.Add(t, dependees);
                }
                count++;
            }
        }

        /// <summary>
        /// Throws ArgumentNullException if any of their parameters are null.
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s==null | t==null)
                throw new ArgumentNullException("null argument not allowed");
            if(dd.ContainsKey(s))
            {
                if (dd[s].Contains(t))
                {
                    dd[s].Remove(t); //remove the dependee if it as no dependents
                    if (dd[s].Count == 0)
                        dd.Remove(s);
                    de[t].Remove(s);
                    if (de[t].Count == 0)  //remove the dependents from the reverse graph
                        de.Remove(t);
                    count--;
                }              
            }
        }

        /// <summary>
        /// Throws ArgumentNullException if any of their parameters are null.
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s==null | newDependents == null)
                throw new ArgumentNullException("null argument not allowed");
            if (dd.ContainsKey(s))
            {
                IEnumerator<string> iet = dd[s].GetEnumerator(); //get all dependents using the key s. must save them to remove from the reverse graph.
                while (iet.MoveNext()) //iterate over the dependents
                {
                    if (iet.Current.Equals(null))
                        throw new ArgumentNullException("null argument not allowed");
                    de[iet.Current].Remove(s); //remove the dependents from the reverse graph
                    if (de[iet.Current].Count == 0) //remove the key from the reverse graph it it has no entries
                        de.Remove(iet.Current);
                    count--;
                }                   
                dd[s].Clear(); //clear the dependents from the regular graph.
                foreach (string str in newDependents)
                    AddDependency(s, str); //add the dependents to both graphs         
            }              
        }

        /// <summary>
        /// Throws ArgumentNullException if any of their parameters are null.
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (newDependees == null | t == null)
                throw new ArgumentNullException("null");
            if (de.ContainsKey(t))
            {
                IEnumerator<string> iet = de[t].GetEnumerator();
                //get all dependees using the key t. must save them to remove from the reverse graph.
                while (iet.MoveNext()) //iterate over the dependees
                {
                    if (iet.Current.Equals(null))
                        throw new ArgumentNullException("null");
                    dd[iet.Current].Remove(t); //remove the dependents from the graph
                    if (dd[iet.Current].Count == 0)
                        dd.Remove(iet.Current);
                    count--;
                }
                de[t].Clear(); //clear the dependents from the reverse graph.
                foreach (string str in newDependees)
                    AddDependency(str, t); //add the dependents to both graphs                   
            }
            else foreach (string s in newDependees) AddDependency(s, t);
        }
    }
}
