using Grid.Features.LMS.Entities;
using Grid.Features.LMS.Entities.Enums;

namespace Grid.Api.Models.LMS
{
    public class LeaveEntitlementModel : ApiModelBase
    {
        public string AllocatedBy { get; set; }
        public string Employee { get; set; }
        public string OperationType { get; set; }
        public int? EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public float PreviousBalance { get; set; }
        public float NewBalance { get; set; }
        public float LeaveCount { get; set; }
        public string LeaveType { get; set; }
        public float Allocation { get; set; }
        public int LeaveTimePeriodId { get; set; }
        public string LeaveTimePeriod { get; set; }
        public string Comments { get; set; }

        public LeaveOperation Operation { get; set; }

        public LeaveEntitlementModel(LeaveEntitlement leaveEntitlement)
        {
            Id = leaveEntitlement.Id;

            if (leaveEntitlement.Employee.User.Person != null)
            {
                Employee = leaveEntitlement.Employee.User.Person.Name;
            }
            if (leaveEntitlement.LeaveType != null)
            {
                LeaveType = leaveEntitlement.LeaveType.Title;
            }
            if (leaveEntitlement.LeaveTimePeriod != null)
            {
                LeaveTimePeriod = leaveEntitlement.LeaveTimePeriod.Title;
            }
            if (leaveEntitlement.CreatedByUser.Person != null)
            {
                AllocatedBy = leaveEntitlement.CreatedByUser.Person.Name;
            }


            Allocation = leaveEntitlement.Allocation;
            Comments = leaveEntitlement.Comments;
            CreatedOn = leaveEntitlement.CreatedOn;





        }
        public LeaveEntitlementModel(LeaveEntitlementUpdate leaveEntitlement)
        {
            Id = leaveEntitlement.Id;

            if (leaveEntitlement.Employee.User.Person != null)
            {
                Employee = leaveEntitlement.Employee.User.Person.Name;
            }
            if (leaveEntitlement.LeaveType != null)
            {
                LeaveType = leaveEntitlement.LeaveType.Title;
            }
            if (leaveEntitlement.LeaveTimePeriod != null)
            {
                LeaveTimePeriod = leaveEntitlement.LeaveTimePeriod.Title;
            }
            if (leaveEntitlement.CreatedByUser.Person != null)
            {
                AllocatedBy = leaveEntitlement.CreatedByUser.Person.Name;
            }
            Allocation = leaveEntitlement.NewBalance;
            CreatedOn = leaveEntitlement.CreatedOn;
            OperationType = GetEnumDescription(leaveEntitlement.Operation);
            PreviousBalance = leaveEntitlement.PreviousBalance;
            NewBalance = leaveEntitlement.NewBalance;
            LeaveCount = leaveEntitlement.LeaveCount;
        }
    }

}