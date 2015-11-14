namespace ArbiterTown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newProperties : DbMigration
    {
        public override void Up()
        {
//            AddColumn("dbo", "ArbiterId", c => c.Int());
        }
        
        public override void Down()
        {
//            DropColumn("dbo.ResponseTipTasks", "BitcoinReturnPublicAddress");
        }
    }
}
