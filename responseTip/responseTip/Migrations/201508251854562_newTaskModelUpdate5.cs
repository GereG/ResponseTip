namespace responseTip.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTaskModelUpdate5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResponseTipTasks", "BitcoinPublicAdress", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ResponseTipTasks", "BitcoinPublicAdress", c => c.String(nullable: false));
        }
    }
}
