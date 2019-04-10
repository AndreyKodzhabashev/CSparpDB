using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PetClinic.Models.Attributes
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var numToTest = value.ToString();

            if (value.ToString().StartsWith("+359"))
            {
                var temp = numToTest.ToString().Skip(4).Take(50).ToArray();

                if (numToTest.Length != 9)
                {
                    return false;
                }

                foreach (var s in numToTest)
                {
                    if (Char.IsDigit(s) == false)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (numToTest.Length != 10)
                {
                    return false;
                }
                else if (numToTest[0] != '0')
                {
                    return false;
                }
                else
                {
                    if (numToTest.All(x => Char.IsDigit(x)) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}