using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.ViewModels
{
    public class InterviewRoundDetailsViewModel: ViewModelBase
    {
        public int JobOpeningId { get; set; }
        public JobOpening JobOpening { get; set; }

        public int CandidateId { get; set; }
        public Entities.Candidate Candidate { get; set; }

        public int RoundId { get; set; }
        public Round Round { get; set; }

        public int InterviewerId { get; set; }
        public Features.HRMS.Entities.User Interviewer { get; set; }

        [DisplayName("Scheduled On")]
        public DateTime? ScheduledOn { get; set; }

        public InterviewStatus Status { get; set; }

        [DisplayName("Comments")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        public List<InterviewRoundActivity> InterviewRoundActivities { get; set; }
        public List<InterviewRoundDocument> InterviewRoundDocuments { get; set; }

        public InterviewRoundDetailsViewModel()
        {
            InterviewRoundActivities = new List<InterviewRoundActivity>();
            InterviewRoundDocuments = new List<InterviewRoundDocument>();
        }

        public InterviewRoundDetailsViewModel(InterviewRound interviewRound)
        {
            Id = interviewRound.Id;
            Code = interviewRound.Code;
            JobOpeningId = interviewRound.JobOpeningId;
            JobOpening = interviewRound.JobOpening;
            CandidateId = interviewRound.CandidateId;
            Candidate = interviewRound.Candidate;
            RoundId = interviewRound.RoundId;
            Round = interviewRound.Round;
            InterviewerId = interviewRound.InterviewerId;
            Interviewer = interviewRound.Interviewer;
            ScheduledOn = interviewRound.ScheduledOn;
            Status = interviewRound.Status;
            Comments = interviewRound.Comments;
        }
    }
}
