using Grid.Features.LMS.Entities;

namespace Grid.Api.Models.LMS
{
    public class LeaveEntitlementUpdateModel : ApiModelBase
    {
        public string LeaveTimePeriod { get; set; }
        public string LeaveType { get; set; }     
        public string OperationType { get; set; }
        public string AllocatedBy { get; set; }
        public float LeaveCount { get; set; }
        public float PreviousBalance { get; set; }
        public float NewBalance { get; set; }
        public string Comments { get; set; }
        public LeaveEntitlementUpdateModel()
        {

        }
        public LeaveEntitlementUpdateModel(LeaveEntitlementUpdate entitlementUpdate)
        {
            Id = entitlementUpdate.Id;
            if (entitlementUpdate.LeaveType != null)
            {
                LeaveType = entitlementUpdate.LeaveType.Title;
            }
            if (entitlementUpdate.LeaveTimePeriod != null)
            {
                LeaveTimePeriod = entitlementUpdate.LeaveTimePeriod.Title;
            }
            if (entitlementUpdate.CreatedByUser.Person != null)
            {
                AllocatedBy = entitlementUpdate.CreatedByUser.Person.Name;
            }

            OperationType = GetEnumDescription(entitlementUpdate.Operation);
            LeaveCount = entitlementUpdate.LeaveCount;
            PreviousBalance = entitlementUpdate.PreviousBalance;
            NewBalance = entitlementUpdate.NewBalance;
            Comments = entitlementUpdate.Comments;
        }
    }
}
