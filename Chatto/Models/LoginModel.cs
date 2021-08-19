using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chatto.Models
{
	public class LogInModel
	{
		[Required(ErrorMessage = "Login is required!")]
		[Display(Name = "Login")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Password is required!")]
		[Display(Name = "Password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}