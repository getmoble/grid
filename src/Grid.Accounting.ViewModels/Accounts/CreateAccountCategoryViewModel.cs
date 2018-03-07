using Swift.Entities.Enums;

namespace Swift.UI.ViewModels.Accounts
{
    public class CreateAccountCategoryViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public AccountTypes AccountType { get; set; }
        public string Remarks { get; set; }
        public string SelectedAccountCategory { get; set; }
    }
}
