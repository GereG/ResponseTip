namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTaskModelUpdate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "taskStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResponseTipTasks", "taskStatus");
        }
    }
}
