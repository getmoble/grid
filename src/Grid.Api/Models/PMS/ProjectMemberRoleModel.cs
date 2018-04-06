using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Api.Models.PMS
{
    public class ProjectMemberRoleModel : ApiModelBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? DepartmentId { get; set; }
        public string Department { get; set; }
        public MemberRole Role { get; set; }
        public string RoleType { get; set; }



        public ProjectMemberRoleModel()
        {

        }
        public ProjectMemberRoleModel(ProjectMemberRole memberRole)
        {
            Id = memberRole.Id;
            if (memberRole.Department != null)
            {
                Department = memberRole.Department.Title;
            }
            Title = memberRole.Title;
            DepartmentId = memberRole.DepartmentId;
            CreatedOn = memberRole.CreatedOn;
            RoleType = GetEnumDescription(memberRole.Role);

        }
    }
}

