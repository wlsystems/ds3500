using Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("s", "1");
            dg.AddDependency("s", "3");
            dg.AddDependency("s", "5");
            IEnumerable<string> s = dg.GetDependents("s");
            dg.AddDependency("a", "2");
            dg.AddDependency("a", "4");
            dg.AddDependency("a", "6");
            dg.ReplaceDependents("a", s);
            IEnumerable < string > s6 = dg.GetDependees("6");
            IEnumerator<string> iet = s6.GetEnumerator();
        }
    }
}
