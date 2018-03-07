namespace Swift.UI.ViewModels.OpeningBalance
{
    public class OpeningBalanceViewModel : ViewModelBase
    {
        public string Description { get; set; }
        public long AccountId { get; set; }
        public string SelectedAccountId { get; set; }
        public double Amount { get; set; }
        public string DateOfTransfer { get; set; }
    }
}
