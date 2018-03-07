using System;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.TicketDesk.Entities;
using Grid.Features.TicketDesk.Entities.Enums;

namespace Grid.Features.TicketDesk.ViewModels
{
    public class ChangeTicketStatusViewModel: ViewModelBase
    {
        [UIHint("Date")]
        [Required]
        public DateTime? DueDate { get; set; }

        public TicketStatus Status { get; set; }

        public int? AssignedToUserId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }


        public ChangeTicketStatusViewModel()
        {
            
        }

        public ChangeTicketStatusViewModel(Ticket ticket)
        {
            Id = ticket.Id;
            DueDate = ticket.DueDate;
            Status = ticket.Status;
            AssignedToUserId = ticket.AssignedToUserId;
        }
    }
}
