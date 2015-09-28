namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArbiterCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "twitterUserName", c => c.String(nullable: false));
            AddColumn("dbo.ResponseTipTasks", "ArbiterCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResponseTipTasks", "ArbiterCount");
            DropColumn("dbo.ResponseTipTasks", "twitterUserName");
        }
    }
}
