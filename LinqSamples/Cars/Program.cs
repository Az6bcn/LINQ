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
            // read csv file into memory
            var cars = ProcessFile("fuel.csv");
            var manufactures = ProcessManufacturesFile("manufacturers.csv");

            var mostEfficientCars = MostFuelEfficientCar(cars, 2 , 10);
            foreach (var car in mostEfficientCars)
            {
                //Console.WriteLine($"{car.Name} : {car.Combined}");
            }

            var topBMWEfficientCar2016 = FilterBySpecificManufacturerForYear2016("BMW", cars).First();
            //Console.WriteLine($"BMW most Efficient Car 2016: {topBMWEfficientCar2016.Name} : {topBMWEfficientCar2016.Combined}");

            var anoymousType = FilterBySpecificManufacturerForYear2016Projection("BMW", cars);
            //Console.WriteLine($"BMW most Efficient Car 2016: {anoymousType.Maker} : {anoymousType.MadeInYear}");


            // **************************************** J o i n *************************************
            var top10EfficientCarsAndHeadquarters = GetMostEfficientCarAndManufacturersHeadquarter(cars, manufactures);
            foreach (var item in top10EfficientCarsAndHeadquarters)
            {
                Console.WriteLine($"{item.Vehile}: {item.HeadQuaters}");
            }


            // **************************************** G r  o u p J o  i n **************************
            var group = GetCarsManufacturersInEachHeadquarter(cars, manufactures);
            foreach (var item in group)
            {
                Console.WriteLine($" Maker => {item.Maker}, Headquarters => {item.HeadQuaters}");
                // iterate the list of each group: maker
                foreach (var car in item.ListOfCars)
                {
                    Console.WriteLine(($"{car.Name} : {car.Manufacture}"));
                }
            }
        }

        private static List<Car> ProcessFile(string path)
        {
            return File.ReadLines(path)
                        .Skip(1)
                        .Where(line => line.Length > 1)
                        .Select(Car.ParseFromCSV)
                        .ToList();
        }


        private static List<Manufacturer> ProcessManufacturesFile(string path)
        {
            return File.ReadLines(path)
                        .Where(line => line.Length > 1)
                        .Select(Manufacturer.ParseFromCSV) // pass each line to the method to project its information into Manufacture object
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


        // ######################### J o i n #################

        private static dynamic GetMostEfficientCarAndManufacturersHeadquarter( List<Car> cars, List<Manufacturer> manufacturers)
        {
            return cars
                .OrderByDescending( e => e.Combined)
                .Join(manufacturers,
                c => c.Manufacture, //key on the left side (1st entity: Cars)
                m => m.Name, //key on the right side (2nd entity: Manufactures)
                (car, marker) => new //result selector: what to do once matched: projection result
                {
                    Vehile = car.Name,
                    HeadQuaters = marker.Headquarters
                }).ToList();
        }

        // GroupJoin by marker name: group car by their makers
        private static dynamic GetCarsManufacturersInEachHeadquarter(List<Car> cars, List<Manufacturer> manufacturers)
        {
            var xxx = manufacturers
                .GroupJoin(cars,
                m => m.Name, //key on the left side (1st entity: Manufactures)
                c => c.Manufacture, //key on the right side (2nd entity: Cars)
                (maker, carsByMaker) => new //result selector: projection result
                {
                    Maker = maker.Name,
                    HeadQuaters = maker.Headquarters,
                    ListOfCars = carsByMaker,  // list/group of cars for each marker 
                })
                .Take(5)
                .ToList();

            return xxx;
        }
    }
}
