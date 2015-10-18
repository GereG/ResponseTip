namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class returnBitcoinAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResponseTipTasks", "BitcoinReturnPublicAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResponseTipTasks", "BitcoinReturnPublicAddress");
        }
    }
}
