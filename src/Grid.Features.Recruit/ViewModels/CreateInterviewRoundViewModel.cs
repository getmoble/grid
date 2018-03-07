using System;
using Grid.Features.Common;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.ViewModels
{
    public class CreateInterviewRoundViewModel: ViewModelBase
    {
        public int JobOpeningId { get; set; }
        public int CandidateId { get; set; }
        public int RoundId { get; set; }
        public int[] InterviewerIds { get; set; }
        public DateTime? ScheduledOn { get; set; }
        public InterviewStatus Status { get; set; }
        public string Comments { get; set; }
    }
}