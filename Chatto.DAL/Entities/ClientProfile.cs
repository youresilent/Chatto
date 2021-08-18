using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chatto.DAL.Entities
{
	public class ClientProfile
	{
		[Key]
		[ForeignKey("ApplicationUser")]
		public string Id { get; set; }

		public string RealName { get; set; }
		public string Adress { get; set; }
		public string Gender { get; set; }
		public int Age { get; set; }

		public virtual ApplicationUser ApplicationUser { get; set; }
	}
}