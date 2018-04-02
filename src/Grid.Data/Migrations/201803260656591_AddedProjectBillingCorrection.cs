namespace Grid.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProjectBillingCorrection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectBillingCorrections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Comments = c.String(),
                        BillingHours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedByUserId = c.Int(nullable: false),
                        UpdatedOn = c.DateTime(),
                        UpdatedByUserId = c.Int(),
                        Code = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.ProjectId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            AddColumn("dbo.ProjectBillings", "BillingHours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectBillingCorrections", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ProjectBillingCorrections", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectBillingCorrections", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.ProjectBillingCorrections", new[] { "UpdatedByUserId" });
            DropIndex("dbo.ProjectBillingCorrections", new[] { "CreatedByUserId" });
            DropIndex("dbo.ProjectBillingCorrections", new[] { "ProjectId" });
            DropColumn("dbo.ProjectBillings", "BillingHours");
            DropTable("dbo.ProjectBillingCorrections");
        }
    }
}
