using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chatto.Models
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "User name is required!")]
		[Display(Name = "Login")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Password is required!")]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirming password is required!")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords do not match!")]
		[Display(Name = "Confirm password")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "First name is required!")]
		[Display(Name = "First name")]
		public string RealName { get; set; }

		[Required(ErrorMessage = "E-mail adress is required!")]
		[Display(Name = "E-mail adress")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Adress is required!")]
		[Display(Name = "Adress")]
		public string Adress { get; set; }

		[Required(ErrorMessage = "Gender is required!")]
		[Display(Name = "Gender")]
		public string Gender { get; set; }

		[Required(ErrorMessage = "Age is required!")]
		[Display(Name = "Age")]
		[Range(1, 120, ErrorMessage = "Wrong age input!")]
		public int Age { get; set; }
	}
}