using Grid.Features.Common;
using Grid.Features.HRMS.Entities.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Grid.Features.HRMS.ViewModels
{
    public class DesignationViewModel : ViewModelBase
    {
        public int DepartmentId { get; set; }

        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public Band? Band { get; set; }

        [DisplayName("Mail Alias")]
        [UIHint("Email")]
        public string MailAlias { get; set; }
    }
}
