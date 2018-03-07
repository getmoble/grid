using System;

namespace Grid.Features.Recruit.ViewModels.Candidate
{
    public class EditCandidateViewModel: NewCandidateViewModel
    {
        public EditCandidateViewModel()
        {
            
        }

        public EditCandidateViewModel(Entities.Candidate candidate)
        {
            Id = candidate.Id;
            Source = candidate.Source;
            Qualification = candidate.Qualification;
            TotalExperience = candidate.TotalExperience;
            ResumePath = candidate.ResumePath;
            PhotoPath = candidate.PhotoPath;
            Status = candidate.Status;
            Comments = candidate.Comments;
            CurrentCTC = candidate.CurrentCTC;
            ExpectedCTC = candidate.ExpectedCTC;
            Person = candidate.Person;
            RecievedOn = candidate.RecievedOn;

            CreatedOn = DateTime.UtcNow;
        }
    }
}