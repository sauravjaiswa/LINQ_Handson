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
            var cars = ProcessCars("Fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");

            //var query = cars.OrderByDescending(c => c.Combined)
            //                .ThenBy(c => c.Name);

            var query = from car in cars
                        join manufacturer in manufacturers 
                        on new { car.Manufacturer, car.Year } 
                        equals 
                        new { Manufacturer = manufacturer.Name, manufacturer.Year }
                        orderby car.Combined descending, car.Name ascending
                        select new
                        {
                            manufacturer.Headquaters,
                            car.Name,
                            car.Combined
                        };

            var query2 = cars.Join(manufacturers,
                            c => new { c.Manufacturer, c.Year },
                            m => new { Manufacturer = m.Name, m.Year }, 
                            (c, m) => new
                            {
                                m.Headquaters,
                                c.Name,
                                c.Combined
                            }).OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name);

            var result3 = cars.Select(c => new { c.Manufacturer, c.Name, c.Combined });
            

            var result = cars.Any(c => c.Manufacturer == "Ford");
            Console.WriteLine(result);

            var result1 = cars.All(c => c.Manufacturer == "Ford");
            Console.WriteLine(result1);

            var result2 = cars.Count(c => c.Manufacturer == "Ford");
            Console.WriteLine(result2);

            var top = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                            .OrderByDescending(c => c.Combined).ThenBy(c => c.Name)
                            .Select(c => c)
                            .FirstOrDefault ();

            Console.WriteLine($"Top Car : {top.Manufacturer} {top.Name}");

            foreach (var car in query2.Take(10))
            {
                Console.WriteLine($"{car.Headquaters} {car.Name} : {car.Combined}");
            }

            //Using group
            var query3 = from car in cars
                         group car by car.Manufacturer.ToUpper() into m
                         orderby m.Key
                         select m;

            var query4 = cars.GroupBy(c => c.Manufacturer.ToUpper())
                            .OrderBy(g => g.Key);

            foreach(var group in query4)
            {
                Console.WriteLine(group.Key);
                foreach(var car in group.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }


            //Using groupjoin
            var query5 = from manufacturer in manufacturers
                         join car in cars on manufacturer.Name equals car.Manufacturer
                         into carGroup
                         orderby manufacturer.Name
                         select new
                         {
                             Manufacturer = manufacturer,
                             Cars = carGroup
                         };

            var query6 = manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer,
                        (m, g) =>
                        new
                        {
                            Manufacturer = m,
                            Cars = g
                        }).OrderBy(m => m.Manufacturer.Name);

            foreach (var group in query6)
            {
                Console.WriteLine($"{group.Manufacturer.Name} : {group.Manufacturer.Headquaters}");
                foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }


            Console.WriteLine("############################");

            var query7 = from manufacturer in manufacturers
                         join car in cars on manufacturer.Name equals car.Manufacturer
                         into carGroup
                         orderby manufacturer.Name
                         select new
                         {
                             Manufacturer = manufacturer,
                             Cars = carGroup
                         } into res
                         group res by res.Manufacturer.Headquaters;

            var query8 = manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer,
                        (m, g) =>
                        new
                        {
                            Manufacturer = m,
                            Cars = g
                        })
                        .GroupBy(m => m.Manufacturer.Headquaters);

            foreach (var group in query8)
            {
                Console.WriteLine($"{group.Key}");
                foreach (var car in group.SelectMany(g => g.Cars).OrderByDescending(c => c.Combined).Take(3))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }

            //Working on Aggregation
            var query9 = from car in cars
                         group car by car.Manufacturer into carGroup
                         select new
                         {
                             Name = carGroup.Key,
                             Max = carGroup.Max(c => c.Combined),
                             Min = carGroup.Min(c => c.Combined),
                             Avg = carGroup.Average(c => c.Combined)
                         } into res
                         orderby res.Max descending
                         select res;

            //Finding Max, Min, Avg in more efficient manner as previous query goes through the whole loop for each max, min and average
            var query10 = cars.GroupBy(c => c.Manufacturer)
                            .Select(g =>
                            {
                                var res = g.Aggregate(new CarStatistics(),
                                                (acc, c) => acc.Accumulate(c),
                                                acc => acc.Compute());
                                return new
                                {
                                    Name = g.Key,
                                    Avg = res.Average,
                                    Min = res.Min,
                                    Max = res.Max
                                };
                            })
                            .OrderByDescending(r => r.Max);

            foreach (var group in query10)
            {
                Console.WriteLine($"{group.Name}");
                Console.WriteLine($"\tMax : {group.Max}");
                Console.WriteLine($"\tMin : {group.Min}");
                Console.WriteLine($"\tAvg : {group.Avg}");
            }
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query = File.ReadAllLines(path)
                            .Where(l => l.Length > 1)
                            .Select(l =>
                            {
                                var columns = l.Split(',');
                                return new Manufacturer
                                {
                                    Name = columns[0],
                                    Headquaters = columns[1],
                                    Year = int.Parse(columns[2])
                                };
                            });

            return query.ToList();
        }

        private static List<Car> ProcessCars(string path)
        {
            //return File.ReadAllLines(path)
            //    .Skip(1)
            //    .Where(line => line.Length > 1)
            //    .Select(Car.ParseFromCsv)
            //    .ToList();

            //return (from line in File.ReadAllLines(path).Skip(1)
            //        where line.Length > 1
            //        select Car.ParseFromCsv(line)).ToList();

            var query = File.ReadAllLines(path)
                            .Skip(1)
                            .Where(l => l.Length > 1)
                            .ToCar();
            return query.ToList();
        }
    }

    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }
        public CarStatistics Accumulate(Car c)
        {
            Count += 1;
            Total += c.Combined;
            Max = Math.Max(Max, c.Combined);
            Min = Math.Min(Min, c.Combined);

            return this;
        }

        public CarStatistics Compute()
        {
            Average = Total / Count;
            return this;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public double Average { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach(var line in source){
                var cols = line.Split(',');
                yield return new Car
                {
                    Year = int.Parse(cols[0]),
                    Manufacturer = cols[1],
                    Name = cols[2],
                    Displacement = double.Parse(cols[3]),
                    Cylinders = int.Parse(cols[4]),
                    City = int.Parse(cols[5]),
                    Highway = int.Parse(cols[6]),
                    Combined = int.Parse(cols[7]),
                };
            }
            
        }
    }
}
