using Grid.Features.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Grid.Features.HRMS.Entities
{
    public class Shift: EntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Start Time")]
        public DateTime StartTime { get; set; }

        [DisplayName("End Time")]
        public DateTime EndTime { get; set; }

        [DisplayName("Needs Compensation")]
        public bool NeedsCompensation { get; set; }
    }
}
