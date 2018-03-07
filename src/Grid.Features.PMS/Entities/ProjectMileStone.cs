using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.Entities
{
    public class ProjectMileStone : UserCreatedEntityBase
    {
        public int ProjectId { get; set; }
        [DisplayName("Project")]
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Target Date")]
        [UIHint("Date")]
        public DateTime TargetDate { get; set; }

        [Required]
        public MileStoneStatus Status { get; set; }
    }
}
