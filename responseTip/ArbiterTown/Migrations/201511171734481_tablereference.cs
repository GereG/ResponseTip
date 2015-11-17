namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablereference : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TextAnswerValidationTasks", "ResponseTipTaskID");
            AddForeignKey("dbo.TextAnswerValidationTasks", "ResponseTipTaskID", "dbo.ResponseTipTasks", "ResponseTipTaskID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TextAnswerValidationTasks", "ResponseTipTaskID", "dbo.ResponseTipTasks");
            DropIndex("dbo.TextAnswerValidationTasks", new[] { "ResponseTipTaskID" });
        }
    }
}
