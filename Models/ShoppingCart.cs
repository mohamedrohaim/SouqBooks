﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Range(1, 500)]
        public int Count { get; set; }
        [ValidateNever]
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser applicationUser { get; set; }

        [NotMapped]
        [ValidateNever]
        public double PriceBasedOnCount { get; set; }


    }
}
