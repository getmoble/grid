using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;

namespace Grid.Features.TicketDesk.Entities
{
    public class TicketActivity: UserCreatedEntityBase
    {
        public int TicketId { get; set; }
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}