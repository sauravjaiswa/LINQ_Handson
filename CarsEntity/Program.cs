using Cars;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace CarsEntity
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDb>());
            //InsertData();
            QueryData();
        }

        private static void QueryData()
        {
            var db = new CarDb();
            db.Database.Log = Console.WriteLine;

            var query = from car in db.Cars
                        orderby car.Combined descending, car.Name ascending
                        select car;

            var query1 = db.Cars.Where(c => c.Manufacturer == "BMW")
                        .OrderByDescending(c => c.Combined)
                        .ThenBy(c => c.Name)
                        .Take(10)
                        .ToList();

            //foreach(var item in query)
            //{
            //    Console.WriteLine(item.Name);
            //}

            Console.WriteLine(query1.Count);
            foreach(var car in query1)
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");
            }

            Console.WriteLine("#####################");

            var query2 = db.Cars.GroupBy(c => c.Manufacturer)
                                .Select(g => new
                                {
                                    Name = g.Key,
                                    Cars = g.OrderByDescending(c => c.Combined).Take(2)
                                });

            var query3 = from car in db.Cars
                         group car by car.Manufacturer into manufacturer
                         select new
                         {
                             Name = manufacturer.Key,
                             Cars = (from car in manufacturer
                                     orderby car.Combined descending
                                     select car).Take(2)
                         };

            foreach (var group in query3)
            {
                Console.WriteLine(group.Name);
                foreach(var car in group.Cars)
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
        }

        private static void InsertData()
        {
            var cars = ProcessCars("Fuel.csv");
            var db = new CarDb();

            if (!db.Cars.Any())
            {
                foreach(var car in cars)
                {
                    db.Cars.Add(car);
                }
                db.SaveChanges();
            }
        }

        private static List<Car> ProcessCars(string path)
        {
            var query = File.ReadAllLines(path)
                            .Skip(1)
                            .Where(l => l.Length > 1)
                            .ToCar();
            return query.ToList();
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
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
