using System;
using System.Collections.Generic;
using System.ComponentModel;
using Grid.Features.Common;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using Grid.Infrastructure;
using Grid.Infrastructure.Extensions;

namespace Grid.Features.PMS.ViewModels
{
    public class TimeSheetViewModel: ViewModelBase
    {
        public bool IsApprover { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }

        [DisplayName("Total Hours")]
        public double TotalHours { get; set; }
        public string Comments { get; set; }
        public TimeSheetState State { get; set; }

        public string StateStyle { get; set; }

        public int? ApprovedByEmployeeId { get; set; }
        public Features.HRMS.Entities.Employee ApprovedByEmployee { get; set; }
        public string ApproverComments { get; set; }

        // Show Delete only for OwnSheet is true
        public bool OwnSheet { get; set; }

        public Features.HRMS.Entities.User CreatedByUser { get; set; }
        public int CreatedByUserId { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public List<TimeSheetLineItem> LineItems { get; set; }
        public List<TimeSheetActivity> TimeSheetActivities { get; set; }

        public TimeSheetViewModel()
        {
            LineItems = new List<TimeSheetLineItem>();
            TimeSheetActivities = new List<TimeSheetActivity>();
        }

        public TimeSheetViewModel(TimeSheet timeSheet, Principal user) : this()
        {
            Id = timeSheet.Id;
            Title = timeSheet.Title;
            Date = timeSheet.Date;
            TotalHours = timeSheet.TotalHours;
            Comments = timeSheet.Comments.Truncate();
            State = timeSheet.State;
            StateStyle = timeSheet.StateStyle;
            ApprovedByEmployeeId = timeSheet.ApprovedByEmployeeId;
            ApprovedByEmployee = timeSheet.ApprovedByEmployee;
            ApproverComments = timeSheet.ApproverComments;
            CreatedByUser = timeSheet.CreatedByUser;
            CreatedByUserId = timeSheet.CreatedByUserId;

            CreatedOn = timeSheet.CreatedOn;
            UpdatedOn = timeSheet.UpdatedOn;

            // Own Sheet
            OwnSheet = timeSheet.CreatedByUserId == user.Id;
        }
    }
}
