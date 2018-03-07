using System;
using Grid.Features.TicketDesk.Entities;

namespace Grid.Api.Models.TicketDesk
{
    public class TicketModel: ApiModelBase
    {
        // Details 
        public string Description { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        
        // List
        public string Title { get; set; }
        public DateTime? DueDate { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public TicketModel(Ticket ticket)
        {
            Id = ticket.Id;
            Title = ticket.Title;
            Description = ticket.Description;
            DueDate = ticket.DueDate;

            if (ticket.TicketCategory != null)
            {
                Category = ticket.TicketCategory.Title;
            }

            if (ticket.TicketSubCategory != null)
            {
                SubCategory = ticket.TicketSubCategory.Title;
            }

            if (ticket.AssignedToUser?.Person != null)
            {
                AssignedTo = ticket.AssignedToUser.Person.Name;
            }

            if (ticket.CreatedByUser?.Person != null)
            {
                CreatedBy = ticket.CreatedByUser.Person.Name;
            }

            Status = GetEnumDescription(ticket.Status);

            LastUpdatedOn = ticket.UpdatedOn;
            CreatedOn = ticket.CreatedOn;
        }
    }
}
