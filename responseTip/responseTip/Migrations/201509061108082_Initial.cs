namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResponseTipTasks",
                c => new
                    {
                        ResponseTipTaskID = c.Int(nullable: false, identity: true),
                        userName = c.String(),
                        question = c.String(nullable: false, maxLength: 30),
                        BitcoinPublicAdress = c.String(),
                        BitcoinPrice = c.Single(nullable: false),
                        isQuestionPublic = c.Boolean(nullable: false),
                        taskStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseTipTaskID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ResponseTipTasks");
        }
    }
}
