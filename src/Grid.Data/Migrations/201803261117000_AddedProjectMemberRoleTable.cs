namespace Grid.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProjectMemberRoleTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectMemberRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        DepartmentId = c.Int(),
                        CreatedByUserId = c.Int(nullable: false),
                        UpdatedOn = c.DateTime(),
                        UpdatedByUserId = c.Int(),
                        Code = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.DepartmentId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectMemberRoles", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ProjectMemberRoles", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.ProjectMemberRoles", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.ProjectMemberRoles", new[] { "UpdatedByUserId" });
            DropIndex("dbo.ProjectMemberRoles", new[] { "CreatedByUserId" });
            DropIndex("dbo.ProjectMemberRoles", new[] { "DepartmentId" });
            DropTable("dbo.ProjectMemberRoles");
        }
    }
}
