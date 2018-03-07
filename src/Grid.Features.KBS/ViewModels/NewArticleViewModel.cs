using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.KBS.Entities;
using Grid.Features.KBS.Entities.Enums;

namespace Grid.Features.KBS.ViewModels
{
    public class NewArticleViewModel: ViewModelBase
    {
        public bool IsPublic { get; set; }

        public string Title { get; set; }

        [DisplayName("Article Version")]
        public float ArticleVersion { get; set; }

        // Category, we need to support multiple categories
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Summary { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }

        public string KeyWords { get; set; }
        public bool IsFeatured { get; set; }
        public ArticleState State { get; set; }
        public int Hits { get; set; }
        public int Rating { get; set; }
        public int Version { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}