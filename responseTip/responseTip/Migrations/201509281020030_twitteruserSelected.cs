namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twitteruserSelected : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "twitterUserNameWritten", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.ResponseTipTasks", "twitterUserNameSelected", c => c.String());
            DropColumn("dbo.ResponseTipTasks", "twitterUserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ResponseTipTasks", "twitterUserName", c => c.String(nullable: false, maxLength: 30));
            DropColumn("dbo.ResponseTipTasks", "twitterUserNameSelected");
            DropColumn("dbo.ResponseTipTasks", "twitterUserNameWritten");
        }
    }
}
