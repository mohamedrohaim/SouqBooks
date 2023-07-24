using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ValidateNever]
        public Product product { get; set; }
        [Range(0, 500)]
        public int Count { get; set; }

        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser applicationUser { get; set; }
    
    
    }
}
