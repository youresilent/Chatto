using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chatto.Models
{
	public class RegisterModel
	{
		[Required]
		[Display(Name = "Login")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords do not match!")]
		[Display(Name = "Confirm password")]
		public string ConfirmPassword { get; set; }

		[Required]
		[Display(Name = "First name")]
		public string RealName { get; set; }

		[Required]
		[Display(Name = "E-mail address")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Adress")]
		public string Adress { get; set; }

		[Required]
		[Display(Name = "Gender")]
		public string Gender { get; set; }

		[Required]
		[Display(Name = "Age")]
		public int Age { get; set; }
	}
}