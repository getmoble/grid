using Grid.Features.Common;
using Grid.Features.KBS.Entities;
using PagedList;

namespace Grid.Features.KBS.ViewModels
{
    public class ArticleSearchViewModel: PagedViewModelBase
    {
        public string Title { get; set; }

        public int? CategoryId { get; set; }

        public IPagedList<Article> Articles { get; set; }
    }
}