using System.ComponentModel.DataAnnotations;

namespace Chatto.BLL.DTO
{
	public class UserDTO
	{
		public string Id { get; set; }
		
		[Display(Name = "Имя пользователя")]
		public string UserName { get; set; }
		public string Password { get; set; }

		[Display(Name = "Настоящее имя")]
		public string RealName { get; set; }
		
		[Display(Name = "Электронная почта")]
		public string Email { get; set; }
		
		[Display(Name = "Адрес")]
		public string Adress { get; set; }

		[Display(Name = "Пол")] 
		public string Gender { get; set; }

		[Display(Name = "Возраст")]
		public int Age { get; set; }

		public string Role { get; set; }
	}
}
