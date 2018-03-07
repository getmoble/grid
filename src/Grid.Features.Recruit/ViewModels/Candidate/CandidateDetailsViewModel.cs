using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.ViewModels.Candidate
{
    public class CandidateDetailsViewModel: ViewModelBase
    {
        public CandidatesSource Source { get; set; }

        public string Company { get; set; }
        public string Designation { get; set; }

        public string Qualification { get; set; }

        [DisplayName("Total Experience")]
        public double? TotalExperience { get; set; }
        public string ResumePath { get; set; }
        public string PhotoPath { get; set; }
        public CandidateStatus Status { get; set; }

        [DisplayName("Current CTC")]
        public double? CurrentCTC { get; set; }
        [DisplayName("Expected CTC")]
        public double? ExpectedCTC { get; set; }

        public long PersonId { get; set; }
        public Person Person { get; set; }

        [DisplayName("Comments")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        [DisplayName("Recieved On")]
        [UIHint("Date")]
        [Required]
        public DateTime RecievedOn { get; set; }

        public List<InterviewRound> InterviewRounds { get; set; }
        public List<CandidateDocument> CandidateDocuments { get; set; }

        // Used in the Intervew Modal
        public InterviewStatus InterviewStatus { get; set; }

        public CandidateDetailsViewModel()
        {
            InterviewRounds = new List<InterviewRound>();
            CandidateDocuments = new List<CandidateDocument>();
        }

        public CandidateDetailsViewModel(Entities.Candidate candidate)
        {
            Id = candidate.Id;
            Code = candidate.Code;
            Source = candidate.Source;
            Qualification = candidate.Qualification;
            TotalExperience = candidate.TotalExperience;
            ResumePath = candidate.ResumePath;
            PhotoPath = candidate.PhotoPath;
            Status = candidate.Status;
            Comments = candidate.Comments;
            CurrentCTC = candidate.CurrentCTC;
            ExpectedCTC = candidate.ExpectedCTC;
            PersonId = candidate.PersonId;
            Person = candidate.Person;
            RecievedOn = candidate.RecievedOn;
            CreatedOn = candidate.CreatedOn;
        }
    }
}