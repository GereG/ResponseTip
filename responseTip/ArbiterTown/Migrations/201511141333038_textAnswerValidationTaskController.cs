namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class textAnswerValidationTaskController : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TextAnswerValidationTasks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ArbiterId = c.Int(nullable: false),
                        TaskId = c.Int(nullable: false),
                        timeAssigned = c.DateTime(nullable: false),
                        timeBeforeExpired = c.Time(nullable: false, precision: 7),
                        arbiterAnswer = c.Int(nullable: false),
                        taskStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TextAnswerValidationTasks");
        }
    }
}
