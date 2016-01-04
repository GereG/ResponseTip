namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationUser_public : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "numOfPuzzlesSuccesfull", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "numOfPuzzlesAttemted", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "percentageOfPuzzlesSuccesfull", c => c.Single(nullable: false));
            AddColumn("dbo.AspNetUsers", "numOfPuzzlesSkipped", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "numOfPuzzlesWaiting", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "numOfPuzzlesWaiting");
            DropColumn("dbo.AspNetUsers", "numOfPuzzlesSkipped");
            DropColumn("dbo.AspNetUsers", "percentageOfPuzzlesSuccesfull");
            DropColumn("dbo.AspNetUsers", "numOfPuzzlesAttemted");
            DropColumn("dbo.AspNetUsers", "numOfPuzzlesSuccesfull");
        }
    }
}
