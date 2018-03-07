using Grid.Api.Models.TMS;
using Grid.Features.LMS.Entities.Enums;
using Grid.Features.PMS.Entities.Enums;
using System;
using System.Collections.Generic;

namespace Grid.Api.Models.LMS
{
    public class LeaveUpdateModel : ApiModelBase
    {       
        public DateTime Start { get; set; }   
        public DateTime End { get; set; }       
        public int LeaveTypeId { get; set; }       
        public string LeaveType { get; set; }
        public LeaveDuration Duration { get; set; }
        public string Reason { get; set; }      
        public LeaveStatus Status { get; set; }
        public int? RequestedForUserId { get; set; }       
        public string RequestedForUser { get; set; }    
        public float Count { get; set; }

        public bool IsLOP { get; set; }
        public string CalculationLog { get; set; }
        public int? ApproverId { get; set; }
        public string Approver { get; set; }
        public string ApproverComments { get; set; }
        public DateTime? ActedOn { get; set; }

        
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public TimeSheetState State { get; set; }
        public string Comments { get; set; }
        public List<TimeSheetLineItemModel> Rows { get; set; }
    }
}
