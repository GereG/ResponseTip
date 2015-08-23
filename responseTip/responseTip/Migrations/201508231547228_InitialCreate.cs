namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResponseTipTasks",
                c => new
                    {
                        ResponseTipTaskID = c.Int(nullable: false, identity: true),
                        userID = c.Int(nullable: false),
                        question = c.String(),
                        BitcoinPublicAdress = c.String(),
                        BitcoinPrice = c.Single(nullable: false),
                        isQuestionPublic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseTipTaskID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ResponseTipTasks");
        }
    }
}
