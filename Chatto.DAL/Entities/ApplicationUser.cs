using Microsoft.AspNet.Identity.EntityFramework;

namespace Chatto.DAL.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public virtual ClientProfile ClientProfile { get; set; }
	}
}
