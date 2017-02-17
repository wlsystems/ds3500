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
            var d1 = new DependencyGraph();
            var d2 = new DependencyGraph(d1);
            d1.AddDependency("a", "b");
            d2.AddDependency("c", "d");
            IEnumerable<string> iet = d2.GetDependents("a");
            Console.WriteLine(d2.Size);
            foreach (string s in iet)
                Console.WriteLine();
            //Console.WriteLine(d2.GetDependents("a"));
            //Assert.IsFalse(d2.HasDependents("a"));
            //Assert.IsTrue(d2.HasDependents("c"));
        }
    }
}
