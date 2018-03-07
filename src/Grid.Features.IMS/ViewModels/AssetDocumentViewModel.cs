using System.Web;
using Grid.Features.Common;
using Grid.Features.IMS.Entities;
using Grid.Features.IMS.Entities.Enums;

namespace Grid.Features.IMS.ViewModels
{
    public class AssetDocumentViewModel : ViewModelBase
    {
        public int AssetId { get; set; }

        public Asset Asset { get; set; }

        public AssetDocumentType DocumentType { get; set; }

        public string DocumentPath { get; set; }

        public HttpPostedFileBase Document { get; set; }
    }
}