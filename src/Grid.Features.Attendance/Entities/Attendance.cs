using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.Attendance.Entities
{
    public class Attendance : EntityBase
    {
        [DisplayName("Date")]
        public DateTime LogDate { get; set; }

        [DisplayName("In Time")]
        public TimeSpan InTime { get; set; }

        [DisplayName("Out Time")]
        public TimeSpan OutTime { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual User Employee { get; set; }
    }
}