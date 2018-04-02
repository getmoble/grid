
using Grid.Features.HRMS.Entities;
using Grid.Features.PMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Grid.Data
{
    public class DataSeeder : CreateDatabaseIfNotExists<GridDataContext>
    {

        protected override void Seed(GridDataContext context)
        {
            var department = new Department
            {
                Title = "Development",
                Description = "",
                MailAlias = "",
                ParentId = null

            };
            var createDepartment = context.Departments.Add(department);
            context.SaveChanges();
            var department1 = new Department
            {
                Title = "Sales",
                Description = "",
                MailAlias = "",
                ParentId = null

            };
            var createDepartment1 = context.Departments.Add(department1);
            context.SaveChanges();
            var department2 = new Department
            {
                Title = "Testing",
                Description = "",
                MailAlias = "",
                ParentId = null

            };
            var createDepartment2 = context.Departments.Add(department2);
            context.SaveChanges();

            var memberRole = new ProjectMemberRole
            {
                Title = "Developer",
                Description = "",
                Role = Features.PMS.Entities.Enums.MemberRole.Developer,
                DepartmentId = department.Id

            };
            var createMemberRole = context.ProjectMemberRoles.Add(memberRole);
            context.SaveChanges();
            var memberRole1 = new ProjectMemberRole
            {
                Title = "Tester",
                Description = "",
                Role = Features.PMS.Entities.Enums.MemberRole.Tester,
                DepartmentId = department2.Id

            };
            var createMemberRole1 = context.ProjectMemberRoles.Add(memberRole1);
            context.SaveChanges();
            var memberRole2 = new ProjectMemberRole
            {
                Title = "Sales",
                Description = "",
                Role = Features.PMS.Entities.Enums.MemberRole.Sales,
                DepartmentId = department1.Id

            };
            var createMemberRole2 = context.ProjectMemberRoles.Add(memberRole2);
            context.SaveChanges();
            var memberRole4 = new ProjectMemberRole
            {
                Title = "Project Manager",
                Description = "",
                Role = Features.PMS.Entities.Enums.MemberRole.ProjectManager,
                DepartmentId = department.Id

            };
            var createMemberRole4 = context.ProjectMemberRoles.Add(memberRole4);
            context.SaveChanges();
        }
    }
}
