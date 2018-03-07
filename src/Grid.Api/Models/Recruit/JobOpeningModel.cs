using Grid.Features.Recruit.Entities;

namespace Grid.Api.Models.Recruit
{
    public class JobOpeningModel: ApiModelBase
    {
        public string Title { get; set; }
        public int Vacancies { get; set; }
        public string Status { get; set; }

        public JobOpeningModel(JobOpening jobOpening)
        {
            Id = jobOpening.Id;
            Title = jobOpening.Title;
            Vacancies = jobOpening.NoOfVacancies;
            Status = GetEnumDescription(jobOpening.OpeningStatus);
            CreatedOn = jobOpening.CreatedOn;
        }
    }
}
