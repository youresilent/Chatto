using System.ComponentModel.DataAnnotations;

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