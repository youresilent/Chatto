namespace Chatto.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AspNetIdentityPKToGuid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClientFriends", "Friend_Id1", "dbo.ClientProfiles");
            DropForeignKey("dbo.ClientPendingFriends", "Id_Receiver", "dbo.ClientProfiles");
            DropForeignKey("dbo.ClientPendingFriends", "Id_Sender", "dbo.ClientProfiles");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ClientProfiles", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.ClientFriends", new[] { "Friend_Id1" });
            DropIndex("dbo.ClientProfiles", new[] { "Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.ClientPendingFriends", new[] { "Id_Receiver" });
            DropIndex("dbo.ClientPendingFriends", new[] { "Id_Sender" });
            DropPrimaryKey("dbo.ClientProfiles");
            DropPrimaryKey("dbo.AspNetUsers");
            DropPrimaryKey("dbo.AspNetUserLogins");
            DropPrimaryKey("dbo.AspNetUserRoles");
            DropPrimaryKey("dbo.ClientPendingFriends");
            DropPrimaryKey("dbo.AspNetRoles");
            AlterColumn("dbo.ClientFriends", "Friend_Id1", c => c.Guid(nullable: false));
            AlterColumn("dbo.ClientFriends", "Friend_Id2", c => c.Guid(nullable: false));
            AlterColumn("dbo.ClientProfiles", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUserRoles", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetUserRoles", "RoleId", c => c.Guid(nullable: false));
            AlterColumn("dbo.ClientPendingFriends", "Id_Receiver", c => c.Guid(nullable: false));
            AlterColumn("dbo.ClientPendingFriends", "Id_Sender", c => c.Guid(nullable: false));
            AlterColumn("dbo.AspNetRoles", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.ClientProfiles", "Id");
            AddPrimaryKey("dbo.AspNetUsers", "Id");
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
            AddPrimaryKey("dbo.AspNetUserRoles", new[] { "UserId", "RoleId" });
            AddPrimaryKey("dbo.ClientPendingFriends", new[] { "Id_Receiver", "Id_Sender" });
            AddPrimaryKey("dbo.AspNetRoles", "Id");
            CreateIndex("dbo.ClientFriends", "Friend_Id1");
            CreateIndex("dbo.ClientProfiles", "Id");
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            CreateIndex("dbo.AspNetUserRoles", "UserId");
            CreateIndex("dbo.AspNetUserRoles", "RoleId");
            CreateIndex("dbo.ClientPendingFriends", "Id_Receiver");
            CreateIndex("dbo.ClientPendingFriends", "Id_Sender");
            AddForeignKey("dbo.ClientFriends", "Friend_Id1", "dbo.ClientProfiles", "Id");
            AddForeignKey("dbo.ClientPendingFriends", "Id_Receiver", "dbo.ClientProfiles", "Id");
            AddForeignKey("dbo.ClientPendingFriends", "Id_Sender", "dbo.ClientProfiles", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ClientProfiles", "Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            DropColumn("dbo.AspNetRoles", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ClientProfiles", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ClientPendingFriends", "Id_Sender", "dbo.ClientProfiles");
            DropForeignKey("dbo.ClientPendingFriends", "Id_Receiver", "dbo.ClientProfiles");
            DropForeignKey("dbo.ClientFriends", "Friend_Id1", "dbo.ClientProfiles");
            DropIndex("dbo.ClientPendingFriends", new[] { "Id_Sender" });
            DropIndex("dbo.ClientPendingFriends", new[] { "Id_Receiver" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.ClientProfiles", new[] { "Id" });
            DropIndex("dbo.ClientFriends", new[] { "Friend_Id1" });
            DropPrimaryKey("dbo.AspNetRoles");
            DropPrimaryKey("dbo.ClientPendingFriends");
            DropPrimaryKey("dbo.AspNetUserRoles");
            DropPrimaryKey("dbo.AspNetUserLogins");
            DropPrimaryKey("dbo.AspNetUsers");
            DropPrimaryKey("dbo.ClientProfiles");
            AlterColumn("dbo.AspNetRoles", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ClientPendingFriends", "Id_Sender", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ClientPendingFriends", "Id_Receiver", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserRoles", "RoleId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserRoles", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ClientProfiles", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ClientFriends", "Friend_Id2", c => c.String());
            AlterColumn("dbo.ClientFriends", "Friend_Id1", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.AspNetRoles", "Id");
            AddPrimaryKey("dbo.ClientPendingFriends", new[] { "Id_Receiver", "Id_Sender" });
            AddPrimaryKey("dbo.AspNetUserRoles", new[] { "UserId", "RoleId" });
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
            AddPrimaryKey("dbo.AspNetUsers", "Id");
            AddPrimaryKey("dbo.ClientProfiles", "Id");
            CreateIndex("dbo.ClientPendingFriends", "Id_Sender");
            CreateIndex("dbo.ClientPendingFriends", "Id_Receiver");
            CreateIndex("dbo.AspNetUserRoles", "RoleId");
            CreateIndex("dbo.AspNetUserRoles", "UserId");
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.ClientProfiles", "Id");
            CreateIndex("dbo.ClientFriends", "Friend_Id1");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ClientProfiles", "Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ClientPendingFriends", "Id_Sender", "dbo.ClientProfiles", "Id");
            AddForeignKey("dbo.ClientPendingFriends", "Id_Receiver", "dbo.ClientProfiles", "Id");
            AddForeignKey("dbo.ClientFriends", "Friend_Id1", "dbo.ClientProfiles", "Id");
        }
    }
}
