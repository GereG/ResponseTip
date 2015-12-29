namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUser_percentageOfPuzzlesSuccesfull : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "percentageOfPuzzlesSuccesfull", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "percentageOfPuzzlesSuccesfull");
        }
    }
}
