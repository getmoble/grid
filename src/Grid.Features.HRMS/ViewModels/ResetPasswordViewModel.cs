using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;

namespace Grid.Features.HRMS.ViewModels
{
    public class ResetPasswordViewModel: ViewModelBase
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        [Required]
        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DisplayName("Confirm New Password")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}