using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VaporStore.Data.Models.Attributes
{
    public class FullNameMatchAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var stringObject = value.ToString();
            var testObject = stringObject.Split().ToArray();

            if (testObject.Length != 2)
            {
                return false;
            }

            foreach (var word in testObject)
            {
                if (Char.IsUpper(word[0])==false)
                {
                    return false;
                }

                for (int i = 1; i < word.Length; i++)
                {
                    var testedChar = word[i];

                    if (Char.IsLetter(testedChar) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}