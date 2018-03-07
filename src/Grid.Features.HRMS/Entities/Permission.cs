using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;

namespace Grid.Features.HRMS.Entities
{
    public class Permission : EntityBase
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        [DisplayName("Permission Code")]
        public int PermissionCode { get; set; }
    }
}