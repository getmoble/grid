using System.Collections.Generic;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Areas.PMS.Models
{
    public class TimeSheetModel
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Date { get; set; }
        public TimeSheetState State { get; set; }
        public string Comments { get; set; }

        public List<TimeSheetLineItemModel> Rows { get; set; }
    }
}