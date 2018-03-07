using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;
using Grid.Features.LMS.Entities.Enums;

namespace Grid.Features.LMS.Entities
{
    public class LeaveEntitlementUpdate: UserCreatedEntityBase
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

        public LeaveOperation Operation { get; set; }

        public int? LeaveId { get; set; }
        [ForeignKey("LeaveId")]
        public virtual Leave Leave { get; set; }

        [DisplayName("Leave Count")]
        public float LeaveCount { get; set; }

        [DisplayName("Previous Balance")]
        public float PreviousBalance { get; set; }

        [DisplayName("New Balance")]
        public float NewBalance { get; set; }

        [DisplayName("Comments")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
    }
}