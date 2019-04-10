using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models.Attributes
{
    public class PurchaseKeyMatchAttribute : ValidationAttribute

    {
        public override bool IsValid(object value)
        {
            //which consists of 3 pairs of 4 uppercase Latin letters and digits, separated by dashes (ex. “ABCD-EFGH-1J3L”) (required)
            var testedValue = value.ToString();
            string[] testArray = testedValue.Split('-');

            if (testArray.Length != 3)
            {
                return false;
            }

            foreach (var element in testArray)
            {
                if (element.Length != 4)
                {
                    return false;
                }

                foreach (var c in element)
                {
                    if (char.IsDigit(c) == false)
                    {
                        if (char.IsLetter(c) && char.IsUpper(c) == false)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}