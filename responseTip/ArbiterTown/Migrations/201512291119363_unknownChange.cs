namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unknownChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResponseTipTasks", "BitcoinPrice", c => c.Decimal(nullable: false, precision: 11, scale: 8));
            AlterColumn("dbo.TextAnswerValidationTasks", "taskPriceInBitcoin", c => c.Decimal(nullable: false, precision: 11, scale: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TextAnswerValidationTasks", "taskPriceInBitcoin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ResponseTipTasks", "BitcoinPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
