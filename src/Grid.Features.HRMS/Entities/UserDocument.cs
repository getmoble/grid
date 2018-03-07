using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.Common;

namespace Grid.Features.HRMS.Entities
{
    public class UserDocument: DocumentEntityBase
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public UserDocumentType DocumentType { get; set; }
    }
}
