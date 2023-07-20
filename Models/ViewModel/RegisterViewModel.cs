using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class RegisterViewModel
    {
         
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is Reqired")]
        [Compare("Password", ErrorMessage = "Confirm Password does not match password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [ValidateNever]
        public string ProfileimageUrl { get; set; }


        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "Address must be like 123-City-Country")]
        public string Address { get; set; }
        public bool IsAgree { get; set; }
    }
}
