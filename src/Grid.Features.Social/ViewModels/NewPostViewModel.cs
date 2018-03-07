using System.Web.Mvc;
using Grid.Features.Common;

namespace Grid.Features.Social.ViewModels
{
    public class NewPostViewModel: ViewModelBase
    {
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}
