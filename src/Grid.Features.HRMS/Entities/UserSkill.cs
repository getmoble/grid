using Grid.Features.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grid.Features.HRMS.Entities
{
    public class UserSkill: EntityBase
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int SkillId { get; set; }
        [ForeignKey("SkillId")]
        public virtual Skill Skill { get; set; }
    }
}
