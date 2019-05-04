using System;
using System.Collections.Generic;

namespace LinqSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var developers = new Employee[]
            {
                 new Employee{ ID = 1, Name ="Azeez"},
                 new Employee{ ID = 2, Name = "Odumosu"}
            };

            var sales = new List<Employee>
            {
                new Employee{ ID = 3, Name = "Sales1"}
            };

            Console.WriteLine("Hello World!");
        }
    }
}
