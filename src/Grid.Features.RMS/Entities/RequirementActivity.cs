using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;

namespace Grid.Features.RMS.Entities
{
    public class RequirementActivity : UserCreatedEntityBase
    {
        public int RequirementId { get; set; }
        [ForeignKey("RequirementId")]
        public virtual Requirement Requirement { get; set; }

        public string Title { get; set; }

        [DisplayName("New Activity")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}