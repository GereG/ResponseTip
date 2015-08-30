namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTaskModelUpdate7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ResponseTipTasks", "socialSiteUser_SocialSiteUsersID", "dbo.SocialSiteUsers");
            DropIndex("dbo.ResponseTipTasks", new[] { "socialSiteUser_SocialSiteUsersID" });
            DropColumn("dbo.ResponseTipTasks", "socialSiteUser_SocialSiteUsersID");
            DropTable("dbo.SocialSiteUsers");
        }
        
        public override void Down()
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
            CreateIndex("dbo.ResponseTipTasks", "socialSiteUser_SocialSiteUsersID");
            AddForeignKey("dbo.ResponseTipTasks", "socialSiteUser_SocialSiteUsersID", "dbo.SocialSiteUsers", "SocialSiteUsersID", cascadeDelete: true);
        }
    }
}
