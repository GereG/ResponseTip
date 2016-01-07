namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class arbiter_numOfExpired : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "numOfPuzzlesExpired", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "numOfPuzzlesExpired");
        }
    }
}
