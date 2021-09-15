namespace Chatto.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PendingFriendsString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientProfiles", "PendingFriends", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientProfiles", "PendingFriends");
        }
    }
}
