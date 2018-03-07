namespace Swift.UI.ViewModels.GeneralExpense
{
    public class GeneralExpenseViewModel : ViewModelBase
    {
        public string Party { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public long ExpenseAccountId { get; set; }
        public string SelectedAccountHead { get; set; }
        public double Amount { get; set; }
        public string SelectedPayingAccount { get; set; }
        public string ChequeOrDDNo { get; set; }
        public string DateOfPayed { get; set; }
        public string ChequeDDDate { get; set; }
        public string TransactionId { get; set; }
    }
}
