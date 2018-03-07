using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;


namespace Grid.ViewModels
{
    public class ChangePasswordViewModel: ViewModelBase
    {
        [DisplayName("Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DisplayName("Confirm New Password")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}