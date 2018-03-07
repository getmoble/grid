using Grid.Features.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grid.Features.HRMS.Entities
{
    public class Department : EntityBase
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
        [ForeignKey("ParentId")]
        public virtual Department Parent { get; set; }
    }
}
