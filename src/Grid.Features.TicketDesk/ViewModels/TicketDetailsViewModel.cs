using System;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.TicketDesk.Entities;
using Grid.Features.TicketDesk.Entities.Enums;

namespace Grid.Features.TicketDesk.ViewModels
{
    public class TicketDetailsViewModel: ViewModelBase
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

        public int CreatedByUserId { get; set; }
        public Features.HRMS.Entities.User CreatedByUser { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public TicketDetailsViewModel(Ticket ticket)
        {
            Id = ticket.Id;
            TicketCategoryId = ticket.TicketCategoryId;
            TicketCategory = ticket.TicketCategory;
            TicketSubCategoryId = ticket.TicketSubCategoryId;
            TicketSubCategory = ticket.TicketSubCategory;
            Title = ticket.Title;
            Description = ticket.Description;
            Status = ticket.Status;
            AssignedToUserId = ticket.AssignedToUserId;
            AssignedToUser = ticket.AssignedToUser;
            CreatedByUserId = ticket.CreatedByUserId;
            CreatedByUser = ticket.CreatedByUser;

            DueDate = ticket.DueDate;
            UpdatedOn = ticket.UpdatedOn;
        }
    }
}