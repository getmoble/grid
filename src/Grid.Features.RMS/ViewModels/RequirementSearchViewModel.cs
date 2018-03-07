using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.RMS.Entities;
using Grid.Features.RMS.Entities.Enums;
using PagedList;

namespace Grid.Features.RMS.ViewModels
{
    public class RequirementSearchViewModel : PagedViewModelBase
    {
        public int? AssignedToUserId { get; set; }
        public int? SourceId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public RequirementStatus? Status { get; set; }
        
        [DisplayName("Start Date")]
        [UIHint("Date")]
        public DateTime? StartDate { get; set; }
        
        [DisplayName("End Date")]
        [UIHint("Date")]
        public DateTime? EndDate { get; set; }
        public IPagedList<Requirement> Requirements { get; set; }

        public int Total { get; set; }
    }
}
