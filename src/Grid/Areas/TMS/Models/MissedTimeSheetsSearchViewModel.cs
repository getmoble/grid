using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.PMS.Entities;


namespace Grid.Areas.TMS.Models
{
    public class MissedTimeSheetsSearchViewModel: ViewModelBase
    {
        public bool IsCalendarMode { get; set; }

        public bool IsPostBack { get; set; }
        public bool TeamMode { get; set; }

        public int? SubmittedUserById { get; set; }

        [DisplayName("Start Date")]
        [UIHint("Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [UIHint("Date")]
        public DateTime? EndDate { get; set; }
        public List<MissedTimeSheet> MissedTimeSheets { get; set; }

        public bool HasTeam { get; set; }

        public MissedTimeSheetsSearchViewModel()
        {
            MissedTimeSheets = new List<MissedTimeSheet>();
        }
    }
}