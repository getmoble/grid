using Grid.Features.Common;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;
using PagedList;

namespace Grid.Features.Recruit.ViewModels
{
    public class InterviewSearchViewModel: PagedViewModelBase
    {
        public int? JobId { get; set; }
        public int? CandidateId { get; set; }
        public int? InterviewerId { get; set; }
        public int? RoundId { get; set; }
        public InterviewStatus? Status { get; set; }

        public IPagedList<InterviewRound> Interviews { get; set; }

        public int Total { get; set; }
    }
}