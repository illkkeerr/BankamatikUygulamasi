namespace BankamatikUygulamasi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerTranAddExplanation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerTransactions", "Explanation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerTransactions", "Explanation");
        }
    }
}
