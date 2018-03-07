using Grid.Features.Common;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Models
{
    public class TimeSheetCalenderEvent: EventData
    {
        public TimeSheetState state { get; set; }   
    }
}