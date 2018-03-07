using System.Web;
using Grid.Features.Common;
using Grid.Features.RMS.Entities;
using Grid.Features.RMS.Entities.Enums;

namespace Grid.Features.RMS.ViewModels
{
    public class RequirementDocumentViewModel: ViewModelBase
    {
        public int RequirementId { get; set; }

        public Requirement Requirement { get; set; }

        public RequirementDocumentType DocumentType { get; set; }

        public string DocumentPath { get; set; }

        public HttpPostedFileBase Document { get; set; }
    }
}