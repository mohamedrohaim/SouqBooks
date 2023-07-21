using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class CompanyViewModel
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "Address must be like 123-City-Country")]
        public string Address { get; set; }
        [Required(ErrorMessage ="Phone Number is required")]
        public string PhoneNumber { get; set; }
    }
}
