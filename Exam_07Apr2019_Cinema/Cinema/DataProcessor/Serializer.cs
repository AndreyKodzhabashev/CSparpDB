using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Cinema.DataProcessor.ExportDto;
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
                .Where(m => m.Rating >= rating && m.Projections.Any(x => x.Tickets.Count > 0))
                .Select(x => new
                {
                    MovieName = x.Title,
                    Rating = x.Rating.ToString("F2"),
                    TotalIncomes = x.Projections.Select(t => t.Tickets.Sum(p => p.Price)).Sum().ToString("F2"),
                    Customers = x.Projections.SelectMany(p => p.Tickets).Select(z => z.Customer).Select(cu => new
                        {
                            FirstName = cu.FirstName,
                            LastName = cu.LastName,
                            Balance = cu.Balance.ToString("F2")
                        })
                        .OrderByDescending(cu => cu.Balance)
                        .ThenBy(cu => cu.FirstName)
                        .ThenBy(cu => cu.LastName)
                        .ToArray()
                })
                .OrderByDescending(m => double.Parse(m.Rating))
                .ThenByDescending(m => decimal.Parse(m.TotalIncomes))
                .Take(10)
                .ToArray();

            var result = JsonConvert.SerializeObject(movies, Formatting.Indented);

            return result;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context.Customers
                .Where(c => c.Age >= age)
                .Select(x => new ExportCustomerDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SpentMoney = x.Tickets.Sum(p => p.Price).ToString("F2"),
                    SpentTime =
                        new TimeSpan(x.Tickets.Sum(p => p.Projection.Movie.Duration.Ticks)).ToString("hh\\:mm\\:ss")
                })
                .OrderByDescending(x=>decimal.Parse(x.SpentMoney))
                .Take(10)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCustomerDto[]), new XmlRootAttribute("Customers"));
            var sb = new StringBuilder();
            serializer.Serialize(new StringWriter(sb), customers,
                new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty}));

            return sb.ToString().TrimEnd();
        }
    }
}