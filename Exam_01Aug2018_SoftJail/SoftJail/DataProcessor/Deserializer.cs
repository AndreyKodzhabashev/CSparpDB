using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SoftJail.Data.Models;
using SoftJail.Data.Models.Enums;
using SoftJail.DataProcessor.ImportDto;

namespace SoftJail.DataProcessor
{
    using Data;
    using System;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departments = JsonConvert.DeserializeObject<Department[]>(jsonString);

            var sb = new StringBuilder();
            var validDepartments = new List<Department>();
            foreach (var department in departments)
            {
                var isDptValid = IsValid(department, out var validationResults1);

                var isCellValid = department.Cells.All(x => IsValid(x, out var validationResults2));

                if (isDptValid && isCellValid)
                {
                    validDepartments.Add(department);

                    sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
                }
                else
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
            }

            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonersDto = JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString);

            var sb = new StringBuilder();

            var validPrisoners = new List<Prisoner>();

            foreach (var dto in prisonersDto)
            {
                bool isPrisonerValid = IsValid(dto, out var validationResults1);
                bool isMailValid = dto.Mails.All(x => IsValid(x, out var validationResults2));


                if (isPrisonerValid && isMailValid)
                {
                    var currentPrisoner = new Prisoner
                    {
                        CellId = dto.CellId,
                        Age = dto.Age,
                        Bail = dto.Bail,
                        FullName = dto.FullName,
                        IncarcerationDate = DateTime.ParseExact(dto.IncarcerationDate, "dd/MM/yyyy",
                            CultureInfo.InvariantCulture),
                        ReleaseDate = dto.ReleaseDate == null
                            ? (DateTime?) null
                            : DateTime.ParseExact(dto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Nickname = dto.Nickname,
                        Mails = dto.Mails.Select(m => new Mail
                            {
                                Address = m.Address,
                                Sender = m.Sender,
                                Description = m.Description
                            })
                            .ToArray()
                    };

                    validPrisoners.Add(currentPrisoner);

                    sb.AppendLine($"Imported {currentPrisoner.FullName} {currentPrisoner.Age} years old");
                }
                else
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
            }

            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var validOfficers = new List<Officer>();

            var serializer = new XmlSerializer(typeof(ImportOfficerDto[]), new XmlRootAttribute("Officers"));

            var officerDtos = (ImportOfficerDto[]) serializer.Deserialize(new StringReader(xmlString));


            foreach (var dto in officerDtos)
            {
                bool isValidOfficer = IsValid(dto, out var validationResult3);
                bool isValidPosition = Enum.TryParse<Position>(dto.Position, out var position);
                bool isValidWeapon = Enum.TryParse<Weapon>(dto.Weapon, out var weapon);

                if (isValidOfficer
                    && isValidPosition
                    && isValidWeapon
                )
                {
                    var officer = new Officer
                    {
                        FullName = dto.Name,
                        Salary = dto.Money,
                        Position = position,
                        Weapon = weapon,
                        DepartmentId = dto.DepartmentId,
                        OfficerPrisoners = dto.Prisoners
                            .Select(x => new OfficerPrisoner
                            {
                                PrisonerId = x.Id
                            })
                            .ToArray()
                    };

                    validOfficers.Add(officer);

                    sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
                }
                else
                {
                    sb.AppendLine("Invalid Data");
                }
            }

            context.Officers.AddRange(validOfficers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object testObject, out List<ValidationResult> validationResults)
        {
            var validationContext = new ValidationContext(testObject);
            validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(testObject, validationContext, validationResults,
                validateAllProperties: true);
        }
    }
}