namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class name_change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TextAnswerValidationTasks", "expirationTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.TextAnswerValidationTasks", "timeBeforeExpired");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TextAnswerValidationTasks", "timeBeforeExpired", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.TextAnswerValidationTasks", "expirationTime");
        }
    }
}
