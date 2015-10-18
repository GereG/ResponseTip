namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bitcoinaddressValidation2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResponseTipTasks", "BitcoinPublicAdress", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ResponseTipTasks", "BitcoinPublicAdress", c => c.String());
        }
    }
}
