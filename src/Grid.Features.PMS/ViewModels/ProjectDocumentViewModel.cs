using System.ComponentModel;
using System.Web;
using Grid.Features.Common;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.ViewModels
{
    public class ProjectDocumentViewModel : ViewModelBase
    {
        public int ProjectId { get; set; }
        public Entities.Project Project { get; set; }

        [DisplayName("Document Type")]
        public ProjectDocumentType DocumentType { get; set; }

        public string DocumentPath { get; set; }

        public HttpPostedFileBase Document { get; set; }
    }
}