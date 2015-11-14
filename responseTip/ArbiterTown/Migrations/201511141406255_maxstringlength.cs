namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class maxstringlength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TextAnswerValidationTasks", "ArbiterId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TextAnswerValidationTasks", "TaskId", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TextAnswerValidationTasks", "TaskId", c => c.String(nullable: false));
            AlterColumn("dbo.TextAnswerValidationTasks", "ArbiterId", c => c.String(nullable: false));
        }
    }
}
