using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;

namespace Grid.Features.TicketDesk.Entities
{
    public class TicketSubCategory: EntityBase
    {
        public int TicketCategoryId { get; set; }
        [ForeignKey("TicketCategoryId")]
        public TicketCategory TicketCategory { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}