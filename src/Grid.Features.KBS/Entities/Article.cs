using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;
using Grid.Features.KBS.Entities.Enums;

namespace Grid.Features.KBS.Entities
{
    public class Article: UserCreatedEntityBase
    {
        public bool IsPublic { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }

        // This is user provided Version
        public float ArticleVersion { get; set; }

        public string Content { get; set; }
        public string KeyWords { get; set; }

        [DisplayName("Is Featured")]
        public bool IsFeatured { get; set; }

        public ArticleState State { get; set; }
        public int Hits { get; set; }
        public int Rating { get; set; }
        public int Version { get; set; }

        // Category, we need to support multiple categories
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}