namespace Chatto.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientPendingFriendTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientPendingFriends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_Receiver = c.String(nullable: false, maxLength: 128),
                        Id_Sender = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientProfiles", t => t.Id_Receiver)
                .Index(t => t.Id_Receiver);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientPendingFriends", "Id_Receiver", "dbo.ClientProfiles");
            DropIndex("dbo.ClientPendingFriends", new[] { "Id_Receiver" });
            DropTable("dbo.ClientPendingFriends");
        }
    }
}
