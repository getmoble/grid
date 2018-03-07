using System.Web;
using Grid.Features.Common;
using Grid.Features.KBS.Entities;
using Grid.Features.KBS.Entities.Enums;

namespace Grid.Features.KBS.ViewModels
{
    public class ArticleAttachmentViewModel: ViewModelBase
    {
        public int ArticleId { get; set; }

        public Article Article { get; set; }

        public ArticleAttachmentType AttachmentType { get; set; }

        public string DocumentPath { get; set; }

        public HttpPostedFileBase Document { get; set; }
    }
}