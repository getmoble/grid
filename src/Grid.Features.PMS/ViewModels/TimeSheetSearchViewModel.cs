using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.ViewModels
{
    public class TimeSheetSearchViewModel : ViewModelBase
    {
        public bool IsCalendarMode { get; set; }

        public bool IsPostBack { get; set; }
        public bool TeamMode { get; set; }

        public int? SubmittedUserById { get; set; }

        public TimeSheetState? State { get; set; }

        [DisplayName("Start Date")]
        [UIHint("Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [UIHint("Date")]
        public DateTime? EndDate { get; set; }
        public List<TimeSheetViewModel> TimeSheets { get; set; }
        public int? ProjectId { get; set; }
        public bool HasTeam { get; set; }

        public TimeSheetSearchViewModel()
        {
            TimeSheets = new List<TimeSheetViewModel>();
        }
    }
}