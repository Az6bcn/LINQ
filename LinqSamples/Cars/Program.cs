using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");

            var mostEfficientCars = MostFuelEfficientCar(cars, 2 , 10);
            foreach (var car in mostEfficientCars)
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");
            }

            var topBMWEfficientCar2016 = FilterBySpecificManufacturerForYear2016("BMW", cars).First();
            Console.WriteLine($"BMW most Efficient Car 2016: {topBMWEfficientCar2016.Name} : {topBMWEfficientCar2016.Combined}");

            var anoymousType = FilterBySpecificManufacturerForYear2016Projection("BMW", cars);
            Console.WriteLine($"BMW most Efficient Car 2016: {anoymousType.Maker} : {anoymousType.MadeInYear}");
        }

        private static List<Car> ProcessFile(string path)
        {
            return File.ReadLines(path)
                        .Skip(1)
                        .Where(line => line.Length > 1)
                        .Select(Car.ParseFromCSV)
                        .ToList();
        }

        /// <summary>
        /// Mosts the fuel efficient car. i.e car that emits least (Combined)
        /// </summary>
        /// <returns>The fuel efficient car.</returns>
        /// <param name="cars">Cars.</param>
        private static List<Car> MostFuelEfficientCar(List<Car> cars, int page, int pageSize )
        {
            return cars.OrderByDescending(c => c.Combined)
                        .ThenByDescending(n => n.Name)
                        .Skip((page -1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        /// <summary>
        /// Filters the by specific manufacturer for 2016.
        /// </summary>
        /// <returns>The by specific manufacturer.</returns>
        /// <param name="maker">Maker.</param>
        private static List<Car> FilterBySpecificManufacturerForYear2016(string maker, List<Car> cars)
        {
            return cars.Where(c => c.Manufacture == maker && c.Year == 2016)
                        .OrderByDescending(c => c.Combined)
                        .ThenByDescending(c => c.Name)
                        .ToList();
        }

        /// <summary>
        /// Filters the by specific manufacturer for 2016 and project result to anoymous type.
        /// </summary>
        /// <returns>The by specific manufacturer.</returns>
        /// <param name="maker">Maker.</param>
        private static dynamic FilterBySpecificManufacturerForYear2016Projection(string maker, List<Car> cars)
        {
            return cars.Where(c => c.Manufacture == maker && c.Year == 2016)
                        .OrderByDescending(c => c.Combined)
                        .ThenByDescending(c => c.Name)
                        .Select(c => new
                        {
                            Maker = c.Manufacture,
                            MadeInYear = c.Year
                        })
                        .First();
        }
    }
}
