using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HappyShare.Models.AccountViewModels
{
    public class AccountViewModelBase
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public virtual string Email { get; set; }
        
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        [Display(Name = "Phone Number(Home)")]
        [Remote(action: "VerifyPhone", controller: "Account", AdditionalFields = "WorkPhoneNumber, MobilePhoneNumber", HttpMethod = "POST", ErrorMessage = "At least one phone number is required.")]
        public virtual string HomePhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        [Display(Name = "Phone Number(work)")]
        public virtual string WorkPhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        [Display(Name = "Phone Number(Mobile)")]
        public virtual string MobilePhoneNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Address")]
        public virtual string Address { get; set; }
    }
}
