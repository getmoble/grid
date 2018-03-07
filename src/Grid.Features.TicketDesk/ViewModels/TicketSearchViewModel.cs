using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.TicketDesk.Entities;
using Grid.Features.TicketDesk.Entities.Enums;
using PagedList;

namespace Grid.Features.TicketDesk.ViewModels
{
    public class TicketSearchViewModel : PagedViewModelBase
    {
        public bool? Mine { get; set; }

        public bool HideCompleted { get; set; }

        public int? TicketCategoryId { get; set; }
        public int? TicketSubCategoryId { get; set; }
        public TicketStatus? TicketStatus { get; set; }

        public int? AssignedToUserId { get; set; }

        public string Title { get; set; }

        [DisplayName("Start Date")]
        [UIHint("Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [UIHint("Date")]
        public DateTime? EndDate { get; set; }

        public IPagedList<Ticket> Tickets { get; set; }
    }
}