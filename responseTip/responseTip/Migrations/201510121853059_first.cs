namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
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
                        answer = c.String(),
                        twitterUserNameWritten = c.String(nullable: false, maxLength: 30),
                        twitterUserIdSelected = c.Int(nullable: false),
                        twitterUserNameSelected = c.String(),
                        ArbiterCount = c.Int(nullable: false),
                        BitcoinPublicAdress = c.String(),
                        BitcoinPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        isQuestionPublic = c.Boolean(nullable: false),
                        timeCreated = c.DateTime(nullable: false),
                        timeQuestionAsked = c.DateTime(nullable: false),
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
