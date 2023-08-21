namespace BankamatikUygulamasi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customerTranDateAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerTransactions", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerTransactions", "DateTime");
        }
    }
}
