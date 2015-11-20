namespace ArbiterTown.Migrations
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
                        question = c.String(nullable: false, maxLength: 137),
                        answer = c.String(),
                        twitterUserNameWritten = c.String(nullable: false, maxLength: 30),
                        twitterUserIdSelected = c.Int(nullable: false),
                        twitterUserNameSelected = c.String(),
                        ArbiterCount = c.Int(nullable: false),
                        BitcoinPublicAdress = c.String(nullable: false),
                        BitcoinReturnPublicAddress = c.String(nullable: false),
                        BitcoinPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DollarPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        isQuestionPublic = c.Boolean(nullable: false),
                        questionTweetId = c.Long(),
                        answerTweetId = c.Long(),
                        timeCreated = c.DateTime(nullable: false),
                        timeQuestionAsked = c.DateTime(nullable: false),
                        taskStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseTipTaskID);
            
            CreateTable(
                "dbo.TextAnswerValidationTasks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        ResponseTipTaskID = c.Int(nullable: false),
                        timeAssigned = c.DateTime(nullable: false),
                        timeBeforeExpired = c.Time(nullable: false, precision: 7),
                        arbiterAnswer = c.Int(nullable: false),
                        taskStatus = c.Int(nullable: false),
                        taskPriceInDollars = c.Decimal(nullable: false, precision: 18, scale: 2),
                        taskPriceInBitcoin = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.ResponseTipTasks", t => t.ResponseTipTaskID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.ResponseTipTaskID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        bitcoinEarned = c.Decimal(nullable: false, precision: 18, scale: 2),
                        numOfPuzzlesSuccesfull = c.Int(nullable: false),
                        numOfPuzzlesAttemted = c.Int(nullable: false),
                        numOfPuzzlesSkipped = c.Int(nullable: false),
                        hourlyPuzzleLimit = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TextAnswerValidationTasks", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TextAnswerValidationTasks", "ResponseTipTaskID", "dbo.ResponseTipTasks");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TextAnswerValidationTasks", new[] { "ResponseTipTaskID" });
            DropIndex("dbo.TextAnswerValidationTasks", new[] { "ApplicationUserId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TextAnswerValidationTasks");
            DropTable("dbo.ResponseTipTasks");
        }
    }
}
