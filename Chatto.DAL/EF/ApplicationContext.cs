using Chatto.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Chatto.DAL.EF
{
	public class ApplicationContext : IdentityDbContext<ApplicationUser>
	{
		//public ApplicationContext(string connectionString) : base(connectionString)
		//{ }

		public ApplicationContext() : base("name=IdentityDb") { }

		public DbSet<ClientProfile> ClientProfiles { get; set; }
		public DbSet<ClientMessage> ClientMessages { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ApplicationUser>()
				.HasRequired(c => c.ClientProfile)
				.WithRequiredPrincipal(c => c.ApplicationUser);

			//modelBuilder.Entity<ClientProfile>()
			//	.HasMany(c => c.ClientMessages)
			//	.WithRequired(c => c.ClientProfile)
			//	.HasForeignKey(c => c.Sender);

			base.OnModelCreating(modelBuilder);
		}
	}
}
