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

		public DbSet<ClientPendingFriend> ClientPendingFriends { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ApplicationUser>()
				.HasRequired(r => r.ClientProfile)
				.WithRequiredPrincipal(p => p.ApplicationUser);

			modelBuilder.Entity<ClientMessage>()
				.HasKey(k => k.Id);

			modelBuilder.Entity<ClientMessage>()
				.Property(p => p.Id)
				.HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

			modelBuilder.Entity<ClientProfile>()
				.HasMany(m => m.ClientPendingFriends)
				.WithRequired(w => w.ClientProfile)
				.HasForeignKey(k => k.Id_Receiver)
				.WillCascadeOnDelete(false);

			base.OnModelCreating(modelBuilder);
		}
	}
}
