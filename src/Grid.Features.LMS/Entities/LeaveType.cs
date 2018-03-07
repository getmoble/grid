using Grid.Features.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Grid.Features.LMS.Entities
{
    public class LeaveType: EntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }     
        public int MaxInAStretch { get; set; }
        public int MaxInMonth { get; set; }
        public bool CanCarryForward { get; set; }
        public int MaxCarryForward { get; set; }
       
    }
}
