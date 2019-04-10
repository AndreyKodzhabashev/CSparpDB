using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BookShop
{
    using System;
    using System.Linq;
    using Data;
    using Initializer;
    using Models.Enums;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {

              // DbInitializer.ResetDatabase(db);

                //string command = Console.ReadLine();


                //Console.WriteLine(GetBooksByAgeRestriction(db, command));
                //Console.WriteLine(GetGoldenBooks(db));
                //Console.WriteLine(GetBooksByPrice(db));
                //Console.WriteLine(GetBooksNotReleasedIn(db, int.Parse(command)));
                //Console.WriteLine(GetBooksByCategory(db, command));
                //Console.WriteLine(GetBooksReleasedBefore(db, command));
                //Console.WriteLine(GetAuthorNamesEndingIn(db, command));
                //Console.WriteLine(GetBookTitlesContaining(db,command));
                //Console.WriteLine(GetBooksByAuthor(db, command));
                //Console.WriteLine((CountBooks(db, int.Parse(command))));
                //Console.WriteLine(CountCopiesByAuthor(db));
                //Console.WriteLine(GetTotalProfitByCategory(db));
                //Console.WriteLine(GetMostRecentBooks(db));
                //Console.WriteLine(IncreasePrices(db));
                Console.WriteLine(RemoveBooks(db));
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageResName = Enum.Parse<AgeRestriction>(command, true);
            var result = context.Books.Where(a => a.AgeRestriction == ageResName).Select(b => b.Title).ToList();

            return string.Join(Environment.NewLine, result.OrderBy(x => x));
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            context.Books.Where(c => c.Copies < 5000 && c.EditionType == EditionType.Gold).Select(b => new
            {
                id = b.BookId,
                Title = b.Title
            }).OrderBy(x => x.id).ToList().ForEach(c => sb.AppendLine(c.Title));


            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Books.Where(p => p.Price > 40).Select(x => new
            {
                Title = x.Title,
                Price = x.Price
            }).OrderByDescending(x => x.Price).ToList().ForEach(x => sb.AppendLine($"{x.Title} - ${x.Price:f2}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var result = context.Books.Where(y => y.ReleaseDate.Value.Year != year).OrderBy(b => b.BookId)
                .Select(x => x.Title)
                .ToList();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower())
                .ToArray();

            var result = context.Books
                .Where(y => y.BookCategories.Any(z => categories.Contains(z.Category.Name.ToLower())))
                .Select(t => t.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var currentDate = DateTime.ParseExact(date, "dd-MM-yyyy", new CultureInfo("en-US"));

            //var test = context.Books.Select(x => x.ReleaseDate.Value).FirstOrDefault();

            var result = context.Books.Where(d => d.ReleaseDate.Value < currentDate)
                .OrderByDescending(x => x.ReleaseDate).Select(x => new
                {
                    Title = x.Title,
                    EditionType = x.EditionType,
                    Price = x.Price
                }).ToList();


            return string.Join(Environment.NewLine, result.Select(x => $"{x.Title} - {x.EditionType} - ${x.Price:f2}"));
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            //Return the full names of authors, whose first name ends with a given string.
            //Return all names in a single string, each on a new row, ordered alphabetically.

            var result = context.Authors.Where(x => x.FirstName.EndsWith(input)).OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Select(x => new {FullName = $"{x.FirstName} {x.LastName}"}).ToList();

            return string.Join(Environment.NewLine, result.Select(x => x.FullName));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var result = context.Books
                .Where(t => EF.Functions.Like(t.Title, $"%{input}%"))
                .Select(t => t.Title).OrderBy(t => t).ToList();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(x => new
                {
                    x.Title,
                    x.Author.FirstName,
                    x.Author.LastName
                }).ToList();

            return string.Join(Environment.NewLine, books.Select(x => $"{x.Title} ({x.FirstName} {x.LastName})"));
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Count(t => t.Title.Count() > lengthCheck);
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var result = context.Authors.Select(x => new
            {
                Author = x.FirstName + " " + x.LastName,
                Count = x.Books.Sum(y => y.Copies)
            }).OrderByDescending(x => x.Count).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var c in result)
            {
                sb.AppendLine($"{c.Author} - {c.Count}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var result = context.Categories.Select(x => new
            {
                x.Name,
                Total = x.CategoryBooks.Sum(z => z.Book.Copies * z.Book.Price)
            }).OrderByDescending(x => x.Total).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var cat in result)
            {
                sb.AppendLine($"{cat.Name} ${cat.Total:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    Recent = c.CategoryBooks.Select(b => new
                        {
                            BookName = b.Book.Title,
                            BookYear = b.Book.ReleaseDate.Value.Year,
                            ResealeDate = b.Book.ReleaseDate
                        })
                        .OrderByDescending(x => x.ResealeDate)
                        .Take(3)
                        
                })
                .OrderBy(y => y.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Recent)
                {
                    sb.AppendLine($"{book.BookName} ({book.BookYear})");
                }
            }

            return sb.ToString().Trim();
        }

        public static int IncreasePrices(BookShopContext context)
        {
            var set = context.Books.Where(x => x.ReleaseDate.Value.Year < 2010);

            foreach (var book in set)
            {
                book.Price += 5m;
            }

            return context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var result = context.Books.Where(x => x.Copies < 4200).ToList();

            context.Books.RemoveRange(result);

            return context.SaveChanges();

            return result.Count;
        }
    }
}