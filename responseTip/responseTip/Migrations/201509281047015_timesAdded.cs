namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timesAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "timeCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.ResponseTipTasks", "timeQuestionAsked", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResponseTipTasks", "timeQuestionAsked");
            DropColumn("dbo.ResponseTipTasks", "timeCreated");
        }
    }
}
