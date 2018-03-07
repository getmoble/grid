using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.Entities
{
    public class InterviewRound: EntityBase
    {
        public int JobOpeningId { get; set; }
        [ForeignKey("JobOpeningId")]
        public JobOpening JobOpening { get; set; }

        public int CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public Candidate Candidate { get; set; }

        public int RoundId { get; set; }
        [ForeignKey("RoundId")]
        public Round Round { get; set; }

        public int InterviewerId { get; set; }
        [ForeignKey("InterviewerId")]
        public User Interviewer { get; set; }

        [DisplayName("Scheduled On")]
        public DateTime? ScheduledOn { get; set; }

        public InterviewStatus Status { get; set; }

        [DisplayName("Comments")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
    }
}
