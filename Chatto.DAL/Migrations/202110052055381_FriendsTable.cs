namespace Chatto.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FriendsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientFriends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Friend_Id1 = c.String(nullable: false, maxLength: 128),
                        Friend_Id2 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientProfiles", t => t.Friend_Id1)
                .Index(t => t.Friend_Id1);
            
            DropColumn("dbo.ClientProfiles", "Friends");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientProfiles", "Friends", c => c.String());
            DropForeignKey("dbo.ClientFriends", "Friend_Id1", "dbo.ClientProfiles");
            DropIndex("dbo.ClientFriends", new[] { "Friend_Id1" });
            DropTable("dbo.ClientFriends");
        }
    }
}
