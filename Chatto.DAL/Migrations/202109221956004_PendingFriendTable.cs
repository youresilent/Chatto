namespace Chatto.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PendingFriendTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientPendingFriends",
                c => new
                    {
                        Id_Receiver = c.String(nullable: false, maxLength: 128),
                        Id_Sender = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => new { t.Id_Receiver, t.Id_Sender })
                .ForeignKey("dbo.ClientProfiles", t => t.Id_Receiver)
                .ForeignKey("dbo.ClientProfiles", t => t.Id_Sender)
                .Index(t => t.Id_Receiver)
                .Index(t => t.Id_Sender);
            
            DropColumn("dbo.ClientProfiles", "PendingFriendsSent");
            DropColumn("dbo.ClientProfiles", "PendingFriendsReceived");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientProfiles", "PendingFriendsReceived", c => c.String());
            AddColumn("dbo.ClientProfiles", "PendingFriendsSent", c => c.String());
            DropForeignKey("dbo.ClientPendingFriends", "Id_Sender", "dbo.ClientProfiles");
            DropForeignKey("dbo.ClientPendingFriends", "Id_Receiver", "dbo.ClientProfiles");
            DropIndex("dbo.ClientPendingFriends", new[] { "Id_Sender" });
            DropIndex("dbo.ClientPendingFriends", new[] { "Id_Receiver" });
            DropTable("dbo.ClientPendingFriends");
        }
    }
}
