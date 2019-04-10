using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PetClinic.Data.ModelDtos.Import;
using PetClinic.Models;

namespace PetClinic.DataProcessor
{
    using System;
    using PetClinic.Data;

    public class Deserializer
    {
        private const string errorMessage = "Error: Invalid data.";

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            var animalAids = JsonConvert.DeserializeObject<AnimalAid[]>(jsonString);
            var validAids = new List<AnimalAid>();

            var sb = new StringBuilder();
            foreach (var animalAid in animalAids)
            {
                var isValid = IsValid(animalAid, out var validationResults);
                var isExisting = context.AnimalAids.FirstOrDefault(a => a.Name == animalAid.Name);
                if (isValid && isExisting == null)
                {
                    context.AnimalAids.Add(animalAid);
                    context.SaveChanges();
                    sb.AppendLine($"Record {animalAid.Name} successfully imported.");
                }
                else
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            var animalDtos = JsonConvert.DeserializeObject<ImportAnimalDto[]>(jsonString);
            var sb = new StringBuilder();

            var validAnimals = new List<Animal>();

            foreach (var animalDto in animalDtos)
            {
                bool isValidAnimal = IsValid(animalDto, out var validationResults);
                bool isValidPassport = IsValid(animalDto.Passport, out var validationResults1);

                bool isPassportExists = context.Passports.Any(p => p.SerialNumber == animalDto.Passport.SerialNumber);


                if (isValidAnimal
                    && isValidPassport
                    && isPassportExists == false)
                {
                    var animal = new Animal
                    {
                        Name = animalDto.Name,
                        Type = animalDto.Type,
                        Age = animalDto.Age,
                        Passport = new Passport
                        {
                            SerialNumber = animalDto.Passport.SerialNumber,
                            OwnerName = animalDto.Passport.OwnerName,
                            OwnerPhoneNumber = animalDto.Passport.OwnerPhoneNumber,
                            RegistrationDate = DateTime.ParseExact(animalDto.Passport.RegistrationDate, "dd-MM-yyyy",
                                CultureInfo.InvariantCulture)
                        }
                    };

                    context.Animals.Add(animal);
                    context.SaveChanges();

                    sb.AppendLine(
                        $"Record {animal.Name} Passport №: {animal.Passport.SerialNumber} successfully imported.");
                }
                else
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            var inputXml =
                @"<Vets><Vet><Name>InsertMe</Name><Profession>Valid Profession</Profession><Age>45</Age><PhoneNumber>0897665544</PhoneNumber></Vet><Vet><Name>DontInsertMe</Name><Profession>Valid Profession</Profession><Age>45</Age><PhoneNumber>0897665544</PhoneNumber></Vet></Vets>";
            var serializer = new XmlSerializer(typeof(ImportVetDto[]), new XmlRootAttribute("Vets"));

            var importVetDtos = (ImportVetDto[]) serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var validVets = new List<Vet>();

            foreach (var dto in importVetDtos)
            {
                bool isValid = IsValid(dto, out var validationResults);
                bool isFreePhoneNumber = context.Vets.Any(x => x.PhoneNumber == dto.PhoneNumber);
                if (isValid
                    && isFreePhoneNumber == false)
                {
                    var vet = new Vet
                    {
                        Name = dto.Name,
                        Profession = dto.Profession,
                        Age = dto.Age,
                        PhoneNumber = dto.PhoneNumber
                    };

                    context.Vets.Add(vet);
                    context.SaveChanges();

                    sb.AppendLine($"Record {vet.Name} successfully imported.");
                }
                else
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
            }


            return sb.ToString().TrimEnd();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            var serizlizer = new XmlSerializer(typeof(ImportProceduresDto[]), new XmlRootAttribute("Procedures"));

            var proceduresDtos = (ImportProceduresDto[]) serizlizer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();
            var aidNames = context.AnimalAids.Select(a => a.Name).ToArray();
            foreach (var dto in proceduresDtos)
            {
                var vet = context.Vets.FirstOrDefault(v => v.Name == dto.Vet);
                var animal = context.Animals.FirstOrDefault(a => a.PassportSerialNumber == dto.Animal);

                bool isAidValid = dto.AnimalAids.All(x => aidNames.Contains(x.Name));

                var originalAids = dto.AnimalAids.Select(x => x.Name).ToArray();
                var aidsAfterCheck = originalAids.Distinct().ToArray();

                bool isAidUnique = originalAids.Length - aidsAfterCheck.Length == 0;

                if (vet != null
                    && animal != null
                    && isAidValid
                    && isAidUnique)
                {
                    var procedure = new Procedure
                    {
                        AnimalId = animal.Id,
                        VetId = vet.Id,
                        DateTime = DateTime.ParseExact(dto.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        ProcedureAnimalAids = dto.AnimalAids
                            .Select(x => new ProcedureAnimalAid
                            {
                                AnimalAidId = context.AnimalAids.SingleOrDefault(z => z.Name == x.Name).Id
                            }).ToArray()
                    };

                    context.Procedures.Add(procedure);
                    context.SaveChanges();

                    sb.AppendLine("Record successfully imported.");
                }
                else
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
            }
            
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity, out List<ValidationResult> validationResults)
        {
            var context = new ValidationContext(entity);
            validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(entity, context, validationResults, validateAllProperties: true);
        }
    }
}