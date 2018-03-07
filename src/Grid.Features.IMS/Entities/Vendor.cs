using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;

namespace Grid.Features.IMS.Entities
{
    public class Vendor: EntityBase
    {
        [DisplayName("Name")]
        public string Title { get; set; }

        [UIHint("Email")]
        public string Email { get; set; }

        [UIHint("PhoneNumber")]
        public string Phone { get; set; }

        public string WebSite { get; set; }

        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DisplayName("Contact Person Name")]
        public string ContactPerson { get; set; }

        [DisplayName("Contact Person Email")]
        [UIHint("Email")]
        public string ContactPersonEmail { get; set; }

        [DisplayName("Contact Person Phone")]
        [UIHint("PhoneNumber")]
        public string ContactPersonPhone { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}