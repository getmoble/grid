namespace Swift.UI.ViewModels.Accounts
{
    public class AccountHeadViewModel : ViewModelBase
    {
        public string Name { get; set; }
        //public long AccountCategoryId { get; set; }
        public string Description { get; set; }
        //public string SelectedAccountCategory { get; set; }
        //public string SelectAccountCategoryId { get; set; }

        public long SelectedAccountHeadId { get; set; }
        public decimal OpeningBalance { get; set; }
        public bool IsTransaction { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
