using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class UserViewModel
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage ="First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email Number is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "Address must be like 123-City-Country")]
        public string Address { get; set; }
        public List<string>? UserRoles { get; set; }
        //[ValidateNever]
        //public IEnumerable<SelectListItem> RolesList { get; set; }

    }
}
