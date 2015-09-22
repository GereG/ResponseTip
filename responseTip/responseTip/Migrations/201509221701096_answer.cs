namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class answer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "answer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResponseTipTasks", "answer");
        }
    }
}
