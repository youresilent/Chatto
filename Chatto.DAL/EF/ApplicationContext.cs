using Chatto.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Chatto.DAL.EF
{
	public class ApplicationContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationContext() : base("name=IdentityDb") { }

		public DbSet<ClientProfile> ClientProfiles { get; set; }
		public DbSet<ClientMessage> ClientMessages { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ApplicationUser>()
				.HasRequired(c => c.ClientProfile)
				.WithRequiredPrincipal(c => c.ApplicationUser);

			modelBuilder.Entity<ClientMessage>()
				.HasKey(k => k.Id);

			modelBuilder.Entity<ClientMessage>()
				.Property(k => k.Id)
				.HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

			base.OnModelCreating(modelBuilder);
		}
	}
}
