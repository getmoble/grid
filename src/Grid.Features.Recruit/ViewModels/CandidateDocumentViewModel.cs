using System.Web;
using Grid.Features.Common;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.ViewModels
{
    public class CandidateDocumentViewModel : ViewModelBase
    {
        public int CandidateId { get; set; }

        public Entities.Candidate Candidate { get; set; }

        public CandidateDocumentType DocumentType { get; set; }

        public string DocumentPath { get; set; }

        public HttpPostedFileBase Document { get; set; }
    }
}