using System.ComponentModel.DataAnnotations;

namespace Chatto.Models
{
	public class ChangePasswordModel
	{
		[Required(ErrorMessage = "Old password is required!")]
		[DataType(DataType.Password)]
		[Display(Name = "Old password")]
		public string OldPassword { get; set; }

		[Required(ErrorMessage = "New password is required!")]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "New password confirmation is required!")]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Passwords don't match (new and confirmation)!")]
		[Display(Name = "Confirm new password")]
		public string ConfirmPassword { get; set; }
	}
}