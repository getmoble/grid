using Grid.Features.Common;

namespace Grid.Areas.LMS.Models
{
    public class LeaveListFilter: EventDataFilter
    {
        public int? UserId { get; set; }
    }
}