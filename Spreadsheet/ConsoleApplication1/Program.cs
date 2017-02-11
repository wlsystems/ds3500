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
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("d", "f");
            HashSet < string > hs = new HashSet<string>();
            hs.Add("a");
            hs.Add(null);
            hs.Add("b");
            t.ReplaceDependees("d", null);
        }   
    }
}
