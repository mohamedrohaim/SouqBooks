using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required,MaxLength(100)]
        public string  FirstName { get; set; }
        [Required,MaxLength(100)]
        public string  LastName { get; set; }

        public string ProfileimageUrl { get; set; }
        public string Address { get; set; }
        public bool IsAgree { get; set; }


    }
}
