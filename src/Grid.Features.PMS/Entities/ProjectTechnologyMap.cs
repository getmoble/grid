using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.PMS.Entities
{
    public class ProjectTechnologyMap: EntityBase
    {
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        public int TechnologyId { get; set; }
        [ForeignKey("TechnologyId")]
        public virtual Technology Technology { get; set; }
    }
}