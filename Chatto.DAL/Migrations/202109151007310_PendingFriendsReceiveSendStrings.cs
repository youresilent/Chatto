namespace Chatto.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PendingFriendsReceiveSendStrings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientProfiles", "PendingFriendsSent", c => c.String());
            AddColumn("dbo.ClientProfiles", "PendingFriendsReceived", c => c.String());
            DropColumn("dbo.ClientProfiles", "PendingFriends");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientProfiles", "PendingFriends", c => c.String());
            DropColumn("dbo.ClientProfiles", "PendingFriendsReceived");
            DropColumn("dbo.ClientProfiles", "PendingFriendsSent");
        }
    }
}
