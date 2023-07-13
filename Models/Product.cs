using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Title is Required")]

        public string Title { get; set; }

        [Required(ErrorMessage = "Description is Required")]

        public string Description { get; set; }

        [Required(ErrorMessage = "ISBN is Required")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Author is Required")]

        public string Author { get; set; }
        [Required(ErrorMessage = "Price is Required")]
        [Range(1, 100000)]

        public double Price { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Category is Required")]
        [Display(Name = "Catrgory")]
        public int CategoryId { get; set; }

        public Category category { get; set; }
        [Required(ErrorMessage = "Cover Type is Required")]

        [Display(Name = "Cover Type")]
        public int CoverTypeId { get; set; }
        public CoverType coverType { get; set; }
    }
}
