namespace Chatto.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageIdToInt : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ClientMessages");
            AlterColumn("dbo.ClientMessages", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ClientMessages", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ClientMessages");
            AlterColumn("dbo.ClientMessages", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ClientMessages", "Id");
        }
    }
}
