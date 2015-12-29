namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArbiterTownApplicationUserBitcoinEarnedPrecisionChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "userName", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "bitcoinEarned", c => c.Decimal(nullable: false, precision: 11, scale: 8));
            DropColumn("dbo.ResponseTipTasks", "ApplicationUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ResponseTipTasks", "ApplicationUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "bitcoinEarned", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.ResponseTipTasks", "userName");
        }
    }
}
