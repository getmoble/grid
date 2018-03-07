using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;

namespace Grid.Features.Recruit.Entities
{
    public class Round: EntityBase
    {
        [Required]
        public string Title { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
