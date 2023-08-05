using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class TrackOrderViewModel
    {
        [Required(ErrorMessage ="Tracking Id is required")]
        public string TrackingId { get; set; }
    }
}
