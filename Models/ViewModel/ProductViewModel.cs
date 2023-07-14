using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ProductViewModel
    {
        public Product product { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem> CatecoryList { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem> CoverTypeList { get; set; }
    }
}
