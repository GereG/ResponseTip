namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AspNetUsersId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TextAnswerValidationTasks", "AspNetUsersId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.TextAnswerValidationTasks", "ResponseTipTaskID", c => c.Int(nullable: false));
            AddColumn("dbo.TextAnswerValidationTasks", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.TextAnswerValidationTasks", "ApplicationUser_Id");
            AddForeignKey("dbo.TextAnswerValidationTasks", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.TextAnswerValidationTasks", "ArbiterId");
            DropColumn("dbo.TextAnswerValidationTasks", "TaskId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TextAnswerValidationTasks", "TaskId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.TextAnswerValidationTasks", "ArbiterId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.TextAnswerValidationTasks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TextAnswerValidationTasks", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.TextAnswerValidationTasks", "ApplicationUser_Id");
            DropColumn("dbo.TextAnswerValidationTasks", "ResponseTipTaskID");
            DropColumn("dbo.TextAnswerValidationTasks", "AspNetUsersId");
        }
    }
}
