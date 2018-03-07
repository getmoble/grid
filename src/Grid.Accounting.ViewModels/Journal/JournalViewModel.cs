namespace Swift.UI.ViewModels.Journal
{
    public class JournalViewModel : ViewModelBase
    {
        public string Reference { get; set; }
        public string Description { get; set; }
        public long FromAccountId { get; set; }
        public string SelectedFromAccount { get; set; }
        public long ToAccountId { get; set; }
        public string SelectedToAccount { get; set; }
        public double Amount { get; set; }
        public string DateOfTransfer { get; set; }
    }
}
