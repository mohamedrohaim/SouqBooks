using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ProductViewModel
    {
        public Product product { get; set; }

        public IEnumerable<SelectListItem> CatecoryList { get; set; }


        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
    }
}
