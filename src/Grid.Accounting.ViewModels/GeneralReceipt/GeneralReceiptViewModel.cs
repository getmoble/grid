using System;

namespace Swift.UI.ViewModels.GeneralReceipt
{
    public class GeneralReceiptViewModel : ViewModelBase
    {
        public DateTime DateOfReceipt { get; set; }
        public string Party { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public long ExpenseAccountId { get; set; }
        public string SelectedAccountHead { get; set; }
        public string SelectedPayingAccount { get; set; }
        public double Amount { get; set; }
        public string ChequeOrDDNo { get; set; }
        public string DateOfReceived{ get; set; }
        public string ChequeDDDate { get; set; }

    }
}
