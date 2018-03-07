using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.LMS.Entities;
using Grid.Features.LMS.Entities.Enums;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.LMS.ViewModels
{
    public class LeaveViewModel: ViewModelBase
    {
        [Required]
        [UIHint("Date")]
        [DisplayName("Start Date")]
        public DateTime Start { get; set; }

        [Required]
        [UIHint("Date")]
        [DisplayName("End Date")]
        public DateTime End { get; set; }

        [Required]
        [DisplayName("Leave Type")]
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }

        [DisplayName("Reason")]
        [DataType(DataType.MultilineText)]
        public string Reason { get; set; }

        [DisplayName("Status")]
        public LeaveStatus Status { get; set; }

        public string StatusStyle { get; set; }

        public int? ApproverId { get; set; }
        [ForeignKey("ApproverId")]
        public Employee Approver { get; set; }

        public string ApproverComments { get; set; }

        public int CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public Features.HRMS.Entities.User CreatedByUser { get; set; }

        public bool IsOwn { get; set; }
        public bool IsApprover { get; set; }

        public DateTime? ActedOn { get; set; }
        public LeaveDuration Duration { get; set; }
        public LeaveViewModel(Leave leave, Principal userPrincipal)
        {
            Id = leave.Id;
            Start = leave.Start;
            End = leave.End;
            LeaveTypeId = leave.LeaveTypeId;
            LeaveType = leave.LeaveType;
            Reason = leave.Reason;
            Status = leave.Status;
            StatusStyle = leave.StatusStyle;
            //if (leave.Approver?.User.Person != null)
            //{
            //    Approver = leave.Approver.User.Person.Name;
            //}
            ApproverId = leave.ApproverId;
            Approver = leave.Approver;
            ApproverComments = leave.ApproverComments;
            CreatedByUserId = leave.CreatedByUserId;
            CreatedByUser = leave.CreatedByUser;
            Duration = leave.Duration;
            if(userPrincipal != null)
            {
                IsOwn = leave.CreatedByUserId == userPrincipal.Id;
                IsApprover = ApproverId == userPrincipal.Id;
            }

            ActedOn = leave.ActedOn;
            CreatedOn = leave.CreatedOn;
        }
    }
}