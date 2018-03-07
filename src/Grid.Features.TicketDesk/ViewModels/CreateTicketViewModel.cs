using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.TicketDesk.Entities.Enums;

namespace Grid.Features.TicketDesk.ViewModels
{
    public class CreateTicketViewModel : ViewModelBase
    {
        public int TicketCategoryId { get; set; }

        public int TicketSubCategoryId { get; set; }

        [ Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [UIHint("Date")]
        [Required]
        [DisplayName("Due Date")]
        public DateTime? DueDate { get; set; }

        public TicketStatus Status { get; set; }

        public int? AssignedToUserId { get; set; }
    }
}
