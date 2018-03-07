using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;

namespace Grid.Features.KBS.Entities
{
    public class ArticleVersion: UserCreatedEntityBase
    {
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }

        public int Version { get; set; }
        public string Content { get; set; }
    }
}