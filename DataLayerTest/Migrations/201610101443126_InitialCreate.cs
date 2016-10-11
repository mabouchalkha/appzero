namespace DataLayerTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        City = c.String(),
                        Country = c.String(),
                        Customer_CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Customer", t => t.Customer_CustomerId)
                .Index(t => t.Customer_CustomerId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        Product = c.String(),
                        Customer_CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Customer", t => t.Customer_CustomerId)
                .Index(t => t.Customer_CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "Customer_CustomerId", "dbo.Customer");
            DropForeignKey("dbo.Address", "Customer_CustomerId", "dbo.Customer");
            DropIndex("dbo.Order", new[] { "Customer_CustomerId" });
            DropIndex("dbo.Address", new[] { "Customer_CustomerId" });
            DropTable("dbo.Order");
            DropTable("dbo.Address");
            DropTable("dbo.Customer");
        }
    }
}
