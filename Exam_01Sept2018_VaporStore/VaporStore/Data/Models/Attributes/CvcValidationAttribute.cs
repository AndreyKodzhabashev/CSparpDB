using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VaporStore.Data.Models.Attributes
{
    public class CvcValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var testedValue = value.ToString();
            if (testedValue.Length != 3)
            {
                return false;
            }

            foreach (var c in testedValue)
            {
                if (Char.IsDigit(c) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}