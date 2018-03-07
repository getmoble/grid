using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;

namespace Grid.Features.HRMS.Entities
{
    public class Location: EntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Address")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DisplayName("Phone")]
        [UIHint("PhoneNumber")]
        public string Phone { get; set; }
    }
}
