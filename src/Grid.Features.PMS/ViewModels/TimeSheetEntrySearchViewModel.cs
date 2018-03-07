using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using PagedList;

namespace Grid.Features.PMS.ViewModels
{
    public class TimeSheetEntrySearchViewModel: PagedViewModelBase
    {
        public int[] ProjectId { get; set; }
        public int[] UserId { get; set; }

        public int? WorkType { get; set; }

        public string Work { get; set; }
        public TimeSheetState? State { get; set; }

        [DisplayName("Start Date")]
        [UIHint("Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [UIHint("Date")]
        public DateTime? EndDate { get; set; }

        public double TotalEffort { get; set; }

        public IPagedList<TimeSheetLineItem> TimeSheetLineItems { get; set; }
    }
}