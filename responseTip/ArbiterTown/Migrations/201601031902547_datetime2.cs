namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datetime2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResponseTipTasks", "timeCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ResponseTipTasks", "timeQuestionAsked", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.TextAnswerValidationTasks", "timeAssigned", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime());
            AlterColumn("dbo.TextAnswerValidationTasks", "timeAssigned", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ResponseTipTasks", "timeQuestionAsked", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ResponseTipTasks", "timeCreated", c => c.DateTime(nullable: false));
        }
    }
}
