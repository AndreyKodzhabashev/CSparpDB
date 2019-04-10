using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Cinema.DataProcessor.ExportDto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Cinema.DataProcessor
{
    using System;
    using Data;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
                .Where(x => x.Rating >= rating
                            && x.Projections.Any(pr => pr.Tickets.Count > 0))
                .Select(c=>new
                {
                    MovieName = c.Title,
                    Rating = c.Rating.ToString("F2"),
                    TotalIncomes = c.Projections.SelectMany(y => y.Tickets).Sum(t => t.Price).ToString("F2"),
                    Customers = c.Projections
                        .SelectMany(x=>x.Tickets.Select(z=>z.Customer))
                        
                        .Select(x=>new
                        {
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Balance = x.Balance.ToString("F2")
                        })
                        .OrderByDescending(x => x.Balance)
                        .ThenBy(x => x.FirstName + " " + x.LastName)
                        
                        .ToArray()
                })
               .OrderByDescending(x => double.Parse(x.Rating))
                .ThenByDescending(x => decimal.Parse(x.TotalIncomes))
                .Take(10)
                .ToArray();

            var result = JsonConvert.SerializeObject(movies, Formatting.Indented);

            return result;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customersTemp = context.Customers
                .Include(t => t.Tickets)
                .ThenInclude(x => x.Projection)
                .ThenInclude(x => x.Movie)
                .ThenInclude(t => t.Duration)
                .Where(x => x.Age >= age)
                .Select(c => new
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    SpentMoney = c.Tickets.Select(x => x.Price).Sum(),
                    SpentTime = new TimeSpan(c.Tickets.Sum(t => t.Projection.Movie.Duration.Ticks))
                })
                .OrderByDescending(x => x.SpentMoney)
                .Take(10)
                .ToArray();

            var customers = customersTemp
                .Select(c => new ExportCustomerDto
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    SpentMoney = c.SpentMoney.ToString("F2"),
                    SpentTime = c.SpentTime.ToString("hh\\:mm\\:ss")
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCustomerDto[]), new XmlRootAttribute("Customers"));
            var sb = new StringBuilder();
            serializer.Serialize(new StringWriter(sb), customers,
                new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty}));

            return sb.ToString().TrimEnd();
        }
    }
}