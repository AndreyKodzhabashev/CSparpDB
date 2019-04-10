using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VaporStore.Data.Models.Attributes
{
    public class CardNymberMatchAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var number = value.ToString().Split().ToArray();

            if (number.Length != 4)
            {
                return false;
            }

            foreach (var num in number)
            {
                foreach (var s in num)
                {
                    if (Char.IsDigit(s) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}