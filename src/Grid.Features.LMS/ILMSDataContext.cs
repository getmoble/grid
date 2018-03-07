using System.Data.Entity;
using Grid.Features.Common;
using Grid.Features.LMS.Entities;

namespace Grid.Features.LMS
{
    public interface ILMSDataContext :  IDbContext
    {
        DbSet<Holiday> Holidays { get; set; }
        DbSet<LeaveType> LeaveTypes { get; set; }
        DbSet<LeaveTimePeriod> LeaveTimePeriods { get; set; }
        DbSet<LeaveEntitlement> LeaveEntitlements { get; set; }
        DbSet<LeaveEntitlementUpdate> LeaveEntitlementUpdates { get; set; }
        DbSet<Leave> Leaves { get; set; }
    }
}
