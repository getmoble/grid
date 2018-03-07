using Grid.Features.Recruit.Entities;

namespace Grid.Api.Models.Recruit
{
    public class ApiCandidateModel: ApiModelBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public double? TotalExperience { get; set; }
        public string Source { get; set; }

        public ApiCandidateModel(Candidate candidate)
        {
            Id = candidate.Id;
            if (candidate.Person != null)
            {
                Name = candidate.Person.Name;
            }
            Code = candidate.Code;
            TotalExperience = candidate.TotalExperience;
            Source = GetEnumDescription(candidate.Source);
        }
    }
}
