using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Employees
{
    public class RegisterEmployeeViewModel
    {[Display(Name = "Position Name")]
        public string PositionName { get; set; }
        

        public string Name { get; set; }

        public int Age { get; set; }

        public int PositionId { get; set; }
        
        public string Address { get; set; }
    }
}
