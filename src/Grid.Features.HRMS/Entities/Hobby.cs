using Grid.Features.Common;
using System.ComponentModel.DataAnnotations;

namespace Grid.Features.HRMS.Entities
{
    public class Hobby: EntityBase
    {
        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}