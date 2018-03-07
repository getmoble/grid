using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.Entities
{
    public class ProjectDocument : DocumentEntityBase
    {
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public ProjectDocumentType DocumentType { get; set; }
    }
}
