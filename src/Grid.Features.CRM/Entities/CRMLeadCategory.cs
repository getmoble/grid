using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.HRMS;

namespace Grid.Features.CRM.Entities
{
    public class CRMLeadCategory : UserCreatedEntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}