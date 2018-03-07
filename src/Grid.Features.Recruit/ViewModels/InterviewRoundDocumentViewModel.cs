using System.Web;
using Grid.Features.Common;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.ViewModels
{
    public class InterviewRoundDocumentViewModel : ViewModelBase
    {
        public int InterviewRoundId { get; set; }

        public InterviewRound InterviewRound { get; set; }

        public InterviewRoundDocumentType DocumentType { get; set; }

        public string DocumentPath { get; set; }

        public HttpPostedFileBase Document { get; set; }
    }
}