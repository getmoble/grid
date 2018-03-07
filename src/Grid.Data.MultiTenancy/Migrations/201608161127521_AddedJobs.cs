namespace Grid.Data.MultiTenancy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedJobs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TenantScheduledJobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        Name = c.String(),
                        JobInfo = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tenants", t => t.TenantId)
                .Index(t => t.TenantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenantScheduledJobs", "TenantId", "dbo.Tenants");
            DropIndex("dbo.TenantScheduledJobs", new[] { "TenantId" });
            DropTable("dbo.TenantScheduledJobs");
        }
    }
}
