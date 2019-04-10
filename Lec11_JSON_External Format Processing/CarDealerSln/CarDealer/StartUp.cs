using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        private const string message = "Successfully imported {0}.";
        private static int addedCars = 0;

        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            //var stringSuppliers = File.ReadAllText(@"..\..\..\Datasets\Suppliers.json");
            //var stringParts = File.ReadAllText(@"..\..\..\Datasets\Parts.json");
            //var stringCars = File.ReadAllText(@"..\..\..\Datasets\Cars.json");
            //var stringCustomers = File.ReadAllText(@"..\..\..\Datasets\Customers.json");
            //var stringSales = File.ReadAllText(@"..\..\..\Datasets\Sales.json");

            //Console.WriteLine(ImportSuppliers(context, stringSuppliers));
            //Console.WriteLine(ImportParts(context, stringParts));
            //Console.WriteLine(ImportCars(context, stringCars));
            //Console.WriteLine(ImportCustomers(context, stringCustomers));
            //Console.WriteLine(ImportSales(context, stringSales));
            //Console.WriteLine(GetOrderedCustomers(context));
            //Console.WriteLine(GetCarsFromMakeToyota(context));
            //Console.WriteLine(GetLocalSuppliers(context));
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            //Console.WriteLine(GetTotalSalesByCustomer(context));
            Console.WriteLine(GetSalesWithAppliedDiscount(context));

            //var cars = JsonConvert.DeserializeObject<List<InsertCarDto>>(carsJson);

            //var totalPartsCount = 0;
            //var totalRepeats = 0;
            //var totalPartCarsInDb = context.PartCars.Count();
            //foreach (var carDto in cars)
            //{
            //    var test = carDto.PartsId;
            //    totalPartsCount += test.Length;
            //    for (int i = 0; i < test.Length; i++)
            //    {
            //        for (int j = i + 1; j < test.Length; j++)
            //        {
            //            if (test[i] == test[j])
            //            {
            //                totalRepeats++;
            //            }
            //        }
            //    }
            //}
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);
            var count = context.SaveChanges();

            return String.Format(message, count);
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson)
                .Where(x => context.Suppliers.Select(z => z.Id).ToArray().Contains(x.SupplierId)).ToArray();

            context.Parts.AddRange(parts);
            var count = context.SaveChanges();

            return String.Format(message, count);
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carDtos = JsonConvert.DeserializeObject<List<InsertCarDto>>(inputJson, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });

            foreach (var carDto in carDtos)
            {
                var currentCar = new Car();
                currentCar.Make = carDto.Make;
                currentCar.Model = carDto.Model;
                currentCar.TravelledDistance = carDto.TravelledDistance;
                context.Cars.Add(currentCar);

                var partCars = new List<PartCar>();

                foreach (var partId in carDto.PartsId.Distinct())
                {
                    var partCar = new PartCar()
                    {
                        Part = context.Parts.FirstOrDefault(x => x.Id == partId),
                        Car = currentCar
                    };

                    if (partCar.Part == null)
                    {
                        continue;
                    }

                    partCars.Add(partCar);
                }

                currentCar.PartCars = partCars;
            }

            context.SaveChanges();
            //тък съм си извадил основата на съобщението в константа с плейс холдър и
            //затова ползвам този вариант за връщане на резуктата
            return string.Format(message, context.Cars.Count());
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

            context.AddRange(customers);

            return string.Format(message, context.SaveChanges());
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);

            return string.Format(message, context.SaveChanges());
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(x => x.BirthDate.Date)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    IsYoungDriver = x.IsYoungDriver
                })
                .ToArray();

            var result = JsonConvert.SerializeObject(customers, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });

            return result;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toytotaCars = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .Select(x => new
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .ToArray();

            var result = JsonConvert.SerializeObject(toytotaCars, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });

            return result;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToArray();

            var result = JsonConvert.SerializeObject(suppliers, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });

            return result;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance,
                    },
                    parts = c.PartCars.Select(p => new
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price.ToString("F2")
                    }).ToArray()
                }).ToArray();

            var result = JsonConvert.SerializeObject(cars, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            });

            return result;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var sales = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Select(
                        x => x.Car.PartCars.Sum(z => z.Part.Price)).ToArray()
                }).ToArray()
                .Select(x => new
                {
                    x.FullName,
                    x.BoughtCars,
                    SpentMoney = Math.Round(x.SpentMoney.Sum(), 2)
                })
                .OrderByDescending(x => x.SpentMoney)
                .ThenByDescending(x => x.BoughtCars)
                .ToArray();

            var result = JsonConvert.SerializeObject(sales, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return result;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    Discount = s.Discount.ToString("F2"),
                    price = s.Car.PartCars.Sum(x => x.Part.Price).ToString("F2"),
                    priceWithDiscount = (s.Car.PartCars.Sum(x => x.Part.Price) -
                                         (s.Discount / 100 * s.Car.PartCars.Sum(x => x.Part.Price))).ToString("F2")
                })
                .ToArray()
                .Take(10);

            var result = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return result;
        }
    }
}