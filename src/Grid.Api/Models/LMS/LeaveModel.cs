using System;
using Grid.Features.LMS.Entities;
using System.Globalization;

namespace Grid.Api.Models.LMS
{
    public class LeaveModel: ApiModelBase
    {
        public string Employee { get; set; }
        public string LeaveType { get; set; }
        public string Duration { get; set; }
        public string Period { get; set; }
        public DateTime AppliedOn { get; set; }
        public string Status { get; set; }
        public string Approver { get; set; }     
        public string Reason { get; set; }    
        public DateTime? ActedOn { get; set; }
        public string VisibleData { get; set; }

        public LeaveModel(Leave leave)
        {
            Id = leave.Id;

            if (leave.RequestedForUser?.User.Person != null)
            {
                Employee = leave.RequestedForUser.User.Person.Name;
            }


            if (leave.LeaveType != null)
            {
                LeaveType = leave.LeaveType.Title;
            }

            Duration = GetEnumDescription(leave.Duration);
            Period = $"{leave.Start.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)} - {leave.End.ToString("dd/MM/yyyy")}";
            AppliedOn = leave.CreatedOn;
            Status = GetEnumDescription(leave.Status);
        
            if (leave.Approver?.User.Person != null)
            {
                Approver = leave.Approver.User.Person.Name;
            }
            
            ActedOn = leave.ActedOn;
            CreatedOn = leave.CreatedOn;
          
         
        }
    }
}



