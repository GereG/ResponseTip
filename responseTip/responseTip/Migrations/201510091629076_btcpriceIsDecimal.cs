namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class btcpriceIsDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResponseTipTasks", "BitcoinPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ResponseTipTasks", "BitcoinPrice", c => c.Single(nullable: false));
        }
    }
}
