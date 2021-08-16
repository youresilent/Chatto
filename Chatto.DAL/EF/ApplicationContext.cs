using Chatto.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Chatto.DAL.EF
{
	public class ApplicationContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationContext(string connectionString) : base(connectionString)
		{ }

		public DbSet<ClientProfile> ClientProfiles { get; set; }
	}
}
