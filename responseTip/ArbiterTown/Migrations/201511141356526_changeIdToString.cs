namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeIdToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TextAnswerValidationTasks", "ArbiterId", c => c.String(nullable: false));
            AlterColumn("dbo.TextAnswerValidationTasks", "TaskId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TextAnswerValidationTasks", "TaskId", c => c.Int(nullable: false));
            AlterColumn("dbo.TextAnswerValidationTasks", "ArbiterId", c => c.Int(nullable: false));
        }
    }
}
