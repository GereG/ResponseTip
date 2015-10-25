namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twitterIDadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "questionTweetId", c => c.Long());
            AddColumn("dbo.ResponseTipTasks", "answerTweetId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResponseTipTasks", "answerTweetId");
            DropColumn("dbo.ResponseTipTasks", "questionTweetId");
        }
    }
}
