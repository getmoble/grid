using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Grid.Features.Common;

namespace Grid.Features.RMS.ViewModels
{
    public class RequirementActivityViewModel: ViewModelBase
    {
        [DisplayName("New Activity")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string NewComment { get; set; }
    }
}