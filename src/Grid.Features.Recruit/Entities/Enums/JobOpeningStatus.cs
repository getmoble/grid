using System.ComponentModel;

namespace Grid.Features.Recruit.Entities.Enums
{
    public enum JobOpeningStatus
    {
        [Description("Open")]
        Open,
        [Description("On Hold")]
        OnHold,
        [Description("Closed")]
        Closed
    }
}