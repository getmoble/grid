using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;

namespace Grid.Features.HRMS.ViewModels
{
    public class CreateRoleViewModel: ViewModelBase
    {
        [Column(TypeName = "varchar")]
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(250)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int[] PermissionIds { get; set; }
    }
}
