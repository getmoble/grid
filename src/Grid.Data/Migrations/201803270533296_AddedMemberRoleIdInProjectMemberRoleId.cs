namespace Grid.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMemberRoleIdInProjectMemberRoleId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectMembers", "ProjectMemberRoleId", c => c.Int());
            CreateIndex("dbo.ProjectMembers", "ProjectMemberRoleId");
            AddForeignKey("dbo.ProjectMembers", "ProjectMemberRoleId", "dbo.ProjectMemberRoles", "Id");
            DropColumn("dbo.ProjectMembers", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectMembers", "Role", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProjectMembers", "ProjectMemberRoleId", "dbo.ProjectMemberRoles");
            DropIndex("dbo.ProjectMembers", new[] { "ProjectMemberRoleId" });
            DropColumn("dbo.ProjectMembers", "ProjectMemberRoleId");
        }
    }
}
