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
            dg.AddDependency("s", "a");
            dg.AddDependency("s", "a");
            IEnumerable<string> s = dg.GetDependents("s");
            foreach (String str in s)
                Console.Write(str);

            IEnumerable<string> dee = dg.GetDependees("a");
 
            foreach (String str in dee)
                Console.Write(str);

            
        }
    }
}
