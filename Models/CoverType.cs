using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class CoverType
	{
		public int Id { get; set; }
		[Display(Name="Cover Type")]
		[Required(ErrorMessage ="Name is Required")]
		[MaxLength(50)]
		public string Name  { get; set; }
	}
}
