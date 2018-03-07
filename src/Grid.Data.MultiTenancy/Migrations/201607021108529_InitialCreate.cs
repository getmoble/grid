namespace Grid.Data.MultiTenancy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        ContactPersonName = c.String(),
                        ContactPersonNumber = c.String(),
                        ConnectionString = c.String(),
                        DomainName = c.String(),
                        SubDomain = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tenants");
        }
    }
}
