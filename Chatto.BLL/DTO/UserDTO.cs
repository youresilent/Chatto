using System.ComponentModel.DataAnnotations;

namespace Chatto.BLL.DTO
{
	public class UserDTO
	{
		public string Id { get; set; }

		[Display(Name = "User name")]
		public string UserName { get; set; }
		public string Password { get; set; }

		[Display(Name = "Real name")]
		public string RealName { get; set; }

		[Display(Name = "E-mail adress")]
		public string Email { get; set; }

		[Display(Name = "Adress")]
		public string Adress { get; set; }

		[Display(Name = "Gender")]
		public string Gender { get; set; }

		[Display(Name = "Age")]
		public int Age { get; set; }

		//public string Friends { get; set; }

		public string Role { get; set; }
	}
}
