using Grid.Features.Common;
using Grid.Features.LMS.Entities;
using Grid.Features.LMS.Entities.Enums;

namespace Grid.Features.LMS.ViewModels
{
    public class UpdateLeaveEntitlementViewModel : ViewModelBase
    {
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public int LeaveTimePeriodId { get; set; }
        public string LeaveTimePeriod { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public LeaveOperation Operation { get; set; }
        public float Count { get; set; }
        public float Allocation { get; set; }
        public string Comments { get; set; }

        public UpdateLeaveEntitlementViewModel()
        {

        }
        public UpdateLeaveEntitlementViewModel(LeaveEntitlement entitlement)
        {
            Id = entitlement.Id;
            LeaveTimePeriodId = entitlement.LeaveTimePeriodId;
            EmployeeId = entitlement.EmployeeId.Value;
            LeaveTypeId = entitlement.LeaveTypeId;
            if (entitlement.Employee.User.Person != null)
            {
                Employee = entitlement.Employee.User.Person.Name;
            }
            if (entitlement.LeaveType != null)
            {
                LeaveType = entitlement.LeaveType.Title;
            }
            if (entitlement.LeaveTimePeriod != null)
            {
                LeaveTimePeriod = entitlement.LeaveTimePeriod.Title;
            }


            Allocation = entitlement.Allocation;
            Comments = entitlement.Comments;
        }
    }
}