using Grid.Features.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Grid.Features.HRMS.Models
{
    public class DepartmentModel:EntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Mail Alias")]
        [UIHint("Email")]
        public string MailAlias { get; set; }

        public int? ParentId { get; set; }
    }
}
