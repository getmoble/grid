using Grid.Features.LMS.Entities;
using System;

namespace Grid.Api.Models.LMS
{
    public class HolidayModel: ApiModelBase
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }

        public HolidayModel(Holiday holiday)
        {
            Id = holiday.Id;
            Title = holiday.Title;
            Type = GetEnumDescription(holiday.Type);
            Date = holiday.Date;
            CreatedOn = holiday.CreatedOn;
        }
    }
}
