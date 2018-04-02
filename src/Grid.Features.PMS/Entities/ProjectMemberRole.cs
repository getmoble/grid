using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grid.Features.PMS.Entities
{
    public class ProjectMemberRole : UserCreatedEntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        public MemberRole Role { get; set; }

    }
}
