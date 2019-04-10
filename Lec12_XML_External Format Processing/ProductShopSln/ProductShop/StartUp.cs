using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private const string message = "Successfully imported {0}";

        public static void Main()
        {
            Mapper.Initialize(x => { x.AddProfile<ProductShopProfile>(); });


            //var stringUser = File.ReadAllText("../../../Datasets/users.xml");
            //var stringProducts = File.ReadAllText("../../../Datasets/products.xml");
            //var stringCategories = File.ReadAllText("../../../Datasets/categories.xml");
            //var stringCategoriesProducts = File.ReadAllText("../../../Datasets/categories-products.xml");

            using (var context = new ProductShopContext())
            {
                // context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                //Console.WriteLine(ImportUsers(context,stringUser));
                //Console.WriteLine(ImportProducts(context,stringProducts));
                // Console.WriteLine(ImportCategories(context, stringCategories));
                //Console.WriteLine(ImportCategoryProducts(context, stringCategoriesProducts));
                //Console.WriteLine(GetProductsInRange(context));
                //Console.WriteLine(GetSoldProducts(context));
                //Console.WriteLine(GetCategoriesByProductsCount(context));
                Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportUserDto[]), new XmlRootAttribute("Users"));

            var usersDtos = (ImportUserDto[]) serializer.Deserialize(new StringReader(inputXml));
         
            var users = new List<User>();
            foreach (var userDto in usersDtos)
            {
                var user = Mapper.Map<User>(userDto);

                users.Add(user);
            }

            context.Users.AddRange(users);

            var result = context.SaveChanges();

            return string.Format(message, result);
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));

            var productsDto = (ImportProductDto[]) serializer.Deserialize(new StringReader(inputXml));

            var products = new List<Product>();
            foreach (var productDto in productsDto)
            {
                var product = Mapper.Map<Product>(productDto);

                products.Add(product);
            }

            context.Products.AddRange(products);
            var result = context.SaveChanges();

            return string.Format(message, result);
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(ImportCategoryDto[]), new XmlRootAttribute("Categories"));

            var categoryDtos = (ImportCategoryDto[]) serializer.Deserialize(new StringReader(inputXml));

            var categories = new List<Category>();
            foreach (var categoryDto in categoryDtos)
            {
                var category = Mapper.Map<Category>(categoryDto);

                if (category.Name != null)
                {
                    categories.Add(category);
                }
            }

            context.Categories.AddRange(categories);
            var result = context.SaveChanges();

            return string.Format(message, result);
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(ImportCatProdDto[]), new XmlRootAttribute("CategoryProducts"));

            var catProds = (ImportCatProdDto[]) serializer.Deserialize(new StringReader(inputXml));

            var result = new List<CategoryProduct>();

            foreach (var catProd in catProds)
            {
                var catprod = new CategoryProduct()
                {
                    Category = context.Categories.Find(catProd.CategoryId),
                    Product = context.Products.Find(catProd.ProductId)
                };
                if (catprod.Product != null && catprod.Category != null)
                {
                    result.Add(catprod);
                }
            }

            ;

            context.CategoryProducts.AddRange(result);

            var test = context.SaveChanges();

            return string.Format(message, test);
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var result = context.Products
                .Where(x => 500 <= x.Price && x.Price <= 1000)
                .OrderBy(x=>x.Price)
                .Select(p => new ExportProductsDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .Take(10)
                .ToArray();
            XmlSerializerNamespaces qm = new XmlSerializerNamespaces();
            qm.Add("", "");

            XmlSerializer serializer = new XmlSerializer(typeof(ExportProductsDto[]), new XmlRootAttribute("Products"));
            StringBuilder sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), result, qm);


            return sb.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var result = context.Users
                .Where(u => u.ProductsSold.Any(y => y.Buyer != null))
                .Select(u => new SoldProductsDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                        .Select(ps => new ProductsDto()
                        {
                            Name = ps.Name,
                            Price = ps.Price,
                          }).ToArray()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ToArray();

            XmlSerializerNamespaces qm = new XmlSerializerNamespaces();
            qm.Add("", "");

            XmlSerializer serializer = new XmlSerializer(typeof(SoldProductsDto[]), new XmlRootAttribute("Users"));
            StringBuilder sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), result, qm);
            
            return sb.ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {

            var result = context.Categories
                .Select(x => new CategoriesByProductsCountDto()
                {
                    Category = x.Name,
                    ProductsCount = x.CategoryProducts.Count(),
                    AveragePrice = x.CategoryProducts.Select(y => y.Product.Price).Average(),
                    TotalRevenue = x.CategoryProducts.Sum(p => p.Product.Price)
                })
                .OrderByDescending(x => x.ProductsCount)
                .ThenBy(x=>x.TotalRevenue)
                .ToArray();

            XmlSerializerNamespaces qm = new XmlSerializerNamespaces();
            qm.Add("", "");

            XmlSerializer serializer = new XmlSerializer(typeof(CategoriesByProductsCountDto[]), new XmlRootAttribute("Categories"));
            StringBuilder sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), result, qm);

            return sb.ToString().TrimEnd();
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var result = context.Users
                    .Where(u => u.ProductsSold.Any(y => y.Buyer != null))
                    .Select(x => new UserProductsDto()
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Age = x.Age,
                      SoldProducts = new SoldProdDto()
                          {
                              Count = x.ProductsSold.Count(ps => ps.Buyer != null),
                              Products = x.ProductsSold
                                  .Where(p=>p.Buyer != null)
                                  .Select(p=>new ProdsDto()
                                  {
                                      Name = p.Name,
                                      Price = p.Price
                                  })
                                  .OrderByDescending(p=>p.Price)
                                  .ToArray()
                          }
                    })
                    .OrderByDescending(x => x.SoldProducts.Count)
                    .ToArray();

            var result1 = new UserAndProductsDto()
            {
                Count = result.Length,
                UsersProducts = result
            };

            XmlSerializerNamespaces qm = new XmlSerializerNamespaces();
            qm.Add("", "");

            XmlSerializer serializer = new XmlSerializer(typeof(UserAndProductsDto), new XmlRootAttribute("Users"));
            StringBuilder sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), result1, qm);

            return sb.ToString().TrimEnd();
        }
    }
}