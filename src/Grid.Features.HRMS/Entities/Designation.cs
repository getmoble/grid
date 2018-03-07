using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Features.HRMS.Entities
{
    public class Designation: EntityBase
    {
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public Band? Band { get; set; }

        [DisplayName("Mail Alias")]
        [UIHint("Email")]
        public string MailAlias { get; set; }
    }
}
