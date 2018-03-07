using System.Collections.Generic;
using Grid.Features.Common;
using Grid.Features.LMS.Entities;

namespace Grid.Features.LMS.ViewModels
{
    public class LeaveEntitlementSearchViewModel: ViewModelBase
    {
        public int? AllocatedUserId { get; set; }
        public int? LeaveTypeId { get; set; }
        public List<LeaveEntitlement> LeaveEntitlements { get; set; }

        public LeaveEntitlementSearchViewModel()
        {
            LeaveEntitlements = new List<LeaveEntitlement>();
        }
    }
}