using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SoftJail.DataProcessor.ExportDto;
using Formatting = Newtonsoft.Json.Formatting;

namespace SoftJail.DataProcessor
{
    using Data;
    using System;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                        .Select(po => new
                        {
                            OfficerName = po.Officer.FullName,
                            Department = po.Officer.Department.Name
                        })
                        .OrderBy(o => o.OfficerName)
                        .ToArray(),
                    TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(s => s.Officer.Salary), 2)
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            var result = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return result;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisoners = prisonersNames.Split(",");

            var allPrisoners = context.Prisoners
                .Where(p => prisoners.Contains(p.FullName))
                .Select(p => new ExportPrisonerMailsDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd"),
                    EncryptedMessages = p.Mails
                        .Select(m => new ExportMailsDto
                        {
                            Description = ReverseText(m.Description)
                        })
                        .ToArray()
                })
                .OrderBy(p=>p.Name)
                .ThenBy(p=>p.Id)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportPrisonerMailsDto[]), new XmlRootAttribute("Prisoners"));

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb),allPrisoners, new XmlSerializerNamespaces(new[]{XmlQualifiedName.Empty,}) );

            var result = sb.ToString().TrimEnd();

            return result;
        }

        private static string ReverseText(string argDescription)
        {
            return string.Concat(argDescription.Reverse());
        }
    }
}