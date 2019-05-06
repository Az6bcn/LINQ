using System;
namespace Cars
{
    public class Manufacturer
    {
        public string Name { get; set; }
        public string Headquarters { get; set; }
        public int Year { get; set; }

        public static Manufacturer ParseFromCSV(string line)
        {
            var columns = line.Split(',');

            return new Manufacturer
            {
                Name = columns[0],
                Headquarters = columns[1],
                Year = int.Parse(columns[2]),
            };
        }
    }
}
