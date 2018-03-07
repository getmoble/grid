using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;
using Grid.Features.TicketDesk.Entities.Enums;

namespace Grid.Features.TicketDesk.Entities
{
    public class Ticket: UserCreatedEntityBase
    {
        public int TicketCategoryId { get; set; }
        [ForeignKey("TicketCategoryId")]
        public TicketCategory TicketCategory { get; set; }

        public int TicketSubCategoryId { get; set; }
        [ForeignKey("TicketSubCategoryId")]
        public TicketSubCategory TicketSubCategory { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [UIHint("Date")]
        public DateTime? DueDate { get; set; }

        public TicketStatus Status { get; set; }

        public int? AssignedToUserId { get; set; }
        [ForeignKey("AssignedToUserId")]
        public User AssignedToUser { get; set; }
    }
}