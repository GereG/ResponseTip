namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bitcoinaddressvalidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResponseTipTasks", "BitcoinReturnPublicAddress", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ResponseTipTasks", "BitcoinReturnPublicAddress", c => c.String());
        }
    }
}
