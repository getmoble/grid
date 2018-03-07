using System.Collections.Generic;
using Grid.Features.KBS.Entities;

namespace Grid.Features.KBS.ViewModels
{
    public class EditArticleViewModel: NewArticleViewModel
    {
        public List<ArticleAttachment> Attachments { get; set; }
        public List<ArticleVersion> Versions { get; set; }

        public EditArticleViewModel()
        {
            Attachments = new List<ArticleAttachment>();
            Versions = new List<ArticleVersion>();
        }

        public EditArticleViewModel(Article article)
        {
            Id = article.Id;
            CreatedOn = article.CreatedOn;
            Title = article.Title;
            IsPublic = article.IsPublic;
            Summary = article.Summary;
            Content = article.Content;
            KeyWords = article.KeyWords;
            IsFeatured = article.IsFeatured;
            State = article.State;
            CategoryId = article.CategoryId;
            ArticleVersion = article.ArticleVersion;
            Version = article.Version;
        }
    }
}