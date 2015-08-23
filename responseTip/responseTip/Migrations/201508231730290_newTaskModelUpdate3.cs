namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTaskModelUpdate3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResponseTipTasks", "question", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ResponseTipTasks", "question", c => c.String());
        }
    }
}
