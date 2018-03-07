using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Grid.Features.Common;

namespace Grid.Features.Recruit.ViewModels.Candidate
{
    public class CandidateActivityViewModel: ViewModelBase
    {
        public int? StatusId { get; set; }

        public int CandidateId { get; set; }
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Comment { get; set; }
    }
}