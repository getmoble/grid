using System.Web;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Features.HRMS.ViewModels
{
    public class UserDocumentViewModel: ViewModelBase
    {
        public int UserId { get; set; }

        public Features.HRMS.Entities.User User { get; set; }

        public UserDocumentType DocumentType { get; set; }

        public string DocumentPath { get; set; }

        public HttpPostedFileBase Document { get; set; }
    }
}