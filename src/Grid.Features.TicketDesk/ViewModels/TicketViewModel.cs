using System;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.TicketDesk.Entities;
using Grid.Features.TicketDesk.Entities.Enums;
using Grid.Infrastructure.Extensions;

namespace Grid.Features.TicketDesk.ViewModels
{
    public class TicketViewModel: ViewModelBase
    {
        public int TicketCategoryId { get; set; }
        public TicketCategory TicketCategory { get; set; }

        public int TicketSubCategoryId { get; set; }
        public TicketSubCategory TicketSubCategory { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public TicketStatus Status { get; set; }

        public int? AssignedToUserId { get; set; }
        public Features.HRMS.Entities.User AssignedToUser { get; set; }

        public Features.HRMS.Entities.User CreatedByUser { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public TicketViewModel()
        {
            
        }

        public TicketViewModel(Ticket ticket)
        {
            Id = ticket.Id;
            Title = ticket.Title.Truncate();
            TicketCategoryId = ticket.TicketCategoryId;
            TicketCategory = ticket.TicketCategory;
            TicketSubCategoryId = ticket.TicketSubCategoryId;
            TicketSubCategory = ticket.TicketSubCategory;
            Description = ticket.Description;
            Status = ticket.Status;
            AssignedToUserId = ticket.AssignedToUserId;
            AssignedToUser = ticket.AssignedToUser;

            CreatedByUser = ticket.CreatedByUser;
            UpdatedOn = ticket.UpdatedOn;
        }
    }
}