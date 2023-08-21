namespace BankamatikUygulamasi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerLogs",
                c => new
                    {
                        CustomerLogsId = c.Int(nullable: false, identity: true),
                        LogTime = c.DateTime(nullable: false),
                        IsLoginOrLogout = c.Boolean(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerLogsId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Password = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.CustomerTransactions",
                c => new
                    {
                        TransactionsId = c.Int(nullable: false, identity: true),
                        AmountOfMoney = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionTypeId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionsId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.TransactionTypes", t => t.TransactionTypeId, cascadeDelete: true)
                .Index(t => t.TransactionTypeId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.TransactionTypes",
                c => new
                    {
                        TransactionTypeId = c.Int(nullable: false, identity: true),
                        TransactionName = c.String(),
                    })
                .PrimaryKey(t => t.TransactionTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerTransactions", "TransactionTypeId", "dbo.TransactionTypes");
            DropForeignKey("dbo.CustomerTransactions", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerLogs", "CustomerId", "dbo.Customers");
            DropIndex("dbo.CustomerTransactions", new[] { "CustomerId" });
            DropIndex("dbo.CustomerTransactions", new[] { "TransactionTypeId" });
            DropIndex("dbo.CustomerLogs", new[] { "CustomerId" });
            DropTable("dbo.TransactionTypes");
            DropTable("dbo.CustomerTransactions");
            DropTable("dbo.Customers");
            DropTable("dbo.CustomerLogs");
        }
    }
}
