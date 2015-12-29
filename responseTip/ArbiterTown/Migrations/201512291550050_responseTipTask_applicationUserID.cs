namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class responseTipTask_applicationUserID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "ApplicationUserId", c => c.String(maxLength: 128));
            DropColumn("dbo.ResponseTipTasks", "userName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ResponseTipTasks", "userName", c => c.String());
            DropColumn("dbo.ResponseTipTasks", "ApplicationUserId");
        }
    }
}
