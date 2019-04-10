namespace BillsPaymentSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; } //(up to 50 characters, unicode)

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; } //(up to 50 characters, unicode)

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                           @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                           @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        public string Email { get; set; } //(up to 80 characters, non-unicode)

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; } //(up to 25 characters, non-unicode)

        //navig
        public ICollection<PaymentMethod> PaymentMethods { get; set; }

        public User()
        {
            this.PaymentMethods = new List<PaymentMethod>();
        }
    }
}