using Grid.Features.TicketDesk.Entities;

namespace Grid.Features.TicketDesk.ViewModels
{
    public class EditTicketViewModel: CreateTicketViewModel
    {
        public EditTicketViewModel()
        {
            
        }

        public EditTicketViewModel(Ticket ticket)
        {
            Id = ticket.Id;
            TicketCategoryId = ticket.TicketCategoryId;
            TicketSubCategoryId = ticket.TicketSubCategoryId;
            Title = ticket.Title;
            Description = ticket.Description;
            DueDate = ticket.DueDate;
            Status = ticket.Status;
            AssignedToUserId = ticket.AssignedToUserId;
        }
    }
}
