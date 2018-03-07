using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.Entities
{
    public class InterviewRoundDocument: DocumentEntityBase
    {
        public int InterviewRoundId { get; set; }
        [ForeignKey("InterviewRoundId")]
        public virtual InterviewRound InterviewRound { get; set; }

        public InterviewRoundDocumentType DocumentType { get; set; }
    }
}
