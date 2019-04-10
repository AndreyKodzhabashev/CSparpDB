using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PetClinic.Data.ModelDtos.Export;
using Formatting = Newtonsoft.Json.Formatting;

namespace PetClinic.DataProcessor
{
    using System;

    using PetClinic.Data;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var result = context.Passports
                .Where(p => p.OwnerPhoneNumber == phoneNumber)
                .Select(x => new
                {
                    OwnerName = x.OwnerName,
                    AnimalName = x.Animal.Name,
                    Age = x.Animal.Age,
                    SerialNumber = x.SerialNumber,
                    RegisteredOn = x.RegistrationDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                })
                .OrderBy(x => x.Age)
                .ThenBy(x => x.SerialNumber)
                .ToArray();

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            var result = context.Procedures
                .Select(p => new ExportProcedureDto()
                {
                    Passport = p.Animal.Passport.SerialNumber,
                    OwnerNumber = p.Animal.Passport.OwnerPhoneNumber,
                    DateTime = p.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    AnimalAids = p.ProcedureAnimalAids
                        .Select(pr => new ExportAnimalAidDto
                        {
                            Name = pr.AnimalAid.Name,
                            Price = pr.AnimalAid.Price.ToString("F2")
                        })
                        .ToArray(),
                    TotalPrice = p.ProcedureAnimalAids.Sum(a=>a.AnimalAid.Price).ToString("F2")
                })
                .OrderBy(x=>x.DateTime)
                .ThenBy(x=>x.Passport)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportProcedureDto[]), new XmlRootAttribute("Procedures"));

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb),result,new XmlSerializerNamespaces(new[]{XmlQualifiedName.Empty} ) );

            return sb.ToString().TrimEnd();
        }
    }
}
