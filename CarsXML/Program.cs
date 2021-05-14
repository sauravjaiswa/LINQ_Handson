using Cars;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CarsXML
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateXml();
            QueryXml();
        }

        private static void QueryXml()
        {
            var document = XDocument.Load("fuel.xml");
            var query = from element in document.Descendants("Car")
                        where element.Attribute("Manufacturer")?.Value == "BMW"
                        select element.Attribute("Name").Value;

            foreach(var name in query)
            {
                Console.WriteLine(name);
            }
        }

        public static void CreateXml()
        {
            var records = ProcessCars("Fuel.csv");

            var document = new XDocument();
            var cars = new XElement("Cars",
                        from record in records
                        select new XElement("Car",
                              new XAttribute("Name", record.Name),
                              new XAttribute("Combined", record.Combined),
                              new XAttribute("Manufacturer", record.Manufacturer)));

            document.Add(cars);
            document.Save("fuel.xml");
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
