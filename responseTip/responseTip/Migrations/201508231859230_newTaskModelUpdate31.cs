namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTaskModelUpdate31 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SocialSiteUsers",
                c => new
                    {
                        SocialSiteUsersID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.SocialSiteUsersID);
            
            AddColumn("dbo.ResponseTipTasks", "socialSiteUser_SocialSiteUsersID", c => c.Int(nullable: false));
            AlterColumn("dbo.ResponseTipTasks", "question", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.ResponseTipTasks", "BitcoinPublicAdress", c => c.String(nullable: false));
            CreateIndex("dbo.ResponseTipTasks", "socialSiteUser_SocialSiteUsersID");
            AddForeignKey("dbo.ResponseTipTasks", "socialSiteUser_SocialSiteUsersID", "dbo.SocialSiteUsers", "SocialSiteUsersID", cascadeDelete: true);
            DropColumn("dbo.ResponseTipTasks", "socialSiteUser_UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ResponseTipTasks", "socialSiteUser_UserName", c => c.String());
            DropForeignKey("dbo.ResponseTipTasks", "socialSiteUser_SocialSiteUsersID", "dbo.SocialSiteUsers");
            DropIndex("dbo.ResponseTipTasks", new[] { "socialSiteUser_SocialSiteUsersID" });
            AlterColumn("dbo.ResponseTipTasks", "BitcoinPublicAdress", c => c.String());
            AlterColumn("dbo.ResponseTipTasks", "question", c => c.String(nullable: false));
            DropColumn("dbo.ResponseTipTasks", "socialSiteUser_SocialSiteUsersID");
            DropTable("dbo.SocialSiteUsers");
        }
    }
}
