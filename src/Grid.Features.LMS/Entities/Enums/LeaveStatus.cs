using System.ComponentModel;

namespace Grid.Features.LMS.Entities.Enums
{
    public enum LeaveStatus
    {
        [Description("Pending")]
        Pending,
        [Description("Approved")]
        Approved,
        [Description("Rejected")]
        Rejected
    }
}