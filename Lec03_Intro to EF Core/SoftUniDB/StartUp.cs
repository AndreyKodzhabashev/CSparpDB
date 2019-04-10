namespace SoftUni
{
    using System;
    using Data;
    using Models;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        //private const string scaffoldString =
        //    @"scaffold-dbcontext ""Server=localhost\sqlexpress;Database=SoftUni;Trusted_Connection=True;"" -Provider Microsoft.EntityFrameworkCore.SqlServer -DataAnnotations -ContextDir Data -OutputDir Models"; //when paste to PMC remove escape "

        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                //Console.WriteLine(GetEmployeesFullInformation(context));

                //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));

                //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
                //Console.WriteLine(AddNewAddressToEmployee(context));
                //Console.WriteLine(GetEmployeesInPeriod(context));

                // Console.WriteLine(GetAddressesByTown(context));//ne minava za pamet
                //Console.WriteLine(GetEmployee147(context));
                //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
                //Console.WriteLine(GetLatestProjects(context));
                // Console.WriteLine(IncreaseSalaries(context));
                //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
                //Console.WriteLine(DeleteProjectById(context));
                //Console.WriteLine(RemoveTown(context));

                var test = context.Employees.Where(e => e.AddressId == 1);

                ;


                var count = test.Count();

                ;
            }
        }


        //public static string GetEmployeesFullInformation(SoftUniContext context)
        //{
        //    StringBuilder result = new StringBuilder();
        //    var allEmployees = context.Employees;

        //    foreach (Employee employee in allEmployees.OrderBy(e => e.EmployeeId))
        //    {
        //        result.AppendLine(
        //            $"{employee.FirstName} {employee.LastName} {employee.MiddleName ?? String.Empty} {employee.JobTitle} {employee.Salary:f2}");
        //    }

        //    return result.ToString();
        //}

        //public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        //{
        //    StringBuilder result = new StringBuilder();
        //    var allEmployees = context.Employees.Where(x => x.Salary > 50000);

        //    foreach (Employee employee in allEmployees.OrderBy(e => e.FirstName))
        //    {
        //        result.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
        //    }

        //    return result.ToString();
        //}

        //public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        //{
        //    StringBuilder result = new StringBuilder();
        //    var allEmployees = context.Employees.Include(x => x.Department)
        //        .Where(x => x.Department.Name.Equals("Research and Development")).Select(x => new
        //        {
        //            x.FirstName,
        //            x.LastName,
        //            Department = x.Department.Name,
        //            x.Salary
        //        });

        //    foreach (var employee in allEmployees.OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName))
        //    {
        //        result.AppendLine(
        //            $"{employee.FirstName} {employee.LastName} from {employee.Department} - ${employee.Salary:f2}");
        //    }

        //    return result.ToString();
        //}

        //public static string AddNewAddressToEmployee(SoftUniContext context)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    string address = "Vitoshka 15";

        //    context.Addresses.Add(new Address()
        //    {
        //        AddressText = address,
        //        TownId = 4
        //    });
        //    context.SaveChanges();
        //    var lastAddress = context.Addresses.Last();
        //    var temp = context.Employees.FirstOrDefault(x => x.LastName.Equals("Nakov"));
        //    temp.AddressId = lastAddress.AddressId;

        //    context.SaveChanges();

        //    var test = context.Employees.Include(x => x.Address).OrderByDescending(x => x.AddressId).Take(10).ToArray();

        //    foreach (var t in test)
        //    {
        //        sb.AppendLine(t.Address.AddressText);
        //    }

        //    return sb.ToString();
        //}

        //public static string GetEmployeesInPeriod(SoftUniContext context)
        //{
        //    //Find the first 10 employees who have projects started in the period 2001 - 2003(inclusive).Print each employee's first name, last name, manager’s first name and last name. Then return all of their projects in the format "--<ProjectName> - <StartDate> - <EndDate>", each on a new row. If a project has no end date, print "not finished" instead.
        //    var emp = context.Employees
        //        .Where(x => x.EmployeesProjects.Any(s => s.Project.StartDate.Year >= 2001 &&
        //                                                 s.Project.StartDate.Year <= 2003))
        //        .Select(x => new
        //        {
        //            EmployeeFullName = x.FirstName + " " + x.LastName,
        //            ManagerFullName = x.Manager.FirstName + " " + x.Manager.LastName,
        //            Projects = x.EmployeesProjects.Select(p => new
        //            {
        //                ProjectName = p.Project.Name,
        //                ProjectStartDate = p.Project.StartDate,
        //                ProjectEndDate = p.Project.EndDate
        //            })
        //        })
        //        .Take(10)
        //        .ToList();

        //    StringBuilder sb = new StringBuilder();
        //    foreach (var e in emp)
        //    {
        //        sb.AppendLine($"{e.EmployeeFullName} - Manager: {e.ManagerFullName}");

        //        foreach (var project in e.Projects)
        //        {
        //            var startDate =
        //                project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
        //            var endDate = project.ProjectEndDate.HasValue
        //                ? project.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
        //                : "not finished";

        //            sb.AppendLine($"--{project.ProjectName} - {startDate} - {endDate}");
        //        }
        //    }

        //    return sb.ToString().TrimEnd();
        //}

        public static string GetAddressesByTown(SoftUniContext context)
        {
            //Find all addresses, ordered by the number of employees who live there(descending), then by town name(ascending), and finally by address text(ascending).Take only the first 10 addresses.For each address return it in the format "<AddressText>, <TownName> - <EmployeeCount> employees"

            var result = context.Addresses.Include(x => x.Employees)
                .Select(s => new
                {
                    s.AddressText,
                    s.Town.Name,
                    Count = s.Employees.Count
                })
                .OrderByDescending(a => a.Count)
                .ThenBy(a => a.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToArray();


            StringBuilder sb = new StringBuilder();
            foreach (var address in result)
            {
                sb.AppendLine($"{address.AddressText}, {address.Name} - {address.Count} employees");
            }

            return sb.ToString();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            //Get the employee with id 147.Return only his/ her first name, last name, job title and projects(print only their names). The projects should be ordered by name(ascending).Format of the output.

            var result = context.Employees.Include(p => p.EmployeesProjects).Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select(p => p.Project.Name).OrderBy(p => p).ToArray()
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var emp in result)
            {
                sb.AppendLine($"{emp.FullName} - {emp.JobTitle}");
                foreach (var project in emp.Projects)
                {
                    sb.AppendLine(project);
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var result = context.Departments.Include(e => e.Employees)
                .Where(d => d.Employees.Count > 5).OrderBy(e => e.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    DepManager = d.Manager.FirstName + d.Manager.LastName,
                    Employees = d.Employees
                });

            StringBuilder sb = new StringBuilder();
            foreach (var department in result)
            {
                sb.AppendLine(
                    $"{department.DepartmentName} - {department.DepManager}");
                foreach (var employee in department.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                {
                    sb.AppendLine($"{employee.FirstName + " " + employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            //Write a program that return information about the last 10 started projects. Sort them by name lexicographically and return their name, description and start date, each on a new row. Format of the output
            //"M/d/yyyy h:mm:ss tt"
            var result = context.Projects.OrderByDescending(d => d.StartDate).Take(10)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                }).OrderBy(p => p.Name).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var project in result)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            //Write a program that increase salaries of all employees that are in the Engineering, Tool Design, Marketing or Information Services department by 12%. Then return first name, last name and salary (2 symbols after the decimal separator) for those employees whose salary was increased. Order them by first name (ascending), then by last name (ascending). Format of the output.
            var dep = new[]
            {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };

            context.Employees.Include(d => d.Departments).Where(d => new[]
                {
                    "Engineering",
                    "Tool Design",
                    "Marketing",
                    "Information Services"
                }.Contains(d.Department.Name))
                .OrderBy(f => f.FirstName).ToList().ForEach(s => s.Salary *= 1.12m);

            context.SaveChanges();

            var test1 = context.Employees.Include(d => d.Departments).Where(d => dep.Contains(d.Department.Name))
                .OrderBy(f => f.FirstName).ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var employee in test1)
            {
                sb.AppendLine($"{employee.FirstName + " " + employee.LastName} (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var result = context.Employees.Where(e => e.FirstName.StartsWith("Sa")).Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                e.Salary
            }).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var e in result.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
            {
                sb.AppendLine($"{e.FirstName + " " + e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects.Find(2);
            var empProj = context.EmployeesProjects.Where(x => x.ProjectId == 2).ToArray();
            context.EmployeesProjects.RemoveRange(empProj);
            context.Projects.RemoveRange(project);


            context.SaveChanges();
            StringBuilder sb = new StringBuilder();
            foreach (var a in context.Projects.Take(10).Select(p => new
            {
                p.Name
            }).ToList())
            {
                sb.AppendLine(a.Name);
            }

            return sb.ToString().TrimEnd();
        }

        //public static string RemoveTown(SoftUniContext context)
        //{
        //    var seattleTown = context.Towns.FirstOrDefault(t => t.Name == "Seattle");
        //    int seattleId = seattleTown.TownId;
        //    var addressId = context.Addresses.Where(x => x.TownId == seattleId).ToArray();

        //    List<Employee> test = new List<Employee>();
        //    foreach (var id in addressId)
        //    {
        //        foreach (var VARIABLE in context.Employees.Where(f => f.AddressId == id.AddressId))
        //        {
        //            test.Add(VARIABLE);
        //        }
        //    }

        //    foreach (var employee in test)
        //    {
        //        employee.AddressId = null;
        //    }

        //    context.SaveChanges();
        //    context.Addresses.RemoveRange(addressId);
        //    int result = context.SaveChanges();
        //    context.Towns.RemoveRange(seattleTown);
        //    context.SaveChanges();

        //    return $"{result} addresses in Seattle were deleted";
        //}

        public static string RemoveTown(SoftUniContext context)
        {
            string result = "";
            var town = context.Towns.Where(t => t.Name == "Seattle").FirstOrDefault();

            if (town != null)
            {
                var addresses = context.Addresses.Where(a => a.TownId == town.TownId).ToList();
                var addressesId = addresses.Select(x => x.AddressId).ToArray();

               
               // var employees = context.Employees.SelectMany(e => addresses.Select(c => c.AddressId)).Distinct().ToList();

                var test1 = context.Employees.Where(x =>addresses.FirstOrDefault(z => z.AddressId == x.AddressId).AddressId == x.AddressId).ToArray();

               // var count = employees.Count();

                foreach (var item in test1.Select(x=>x.AddressId))
                {
                    var currentEmployee = context.Employees.FirstOrDefault(e => e.AddressId == item);
                    currentEmployee.AddressId = null;
                }

                int test = context.SaveChanges();
                context.Addresses.RemoveRange(addresses);
                int count = context.SaveChanges();
                context.Towns.Remove(town);
                var townRemove = context.SaveChanges();

                result = $"{count} addresses in Seattle were deleted";
            }
            else
            {
                result = $"0 addresses in Seattle were deleted";
            }

            return result;
        }
        //public static string RemoveTown(SoftUniContext context)
        //{
        //    string result = "";
        //    string townToDelete = "Redmond";
        //    var count = 0;
        //    var town = context.Towns.Where(t => t.Name == townToDelete).FirstOrDefault();

        //    if (town != null)
        //    {
        //        var addresses = context.Addresses.Where(a => a.TownId == town.TownId).ToList();
        //        var employees = context.Employees.SelectMany(e => addresses.Select(c => c.AddressId)).Distinct().ToList();
        //        //count = addresses.Count();

        //        foreach (var item in employees)
        //        {
        //            var currentEmployee = context.Employees.Where(e => e.AddressId == item).FirstOrDefault();
        //            currentEmployee.AddressId = null;
        //        }

        //        context.SaveChanges();
        //        context.Addresses.RemoveRange(addresses);
        //        count = context.SaveChanges();
        //        context.Towns.Remove(town);
        //        context.SaveChanges();

        //        result = $"{count} addresses in {townToDelete} were deleted";
        //    }
        //    else
        //    {
        //        result = $"{count} addresses in {townToDelete} were deleted";
        //    }

        //    return result;
        //}
    }
}