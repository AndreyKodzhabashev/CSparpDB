using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            //Mapper.Initialize(cfg => new ProductShopProfile());
            //Mapper.Configuration.CompileMappings();

            var cont = new ProductShopContext();

            //cont.Database.EnsureDeleted();
            //cont.Database.EnsureCreated();
            string input = File.ReadAllText(
                @"C:\Users\PC\Documents\SOFTUNI\MyCourses\_08_DB_Advance_EntityFramework\Lec11_JSON_External Format Processing\ProductShopSln\ProductShop\Datasets\categories-products.json");
            //Console.WriteLine(ImportUsers(cont,input));
            //Console.WriteLine(ImportProducts(cont,input));
            //Console.WriteLine(ImportCategories(cont, input));
            //Console.WriteLine(ImportCategoryProducts(cont,input));
            //Console.WriteLine(GetProductsInRange(cont));
            //Console.WriteLine(GetSoldProducts(cont));
            //Console.WriteLine(GetCategoriesByProductsCount(cont));
            Console.WriteLine(GetUsersWithProducts(cont));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);

            return $"Successfully imported {context.SaveChanges()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);

            return $"Successfully imported {context.SaveChanges()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(inputJson)
                .Where(x => x.Name.Length > 2 && x.Name.Length < 16 && x.Name != null).ToList();

            context.Categories.AddRange(categories);

            return $"Successfully imported {context.SaveChanges()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            List<CategoryProduct> categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);
            context.CategoryProducts.AddRange(categoryProducts);
            return $"Successfully imported {context.SaveChanges()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var result = context.Products
                .OrderBy(x => x.Price)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .Where(x => x.price >= 500 && x.price <= 1000).ToList();

            var json = JsonConvert.SerializeObject(result);

            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var result = context.Users
                .Where(u => u.ProductsSold.Any(y => y.Buyer != null))
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                        .Select(ps => new
                        {
                            Name = ps.Name,
                            Price = ps.Price,
                            BuyerFirstName = ps.Buyer.FirstName,
                            BuyerLastName = ps.Buyer.LastName
                        }).ToArray()
                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToArray();


            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var result = context.Categories
                .Select(x => new
                {
                    Category = x.Name,
                    ProductsCount = x.CategoryProducts.Count(),
                    AveragePrice = x.CategoryProducts.Average(p => p.Product.Price).ToString("F2"),
                    TotalRevenue = x.CategoryProducts.Sum(p => p.Product.Price).ToString("F2")
                })
                .OrderByDescending(x => x.ProductsCount)
                .ToArray();

            var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver() {NamingStrategy = new CamelCaseNamingStrategy()},
                Formatting = Formatting.Indented
            });

            return json;
        }

        //public static string GetUsersWithProducts(ProductShopContext context)
        //{
        //    var users = context.Users
        //        .Where(u => u.ProductsSold.Any(y => y.Buyer != null))
        //        .Select(x => new
        //        {
        //            FirstName = x.FirstName,
        //            LastName = x.LastName,
        //            Age = x.Age,
        //            SoldProducts = new
        //            {
        //                Count = x.ProductsSold.Count(ps => ps.Buyer != null),
        //                Products = x.ProductsSold
        //                    .Where(p => p.Buyer != null)
        //                    .Select(p => new
        //                    {
        //                        Name = p.Name,
        //                        Price = p.Price
        //                    })
        //                    .ToArray()
        //            }
        //        })
        //        .OrderByDescending(x=>x.SoldProducts.Count)
        //        .ToArray();

        //    var result = new
        //    {
        //        UsersCount = users.Length,
        //        Users = users
        //    };

        //    var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
        //    {
        //        ContractResolver = new DefaultContractResolver()
        //        {
        //            NamingStrategy = new CamelCaseNamingStrategy()
        //        },

        //       // Formatting = Formatting.Indented,
        //        NullValueHandling = NullValueHandling.Ignore
        //    });
        //    return json;
        //}

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var count = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    productsSold = u.ProductsSold
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                })
                .Count();

            string countToString = JsonConvert.SerializeObject(count);

            var collection = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = u.ProductsSold.Count,
                    products = u.ProductsSold
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                })
                .ToList();

            var jsonCollection = JsonConvert.SerializeObject(collection, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            /*JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings {
            NullValueHandling = NullValueHandling.Ignore
                            });*/
            return $"{"{"}\n\"usersCount\":{countToString},\n\"users\":\n{jsonCollection}";
        }
    }
}