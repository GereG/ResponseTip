namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TextAnswerValidationTasks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TextAnswerValidationTasks", new[] { "ApplicationUser_Id" });
            RenameColumn(table: "dbo.TextAnswerValidationTasks", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            AlterColumn("dbo.TextAnswerValidationTasks", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.TextAnswerValidationTasks", "ApplicationUserId");
            AddForeignKey("dbo.TextAnswerValidationTasks", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.TextAnswerValidationTasks", "AspNetUsersId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TextAnswerValidationTasks", "AspNetUsersId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.TextAnswerValidationTasks", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.TextAnswerValidationTasks", new[] { "ApplicationUserId" });
            AlterColumn("dbo.TextAnswerValidationTasks", "ApplicationUserId", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.TextAnswerValidationTasks", name: "ApplicationUserId", newName: "ApplicationUser_Id");
            CreateIndex("dbo.TextAnswerValidationTasks", "ApplicationUser_Id");
            AddForeignKey("dbo.TextAnswerValidationTasks", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
