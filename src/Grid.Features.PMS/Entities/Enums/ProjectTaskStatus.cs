using System.ComponentModel;

namespace Grid.Features.PMS.Entities.Enums
{
    public enum ProjectTaskStatus
    {
        [Description("Not Started")]
        Notstarted = 1,
        [Description("Started")]
        Started = 2,
        [Description("Completed")]
        Completed = 3,
        [Description("Cancelled")]
        Cancelled = 4
    }
}
