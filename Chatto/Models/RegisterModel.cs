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
		[Display(Name = "Имя пользователя")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароли не совпадают!")]
		[Display(Name = "Подтвердите пароль")]
		public string ConfirmPassword { get; set; }

		[Required]
		[Display(Name = "Настоящее имя")]
		public string RealName { get; set; }

		[Required]
		[Display(Name = "Электронная почта")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Адрес")]
		public string Address { get; set; }

		[Required]
		[Display(Name = "Пол")]
		public string Gender { get; set; }

		[Required]
		[Display(Name = "Возраст")]
		public int Age { get; set; }
	}
}