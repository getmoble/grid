using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.Entities
{
    public class CandidateDocument: DocumentEntityBase
    {
        public int CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public Candidate Candidate { get; set; }

        public CandidateDocumentType DocumentType { get; set; }
    }
}