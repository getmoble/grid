namespace Grid.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMemberRoleInProjectMemberRoleTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectMemberRoles", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectMemberRoles", "Role");
        }
    }
}
