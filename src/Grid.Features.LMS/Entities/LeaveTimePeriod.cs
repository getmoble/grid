using Grid.Features.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Grid.Features.LMS.Entities
{
    public class LeaveTimePeriod : EntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [Required]
        [UIHint("Date")]
        [DisplayName("Start Date")]
        public DateTime Start { get; set; }

        [Required]
        [UIHint("Date")]
        [DisplayName("End Date")]
        public DateTime End { get; set; }
        public string Description { get; set; }
    }
}