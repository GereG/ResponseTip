namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResponseTipTasks", "question", c => c.String(nullable: false, maxLength: 137));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ResponseTipTasks", "question", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
