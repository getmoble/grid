using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.Recruit.Entities
{
    public class CandidateTechnologyMap: EntityBase
    {
        public int CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public virtual Candidate Candidate { get; set; }

        public int TechnologyId { get; set; }
        [ForeignKey("TechnologyId")]
        public virtual Technology Technology { get; set; }
    }
}