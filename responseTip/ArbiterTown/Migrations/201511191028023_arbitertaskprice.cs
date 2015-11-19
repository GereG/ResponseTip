namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class arbitertaskprice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TextAnswerValidationTasks", "taskPriceInDollars", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TextAnswerValidationTasks", "taskPriceInBitcoin", c => c.Decimal(nullable: false, precision: 11, scale: 8));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TextAnswerValidationTasks", "taskPriceInBitcoin");
            DropColumn("dbo.TextAnswerValidationTasks", "taskPriceInDollars");
        }
    }
}
