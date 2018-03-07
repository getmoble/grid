using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;

namespace Grid.Features.HRMS.Entities
{
    public class RolePermission : EntityBase
    {
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        public int PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}