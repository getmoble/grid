using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;

namespace Grid.Features.TicketDesk.Entities
{
    public class TicketCategory: EntityBase
    {
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}