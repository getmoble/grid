using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.KBS.Entities.Enums;

namespace Grid.Features.KBS.Entities
{
    public class ArticleAttachment: DocumentEntityBase
    {
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }

        public ArticleAttachmentType AttachmentType { get; set; }
    }
}