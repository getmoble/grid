using System.ComponentModel.DataAnnotations;
using Grid.Features.HRMS;

namespace Grid.Features.CRM.Entities
{
    public class CRMLeadStatus : UserCreatedEntityBase
    {
        [Required]
        public string Name { get; set; }
    }
}
