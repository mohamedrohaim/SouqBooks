using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public Product product { get; set; }

        [Required(ErrorMessage = "Enter Number Of copies!"), Range(1, 100, ErrorMessage = "Number Of copies between 1 to 100")]
        public int Count { get; set; }

    }
}
