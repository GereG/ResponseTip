namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class arbiterProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "bitcoinEarned", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "numOfPuzzlesSuccesfull", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "numOfPuzzlesAttemted", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "numOfPuzzlesSkipped", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "hourlyPuzzleLimit", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "UserBitcoinAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserBitcoinAddress", c => c.String());
            DropColumn("dbo.AspNetUsers", "hourlyPuzzleLimit");
            DropColumn("dbo.AspNetUsers", "numOfPuzzlesSkipped");
            DropColumn("dbo.AspNetUsers", "numOfPuzzlesAttemted");
            DropColumn("dbo.AspNetUsers", "numOfPuzzlesSuccesfull");
            DropColumn("dbo.AspNetUsers", "bitcoinEarned");
        }
    }
}
