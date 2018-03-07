using System;
using System.Globalization;
using System.Web.Mvc;
using Grid.Api.Infrastructure;
using Grid.BLL.Accounting.Interfaces;

namespace Grid.Accounting.Api
{
    public partial class InvoiceController : BaseApiController
    {
        readonly IInvoiceService _invoiceService;


        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var generalReceipt = _invoiceService.GetAllAccounts(WebUser.TenantId);
               var accountHeads = _invoiceService.GetAllIncomeHead(WebUser.TenantId);
               var payingAccount = _invoiceService.GetAllPayingAccountList(WebUser.TenantId);
               var vm = new { IncomeHeads = accountHeads, GeneralReceipts = generalReceipt, ReceivingAccounts = payingAccount };
               return vm;
           }));
        }



        [HttpPost]
        public virtual ActionResult Create(GeneralReceiptViewModel vm)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var dateOfReceived = DateTime.ParseExact(vm.DateOfReceived, "dd/MM/yyyy", CultureInfo.InvariantCulture);
               DateTime? chequeOrDDDate = null;
               if (vm.ChequeDDDate != null)
                   chequeOrDDDate = DateTime.ParseExact(vm.ChequeDDDate, "dd/MM/yyyy",
                       CultureInfo.InvariantCulture);

               var generalReciept = _invoiceService.CreateAccount(dateOfReceived, vm.Party,
                   vm.Reference, vm.Description, long.Parse(vm.SelectedAccountHead),
                       long.Parse(vm.SelectedPayingAccount), vm.Amount, vm.ChequeOrDDNo,
                       chequeOrDDDate, EntryType.Invoice,
                       WebUser.TenantId);
               return generalReciept;

           }));
        }

        [HttpPost]
        public virtual ActionResult Edit(GeneralReceiptViewModel vm)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var dateOfReceived = DateTime.ParseExact(vm.DateOfReceived, "dd/MM/yyyy", CultureInfo.InvariantCulture);
               DateTime? chequeOrDDDate = null;
               if (vm.ChequeDDDate != null)
                   chequeOrDDDate = DateTime.ParseExact(vm.ChequeDDDate, "dd/MM/yyyy",
                       CultureInfo.InvariantCulture);
               var generalReceipt = _invoiceService.UpdateAccount(dateOfReceived, vm.Party, vm.Reference, vm.Description, long.Parse(vm.SelectedAccountHead),
                      long.Parse(vm.SelectedPayingAccount), vm.Amount, vm.ChequeOrDDNo,
                       chequeOrDDDate, EntryType.Invoice,
                       WebUser.TenantId, vm.Id);
               return generalReceipt;
           }));
        }
        [HttpPost]
        public virtual ActionResult Delete(long id)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var result = _invoiceService.Delete(id, WebUser.Id);
               return result;
           }));
        }
        [HttpPost]
        public virtual ActionResult SettleInvoice(long id)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var result = _invoiceService.SettleInvoice(id);
                return result;
            }));
        }

    }
}