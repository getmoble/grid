using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.Entities
{
    public class Referal: EntityBase
    {
        public int JobOpeningId { get; set; }
        [DisplayName("Job Opening")]
        [ForeignKey("JobOpeningId")]
        public virtual JobOpening JobOpening { get; set; }

        public int CandidateId { get; set; }
        [DisplayName("Candidate")]
        [ForeignKey("CandidateId")]
        public virtual Candidate Candidate { get; set; }

        public int ReferedByUserId { get; set; }
        [DisplayName("Refered By")]
        [ForeignKey("ReferedByUserId")]
        public virtual User ReferedByUser { get; set; }

        public DateTime ReferedOn { get; set; }
        public string ReferenceLetter { get; set; }
        public ReferalState State { get; set; }
    }
}
