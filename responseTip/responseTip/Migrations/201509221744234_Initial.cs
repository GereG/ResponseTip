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
                        answer = c.String(),
                        BitcoinPublicAdress = c.String(),
                        BitcoinPrice = c.Single(nullable: false),
                        isQuestionPublic = c.Boolean(nullable: false),
                        taskStatus = c.Int(nullable: false),
                        socialSiteUser_SocialSiteUsersID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseTipTaskID)
                .ForeignKey("dbo.SocialSiteUsers", t => t.socialSiteUser_SocialSiteUsersID, cascadeDelete: true)
                .Index(t => t.socialSiteUser_SocialSiteUsersID);
            
            CreateTable(
                "dbo.SocialSiteUsers",
                c => new
                    {
                        SocialSiteUsersID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.SocialSiteUsersID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResponseTipTasks", "socialSiteUser_SocialSiteUsersID", "dbo.SocialSiteUsers");
            DropIndex("dbo.ResponseTipTasks", new[] { "socialSiteUser_SocialSiteUsersID" });
            DropTable("dbo.SocialSiteUsers");
            DropTable("dbo.ResponseTipTasks");
        }
    }
}
