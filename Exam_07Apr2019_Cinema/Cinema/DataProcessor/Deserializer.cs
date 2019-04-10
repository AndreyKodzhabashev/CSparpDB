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
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using Newtonsoft.Json;
using Mapper = AutoMapper.Mapper;

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
            var movies = JsonConvert.DeserializeObject<Movie[]>(jsonString);
            var validMovies = new List<Movie>();
            var sb = new StringBuilder();
            foreach (var movie in movies)
            {
                bool isValid = IsValid(movie, out var validationResults);
                bool IsGenreValid = movie.Genre != 0;


                if (isValid
                    && IsGenreValid)
                {
                    validMovies.Add(movie);
                    sb.AppendLine(string.Format(SuccessfulImportMovie, movie.Title, movie.Genre,
                        movie.Rating.ToString("F2")));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Movies.AddRange(validMovies);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var halls = JsonConvert.DeserializeObject<ImportHallDto[]>(jsonString);

            var validHalls = new List<Hall>();

            var sb = new StringBuilder();

            foreach (var hall in halls)
            {
                bool isValid = IsValid(hall, out var validationResults);

                if (isValid)
                {
                    var currentHall = Mapper.Map<ImportHallDto, Hall>(hall);

                    var seats = CreateSeats(hall.Seats, currentHall);

                    var hallType = CreateType(hall.Is3D, hall.Is4Dx);

                    validHalls.Add(currentHall);

                    sb.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, hallType,
                        currentHall.Seats.Count));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Halls.AddRange(validHalls);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static string CreateType(bool hallIs3D, bool hallIs4Dx)
        {
            var hallType = "Normal";
            if (hallIs3D && hallIs4Dx)
            {
                hallType = "4Dx/3D";
            }
            else if (hallIs3D)
            {
                hallType = "3D";
            }
            else if (hallIs4Dx)
            {
                hallType = "4Dx";
            }

            return hallType;
        }

        private static Hall CreateSeats(int hallSeats, Hall hall)
        {
            for (int i = 0; i < hallSeats; i++)
            {
                hall.Seats.Add(new Seat
                {
                    Hall = hall
                });
            }

            return hall;
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportProjectionDto[]), new XmlRootAttribute("Projections"));

            var projectionDtos = (ImportProjectionDto[]) serializer.Deserialize(new StringReader(xmlString));

            var validProjections = new List<Projection>();

            var sb = new StringBuilder();

            var moviesIds = context.Movies.Select(m => m.Id).ToArray();
            var hallsIds = context.Halls.Select(h => h.Id).ToArray();

            foreach (var dto in projectionDtos)
            {
                bool isValid = IsValid(dto, out var validationResults);
                bool isHallValid = hallsIds.Contains(dto.HallId);
                bool isMovieValid = moviesIds.Contains(dto.MovieId);
                bool isDateNull = String.IsNullOrEmpty(dto.DateTime) ||
                                  String.IsNullOrWhiteSpace(dto.DateTime);
                if (isValid && isMovieValid && isHallValid && isDateNull == false)
                {
                    var currentProjection = Mapper.Map<Projection>(dto);

                    var currentMovieTitle = context.Movies.Find(currentProjection.MovieId).Title;

                    validProjections.Add(currentProjection);
                    sb.AppendLine(string.Format(SuccessfulImportProjection, currentMovieTitle,
                        currentProjection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Projections.AddRange(validProjections);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerTicketsDto[]), new XmlRootAttribute("Customers"));

            var customerDtos = (ImportCustomerTicketsDto[]) serializer.Deserialize(new StringReader(xmlString));

            var validCustomers = new List<Customer>();

            var sb = new StringBuilder();

            foreach (var dto in customerDtos)
            {
                bool isValid = IsValid(dto, out var validationResults);
                bool isValidTickets = dto.Tickets.All(x => IsValid(x, out var validationResults1));

                var projectionIds = context.Projections.Select(p => p.Id).ToArray();
                bool isProjectionValid = dto.Tickets.All(x => projectionIds.Contains(x.ProjectionId));

                if (isValid && isValidTickets && isProjectionValid)
                {
                    var currentCustomer = Mapper.Map<ImportCustomerTicketsDto, Customer>(dto);

                    currentCustomer.Tickets = CreateTickets(dto.Tickets, currentCustomer);

                    validCustomers.Add(currentCustomer);

                    sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, currentCustomer.FirstName,
                        currentCustomer.LastName,
                        currentCustomer.Tickets.Count));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Customers.AddRange(validCustomers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static ICollection<Ticket> CreateTickets(ImportTicketDto[] dtoTickets, Customer currentCustomer)
        {
            var tickets = new List<Ticket>();

            foreach (var dtoTicket in dtoTickets)
            {
                tickets.Add(new Ticket
                {
                    ProjectionId = dtoTicket.ProjectionId,
                    Price = dtoTicket.Price
                });
            }

            return tickets;
        }


        public static bool IsValid(object entity, out List<ValidationResult> validationResults)
        {
            var context = new ValidationContext(entity);
            validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(entity, context, validationResults, true);
        }
    }
}