using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HappyShare.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number(Home)")]
        public string HomePhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number(work)")]
        public string WorkPhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number(Mobile)")]
        public string MobilePhoneNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(100)]
        public string Address { get; set; }

        public bool Enabled { get; set; }
    }

}
