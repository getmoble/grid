using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;

namespace Grid.Features.HRMS.Entities
{
    public class Award: EntityBase
    {
        [Required]
        public string Title { get; set; }

        [MaxLength(250)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}