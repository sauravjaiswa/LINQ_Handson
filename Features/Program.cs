using Introduction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee{Id=1,Name="Scott"},
                new Employee{Id=2,Name="Chris"}
            };

            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee{Id=3,Name="Alex"}
            };

            Console.WriteLine(developers.Count());  //Here Count is the Custom Extension method made in MyLinq
            Console.WriteLine(sales.Count());

            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }

            /**Method Syntax Approach**/

            //Printing names starting with 'S'
            foreach(var employee in developers.Where(employee => employee.Name.StartsWith("S")))
            {
                Console.WriteLine(employee.Name);
            }

            //Printing names having length 5 and sorting it
            foreach (var employee in developers.Where(employee => employee.Name.Length == 5).OrderBy(e => e.Name))
            {
                Console.WriteLine(employee.Name);
            }

            /**Query Syntax Approach**/
            var query2 = from developer in developers
                         where developer.Name.Length == 5
                         orderby developer.Name descending
                         select developer;
            
            foreach(var dev in query2)
            {
                Console.WriteLine(dev.Name);
            }
        }
    }
}
