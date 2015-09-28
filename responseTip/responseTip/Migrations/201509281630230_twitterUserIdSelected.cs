namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twitterUserIdSelected : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "twitterUserIdSelected", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResponseTipTasks", "twitterUserIdSelected");
        }
    }
}
