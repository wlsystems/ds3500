using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            StreamReader reader = File.OpenText("Employees.txt");
            string line;
            double hours = 0;
            double pay = 0;
            List <Workers> sortedL = new List<Workers>();
            Dictionary<string, List<Workers>> AllStates = new Dictionary<string, List<Workers>>(9);
            AllStates["UT"] = new List<Workers>();
            AllStates["WY"] = new List<Workers>();
            AllStates["NV"] = new List<Workers>();
            AllStates["CO"] = new List<Workers>();
            AllStates["ID"] = new List<Workers>();
            AllStates["AZ"] = new List<Workers>();
            AllStates["WA"] = new List<Workers>();
            AllStates["NM"] = new List<Workers>();
            AllStates["TX"] = new List<Workers>();
            AllStates["OR"] = new List<Workers>();
            Dictionary<string, Workers> empLookup = new Dictionary<string, Workers>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split(',');
                hours = Convert.ToDouble(items[7]);
                pay = Convert.ToDouble(items[4]);
                var today = DateTime.Today;
                // Calculate the age.
                var age = today.Year - DateTime.Parse(items[5].ToString()).Year;
                // Go back to the year the person was born in case of a leap year

                if (DateTime.Parse(items[5].ToString()) > today.AddYears(-age))
                    age--;

                if (items[3] == "S")
                    pay = pay / 28;
                else if (items[3].ToString() == "H")
                {
                    if (hours < 80)
                        pay = hours * pay;
                    else if (hours > 80 && hours <= 90)
                        pay = 80 * pay + ((hours - 80) * pay * 1.5);
                    else
                        pay = (80 * pay) + (10 * pay * 1.5) + ((hours - 90) * 1.75 * pay);
                }
                double state;
                double fed;
                fed = pay * .15;
                if (items[6].ToString() == "UT" | items[6].ToString() == "WY" | items[6].ToString() == "NV")
                    state = pay * .05;
                else if (items[6].ToString() == "CO" | items[6].ToString() == "ID" | items[6].ToString() == "AZ" | items[6].ToString() == "OR")
                    state = pay * .065;
                else if (items[6].ToString() == "WA" | items[6].ToString() == "NM" | items[6].ToString() == "TX")
                    state = pay * .07;
                else state = 0;
                Workers W = new Workers(Convert.ToDouble(items[7]),items[6], items[0], items[1], items[2], Math.Round(pay, 2), Math.Round(fed,2), Math.Round(state,2), age, Math.Round(pay - fed - state, 2));
                sortedL.Add(W);
                Workers WState = new Workers(Convert.ToDouble(items[7]), items[6], items[0], items[1], items[2], Math.Round(pay, 2), Math.Round(fed, 2), Math.Round(state, 2), age, Math.Round(pay - fed - state, 2));
                AllStates[items[6].ToString()].Add(WState);
                empLookup.Add(items[0], W);
            }
            List<Workers> SortedList = sortedL.OrderByDescending(o => o.Gross).ToList();
            StreamWriter writer = new StreamWriter("report1.txt");
            foreach (Workers w in SortedList)
                writer.WriteLine(w.ID + ", "+ w.FirstName + ", " + w.LastName + ", " + w.Gross + ", " + w.Fed + ", " + w.State + ", " + w.Net);
            writer.Close();
            List<Workers> top15 = SortedList.GetRange(0, (Convert.ToInt32(SortedList.Count * .15)));
            top15  = top15.OrderByDescending(o => o.Years).ToList();
            top15 = top15.OrderByDescending(o => o.LastName).ToList();
            top15 = top15.OrderByDescending(o => o.FirstName).ToList();
            StreamWriter writer2 = new StreamWriter("report2.txt");
            foreach (Workers w in top15)
                writer2.WriteLine(w.FirstName + ", " + w.LastName + ", " + w.Years + ", " + w.Gross);
            writer2.Close();
            StreamWriter writer3 = new StreamWriter("report3.txt");
            double totalTax = 0;
            foreach (KeyValuePair<string, List<Workers>> item in AllStates)
            {
                totalTax = 0;
                List<Workers> jWorkers = item.Value.OrderByDescending(o => o.Hours).ToList();
                List<Workers> jKWorkers = item.Value.OrderByDescending(o => o.Net).ToList();
                int median = Convert.ToInt32(jWorkers.Count / 2);
                foreach (Workers k in jWorkers) {
                    totalTax = totalTax + k.State;
                }
                writer3.WriteLine(item.Key +", " + jWorkers.ElementAt(median).Hours + ", " + +jKWorkers.ElementAt(median).Net + ", " + Math.Round(totalTax,2));
            }
            writer3.Close();
            ////// Lookup Method
            //////
            Workers testEmp = getEmp(empLookup, "1");
            Console.WriteLine(testEmp.FirstName + ", " + testEmp.LastName);
        }
        public static Workers getEmp(Dictionary<string, Workers> d, string id)
        {
            try
            {
                Workers queryWorker = d[id];
                return queryWorker;
            }
            catch
            {
                return null;
            }
        }
        public class States
        {
            public States() { }
            public States(string stateName) {
                StateName = stateName;
            }
            public string StateName { get; set; }
            public List<Workers> StateWorkers{ get; set; }
            public void AddWorker(Workers w)
            {
                StateWorkers.Add(w);
            }
        }
        public class Workers
        {
            public Workers() { }
            public Workers(double hours, string stateName, string id, string first, string last, double gross, double fed, double state, int years, double net)
            {
                FirstName = first;
                LastName = last;
                ID = id;
                Gross = gross;
                Net = net;
                Fed = fed;
                State = state;
                StateName = stateName;
                Years = years;
                Hours = hours;
            }

            // Properties.
            public double Hours { get; set; }
            public int Years { get; set; }
            public string StateName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ID { get; set; }
            public double Net { get; set; }
            public double Gross { get; set; }
            public double Fed { get; set; }
            public double State { get; set; }
            public override string ToString()
            {
                return FirstName + "  " + ID;
            }
        }
    }
}
