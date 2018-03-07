using System.ComponentModel;

namespace Grid.Features.LMS.Entities.Enums
{
    public enum LeaveDuration
    {
        [Description("Multiple Days")]
        MultipleDays,
        [Description("One Full Day")]
        OneFullDay,
        [Description("First Half")]
        FirstHalf,
        [Description("Second Half")]
        SecondHalf
    }
}