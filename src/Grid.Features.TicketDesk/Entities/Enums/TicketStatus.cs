using System.ComponentModel;

namespace Grid.Features.TicketDesk.Entities.Enums
{
    public enum TicketStatus
    {
        [Description("Open")]
        Open,
        [Description("Assigned")]
        Assigned,
        [Description("Closed")]
        Closed,
        [Description("Cancelled")]
        Cancelled
    }
}