using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Cinema.Data.Models;
using Cinema.Data.Models.Enums;
using Cinema.DataProcessor.ImportDto;
using Newtonsoft.Json;

namespace Cinema.DataProcessor
{
    using System;
    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2}!";

        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";

        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";

        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var jsonDto = JsonConvert.DeserializeObject<ImportMovieDto[]>(jsonString);

            foreach (var movie in jsonDto)
            {
                var isValid = IsValid(movie, out var validationResults);
                var isMovieIn = context.Movies.FirstOrDefault(x => x.Title == movie.Title);
                var isGenreValid = Enum.TryParse<Genre>(movie.Genre, out Genre genre);

                if (isValid
                    && isMovieIn == null
                    && isGenreValid)
                {
                    var currentMovie = new Movie
                    {
                        Genre = genre,
                        Title = movie.Title,
                        Rating = movie.Rating,
                        Director = movie.Director,
                        Duration = movie.Duration
                    };

                    context.Movies.Add(currentMovie);
                    context.SaveChanges();

                    sb.AppendLine(string.Format(SuccessfulImportMovie, movie.Title, movie.Genre,
                        movie.Rating.ToString("F2")));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var hallDtos = JsonConvert.DeserializeObject<ImportHallDto[]>(jsonString);

            foreach (var dto in hallDtos)
            {
                var isValid = IsValid(dto, out var validationResults);

                if (isValid)
                {
                    var currentHall = new Hall
                    {
                        Is3D = dto.Is3D,
                        Is4Dx = dto.Is4Dx,
                        Name = dto.Name,
                    };

                    var seatsCollection = new List<Seat>();

                    for (int i = 0; i < dto.Seats; i++)
                    {
                        seatsCollection.Add(new Seat
                        {
                            
                            Hall = currentHall
                        });
                    }

                    currentHall.Seats = seatsCollection;

                    context.Halls.Add(currentHall);
                    context.SaveChanges();


                    string type = currentHall.Is3D == true ? "3D" : currentHall.Is4Dx == true ? "4Dx" : "Normal";
                    sb.AppendLine(string.Format(SuccessfulImportHallSeat, currentHall.Name, type,
                        currentHall.Seats.Count));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportProjectionDto[]), new XmlRootAttribute("Projections"));

            var projectionDtos = (ImportProjectionDto[]) serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var moviesIds = context.Movies.Select(x => x.Id).ToArray();
            var hallIds = context.Halls.Select(x => x.Id).ToArray();

            foreach (var dto in projectionDtos)
            {
                bool isValid = IsValid(dto, out var validationResults);

                bool isMovieValid = moviesIds.Contains(dto.MovieId);
                bool isHallValid = hallIds.Contains(dto.HallId);
                var dateTime = DateTime.ParseExact(dto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                if (isValid
                    && isMovieValid
                    && isHallValid)
                {
                    var currentProjection = new Projection
                    {
                        HallId = dto.HallId,
                        MovieId = dto.MovieId,
                        DateTime = dateTime
                    };

                    context.Projections.Add(currentProjection);
                    context.SaveChanges();

                    var result = string.Format(SuccessfulImportProjection, currentProjection.Movie.Title,
                        currentProjection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));

                    sb.AppendLine(result);
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));

            var customerDtos = (ImportCustomerDto[]) serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            foreach (var dto in customerDtos)
            {
                bool isValid = IsValid(dto, out var validationResults);

                bool isValidTickets = dto.Tickets.All(x => IsValid(x, out var validationResults1));

                if (isValid
                    && isValidTickets)
                {
                    var customer = new Customer
                    {
                        Age = dto.Age,
                        Balance = dto.Balance,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Tickets = dto.Tickets.Select(t=>new Ticket
                        {
                            ProjectionId = t.ProjectionId,
                            Price = t.Price
                        }).ToArray()
                    };

                    context.Customers.Add(customer);
                    context.SaveChanges();

                    var result = string.Format(SuccessfulImportCustomerTicket, customer.FirstName, customer.LastName,
                        customer.Tickets.Count);

                    sb.AppendLine(result);
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object testObj, out List<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();

            var context = new ValidationContext(testObj);

            return Validator.TryValidateObject(testObj, context, validationResults, validateAllProperties: true);
        }
    }
}