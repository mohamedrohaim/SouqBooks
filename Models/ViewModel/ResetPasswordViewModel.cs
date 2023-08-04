using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Compare("NewPassword",ErrorMessage ="Confitm password must match new password")]
		[Required(ErrorMessage = "Confirm password is required")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }


    }
}
