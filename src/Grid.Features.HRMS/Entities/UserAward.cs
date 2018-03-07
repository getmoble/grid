using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;

namespace Grid.Features.HRMS.Entities
{
    public class UserAward: EntityBase
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int AwardId { get; set; }
        [ForeignKey("AwardId")]
        public virtual Award Award { get; set; }

        [MaxLength(250)]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
    }
}