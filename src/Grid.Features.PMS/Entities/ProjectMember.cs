using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.Entities
{
    public class ProjectMember : UserCreatedEntityBase
    {
        public int ProjectId { get; set; }
        [DisplayName("Project")]
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public int? EmployeeId { get; set; }        
        [ForeignKey("EmployeeId")]
        public Employee MemberEmployee { get; set; }
        public MemberStatus MemberStatus { get; set; }

        [DisplayName("Billing")]
        public Billing Billing { get; set; }
        public double Rate { get; set; }

        public int? ProjectMemberRoleId { get; set; }
        [ForeignKey("ProjectMemberRoleId")]
        public ProjectMemberRole ProjectMemberRole { get; set; }

    }
}
