using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.RMS.Entities.Enums;

namespace Grid.Features.RMS.Entities
{
    public class RequirementDocument: DocumentEntityBase
    {
        public int RequirementId { get; set; }
        [ForeignKey("RequirementId")]
        public Requirement Requirement { get; set; }

        public RequirementDocumentType DocumentType { get; set; }
    }
}