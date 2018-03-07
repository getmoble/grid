using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Grid.Features.Common;

namespace Grid.Features.RMS.ViewModels
{
    public class ChangeRequirementViewModel: ViewModelBase
    {
        public int RequirementId { get; set; }
        public string Title { get; set; }
        
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Comment { get; set; }
    }
}