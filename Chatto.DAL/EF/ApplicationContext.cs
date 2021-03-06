using Chatto.DAL.Entities;
using Chatto.DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace Chatto.DAL.EF
{
	public class ApplicationContext : IdentityDbContext<ApplicationUser, CustomRole, Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>
	{
		public ApplicationContext() : base("name=IdentityDb") { }

		public DbSet<ClientProfile> ClientProfiles { get; set; }
		public DbSet<ClientMessage> ClientMessages { get; set; }

		public DbSet<ClientFriend> ClientFriends { get; set; }

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
				.HasMany(m => m.ClientFriends)
				.WithRequired(r => r.Friend_Profile1)
				.HasForeignKey(fk => fk.Friend_Id1)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ClientPendingFriend>()
				.HasKey(pf => new { pf.Id_Receiver, pf.Id_Sender });

			modelBuilder.Entity<ClientPendingFriend>()
				.HasRequired(r => r.SenderClientProfile)
				.WithMany(m => m.ClientSenderPendingFriends)
				.HasForeignKey(fk => fk.Id_Sender)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ClientPendingFriend>()
				.HasRequired(r => r.ReceiverClientProfile)
				.WithMany(m => m.ClientReceiverPendingFriends)
				.HasForeignKey(fk => fk.Id_Receiver)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ClientPendingFriend>()
				.Property(p => p.Id)
				.HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

			base.OnModelCreating(modelBuilder);
		}
	}
}
