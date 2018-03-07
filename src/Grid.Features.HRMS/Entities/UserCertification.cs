using Grid.Features.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grid.Features.HRMS.Entities
{
    public class UserCertification: EntityBase
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int CertificationId { get; set; }
        [ForeignKey("CertificationId")]
        public virtual Certification Certification { get; set; }
    }
}
