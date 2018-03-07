using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.LMS.Entities
{
    public class LeaveEntitlement: UserCreatedEntityBase
    {

        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required]
        [DisplayName("Period")]
        public int LeaveTimePeriodId { get; set; }
        [ForeignKey("LeaveTimePeriodId")]
        public virtual LeaveTimePeriod LeaveTimePeriod { get; set; }

        [Required]
        [DisplayName("Leave Type")]
        public int LeaveTypeId { get; set; }
        [ForeignKey("LeaveTypeId")]
        public virtual LeaveType LeaveType { get; set; }

        [Required]
        [DisplayName("Allocated Days")]
        public float Allocation { get; set; }

        [DisplayName("Comments")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
    }
}