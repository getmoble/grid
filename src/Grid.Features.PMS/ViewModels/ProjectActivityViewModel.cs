using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Grid.Features.Common;

namespace Grid.Features.PMS.ViewModels
{
    public class ProjectActivityViewModel : ViewModelBase
    {
        public int ProjectId { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Comment { get; set; }
    }
}