namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class responseTip_answerValidation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "answerValidation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResponseTipTasks", "answerValidation");
        }
    }
}
