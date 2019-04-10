using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new CarDealerContext())
            {
                Mapper.Initialize(cgf => cgf.AddProfile(new CarDealerProfile()));
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                //string suppliersImport =
                //File.ReadAllText(
                //    @"C:\Users\PC\Documents\SOFTUNI\MyCourses\_08_DB_Advance_EntityFramework\Lec12_XML_External Format Processing\CarDealerSln\CarDealer - Skeleton\CarDealer\Datasets\suppliers.xml");
                //string partsImport =
                //    File.ReadAllText(
                //        @"C:\Users\PC\Documents\SOFTUNI\MyCourses\_08_DB_Advance_EntityFramework\Lec12_XML_External Format Processing\CarDealerSln\CarDealer - Skeleton\CarDealer\Datasets\parts.xml");
                //string carsImport =
                //    File.ReadAllText(
                //        @"C:\Users\PC\Documents\SOFTUNI\MyCourses\_08_DB_Advance_EntityFramework\Lec12_XML_External Format Processing\CarDealerSln\CarDealer - Skeleton\CarDealer\Datasets\cars.xml");
                //string customersImport =
                //    File.ReadAllText(
                //        @"C:\Users\PC\Documents\SOFTUNI\MyCourses\_08_DB_Advance_EntityFramework\Lec12_XML_External Format Processing\CarDealerSln\CarDealer - Skeleton\CarDealer\Datasets\customers.xml");
                //string salesImport =
                //File.ReadAllText(
                //    @"C:\Users\PC\Documents\SOFTUNI\MyCourses\_08_DB_Advance_EntityFramework\Lec12_XML_External Format Processing\CarDealerSln\CarDealer - Skeleton\CarDealer\Datasets\sales.xml");

                //Console.WriteLine(ImportSuppliers(context, suppliersImport));
                //Console.WriteLine(ImportParts(context, partsImport));
                //Console.WriteLine(ImportCars(context, carsImport));
                //Console.WriteLine(ImportCustomers(context,customersImport));
                // Console.WriteLine(ImportSales(context, salesImport));
                //Console.WriteLine(GetCarsWithDistance(context));
                //Console.WriteLine(GetCarsFromMakeBmw(context));
                //Console.WriteLine(GetLocalSuppliers(context));
                //Console.WriteLine(GetCarsWithTheirListOfParts(context));
                //Console.WriteLine(GetTotalSalesByCustomer(context));
                Console.WriteLine(GetSalesWithAppliedDiscount(context));
            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(ImportSuppliersDto[]), new XmlRootAttribute("Suppliers"));
            var suppliersDto = (ImportSuppliersDto[]) serializer.Deserialize(new StringReader(inputXml));


            var suppliers = new List<Supplier>();
            foreach (var supplier in suppliersDto)
            {
                if (String.IsNullOrEmpty(supplier.Name))
                {
                    continue;
                }

                suppliers.Add(new Supplier()
                {
                    Name = supplier.Name,
                    IsImporter = supplier.IsImporter
                });
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {context.Suppliers.Count()}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCarsDto[]), new XmlRootAttribute("Cars"));

            var result = (ImportCarsDto[]) serializer.Deserialize(new StringReader(inputXml));

            foreach (var carsDto in result)
            {
                var currentCar = new Car();
                currentCar.Make = carsDto.Make;
                currentCar.Model = carsDto.Model;
                currentCar.TravelledDistance = carsDto.TraveledDistance;
                context.Cars.Add(currentCar);

                var ids = carsDto.PartsDto.Select(x => x.Id).ToList();
                foreach (var partDto in ids.Distinct())
                {
                    var partCar = new PartCar()
                    {
                        Car = currentCar,
                        Part = context.Parts.FirstOrDefault(x => x.Id == partDto)
                    };

                    if (partCar.Part == null)
                    {
                        continue;
                    }

                    currentCar.PartCars.Add(partCar);
                }

                context.SaveChanges();
            }

            return $"Successfully imported {context.Cars.Count()}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportPartsDto[]), new XmlRootAttribute("Parts"));

            var partsDtos = (ImportPartsDto[]) serializer.Deserialize(new StringReader(inputXml));

            var parts = new List<Part>();
            foreach (var partsDto in partsDtos)
            {
                var supplier = context.Suppliers.Find(partsDto.SupplierId);

                if (String.IsNullOrEmpty(partsDto.Name)
                    || supplier == null)
                {
                    continue;
                }

                supplier.Parts.Add(new Part()
                {
                    Name = partsDto.Name,
                    Price = partsDto.Price,
                    Quantity = partsDto.Quantity,
                    SupplierId = supplier.Id
                });
                context.SaveChanges();
            }

            return $"Successfully imported {context.Parts.Count()}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(ImportCustomersDto[]), new XmlRootAttribute("Customers"));

            var customersDtos = (ImportCustomersDto[]) serializer.Deserialize(new StringReader(inputXml));

            var customers = new List<Customer>();
            foreach (var importCustomersDto in customersDtos)
            {
                var currentCustomer = new Customer()
                {
                    Name = importCustomersDto.Name,
                    BirthDate = DateTime.Parse(importCustomersDto.BirthDate, CultureInfo.InvariantCulture),
                    IsYoungDriver = importCustomersDto.IsYoungDriver
                };

                if (String.IsNullOrEmpty(currentCustomer.Name))
                {
                    continue;
                }

                customers.Add(currentCustomer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {context.Customers.Count()}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(ImportSalesDto[]), new XmlRootAttribute("Sales"));

            var salesDtos = (ImportSalesDto[]) serializer.Deserialize(new StringReader(inputXml));

            var sales = new List<Sale>();

            foreach (var salesDto in salesDtos)
            {
                var currentSale = new Sale();

                currentSale.Car = context.Cars.Find(salesDto.CarId);
                currentSale.Customer = context.Customers.Find(salesDto.CustomerId);
                currentSale.Discount = salesDto.Discount;

                if (currentSale.Car == null)
                {
                    continue;
                }

                sales.Add(currentSale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {context.Sales.Count()}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var serializer = new XmlSerializer(typeof(ExportCardDistDto[]), new XmlRootAttribute("cars"));

            var cardDistDtos = context.Cars
                .Where(x => x.TravelledDistance > 2000000)
                .Select(x => new ExportCardDistDto
                {
                    Make = x.Make,
                    Model = x.Model,
                    TraveledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ToArray();
            var sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces();
            ns.Add(String.Empty, string.Empty);
            serializer.Serialize(new StringWriter(sb), cardDistDtos, ns);

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var serializer = new XmlSerializer(typeof(ExportCarBmwDto[]), new XmlRootAttribute("cars"));

            var bmwDto = context.Cars
                .Where(x => x.Make == "BMW")
                //.Select(x => new ExportCarBmwDto
                //{
                //    Id = x.Id,
                //    Model = x.Model,
                //    TravelledDistance = x.TravelledDistance
                //})
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ProjectTo<ExportCarBmwDto>()
                .ToArray();

            var sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            serializer.Serialize(new StringWriter(sb), bmwDto, ns);
            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var serializer = new XmlSerializer(typeof(ExportSuppliersDto[]), new XmlRootAttribute("suppliers"));

            var sb = new StringBuilder();

            var suppliersDtos = context.Suppliers
                .Where(x => x.IsImporter == false)
                .ProjectTo<ExportSuppliersDto>()
                .ToArray();


            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            serializer.Serialize(new StringWriter(sb), suppliersDtos, ns);
            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carWithPartsDto = context.Cars
                .Select(x => new ExportCarWithPartsDto()
                    {
                        Make = x.Make,
                        Model = x.Model,
                        TravelledDistance = x.TravelledDistance,
                        Parts = x.PartCars
                            .Select(p => new PartsDto()
                            {
                                Name = p.Part.Name,
                                Price = p.Part.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                    }
                )
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCarWithPartsDto[]), new XmlRootAttribute("cars"));
            var sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            serializer.Serialize(new StringWriter(sb), carWithPartsDto, ns);
            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Select(x => x.Car.PartCars.Sum(z => z.Part.Price)).ToArray()
                })
                .Select(x => new ExportCustomersSalesDto
                {
                    FullName = x.FullName,
                    BoughtCars = x.BoughtCars,
                    SpentMoney = Math.Round(x.SpentMoney.Sum(), 2)
                })
                .OrderByDescending(x=>x.SpentMoney)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCustomersSalesDto[]), new XmlRootAttribute("customers"));
            var sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            serializer.Serialize(new StringWriter(sb), customers, ns);
            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {

            var salesDto = context.Sales
               
                .Select(s => new ExportSalesWithDiscountDto
                    {
                        Car = new CarExportDto
                        {
                            Model = s.Car.Model,
                            Make = s.Car.Make,
                            TravelledDistance = s.Car.TravelledDistance
                        },
                        Price =  s.Car.PartCars.Sum(pc => pc.Part.Price),
                        Discount = s.Discount,
                        CustomerName = s.Customer.Name,
                        PriceWithDiscount = Math.Round(s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100, 2)
                    }
                    )
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportSalesWithDiscountDto[]), new XmlRootAttribute("sales"));
            var sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            serializer.Serialize(new StringWriter(sb),salesDto , ns);
            var result = sb.ToString().TrimEnd();
            return result;
        }
    }
}

