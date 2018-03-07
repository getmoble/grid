using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.HRMS;

namespace Grid.Features.RMS.Entities
{
    public class RequirementCategory : UserCreatedEntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }
    }
}