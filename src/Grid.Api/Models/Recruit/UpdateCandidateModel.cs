using System;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Api.Models.Recruit
{
    public class UpdateCandidateModel: ApiModelBase
    {
        public DateTime RecievedOn { get; set; }
        public CandidateStatus Status { get; set; }
        public CandidatesSource Source { get; set; }
        public int[] TechnologyIds { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string Email { get; set; }
        public string SecondaryEmail { get; set; }
        public string PhoneNo { get; set; }
        public string OfficePhone { get; set; }
        public string Skype { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string GooglePlus { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }

        public int? CandidateDesignation { get; set; }
        public string Address { get; set; }
        public string CommunicationAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Qualification { get; set; }
        public double? TotalExperience { get; set; }
        public double? CurrentCTC { get; set; }
        public double? ExpectedCTC { get; set; }
        public string Comments { get; set; }

        public UpdateCandidateModel(Candidate candidate)
        {
            Id = candidate.Id;
            RecievedOn = candidate.RecievedOn;
            Status = candidate.Status;
            Source = candidate.Source;
            FirstName = candidate.Person.FirstName;
            MiddleName = candidate.Person.MiddleName;
            LastName = candidate.Person.LastName;
            Gender = candidate.Person.Gender;
            Email = candidate.Email;
            SecondaryEmail = candidate.Person.SecondaryEmail;
            PhoneNo = candidate.Person.PhoneNo;
            OfficePhone = candidate.Person.OfficePhone;
            Skype = candidate.Person.Skype;
            Facebook = candidate.Person.Facebook;
            Twitter = candidate.Person.Twitter;
            LinkedIn = candidate.Person.LinkedIn;
            GooglePlus = candidate.Person.GooglePlus;
            Organization = candidate.Person.Organization;
            Designation = candidate.Person.Designation;
            CandidateDesignation = candidate.DesignationId;
            Address = candidate.Person.Address;
            CommunicationAddress = candidate.Person.CommunicationAddress;
            DateOfBirth = candidate.Person.DateOfBirth;
            Qualification = candidate.Qualification;
            TotalExperience = candidate.TotalExperience;
            CurrentCTC = candidate.CurrentCTC;
            ExpectedCTC = candidate.ExpectedCTC;
            Comments = candidate.Comments;
        }
    }
}
