namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unknown_changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PuzzleLimit", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "numOfPuzzlesSuccesfull");
            DropColumn("dbo.AspNetUsers", "numOfPuzzlesAttemted");
            DropColumn("dbo.AspNetUsers", "percentageOfPuzzlesSuccesfull");
            DropColumn("dbo.AspNetUsers", "numOfPuzzlesSkipped");
            DropColumn("dbo.AspNetUsers", "hourlyPuzzleLimit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "hourlyPuzzleLimit", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "numOfPuzzlesSkipped", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "percentageOfPuzzlesSuccesfull", c => c.Single(nullable: false));
            AddColumn("dbo.AspNetUsers", "numOfPuzzlesAttemted", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "numOfPuzzlesSuccesfull", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "PuzzleLimit");
        }
    }
}
