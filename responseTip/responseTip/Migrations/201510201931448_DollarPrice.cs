namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DollarPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "DollarPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResponseTipTasks", "DollarPrice");
        }
    }
}
