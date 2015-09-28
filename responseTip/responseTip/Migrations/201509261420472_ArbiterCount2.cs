namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArbiterCount2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResponseTipTasks", "twitterUserName", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ResponseTipTasks", "twitterUserName", c => c.String(nullable: false));
        }
    }
}
