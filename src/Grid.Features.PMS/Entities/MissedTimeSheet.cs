using System;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.PMS.Entities
{
    public class MissedTimeSheet: EntityBase
    {
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime? FilledOn { get; set; }
    }
}
