using System;
using System.Globalization;
using System.Web.Mvc;
using Grid.Api.Infrastructure;
using Grid.BLL.Accounting.Interfaces;

namespace Grid.Accounting.Api
{
    public partial class GeneralReceiptController : BaseApiController
    {
        readonly IGeneralReceiptService _generalReceiptService;

        public GeneralReceiptController(IGeneralReceiptService generalReceiptService)
        {
            _generalReceiptService = generalReceiptService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var generalReceipt = _generalReceiptService.GetAllAccounts(WebUser.TenantId);
               var accountHeads = _generalReceiptService.GetAllIncomeHead(WebUser.TenantId);
               var payingAccount = _generalReceiptService.GetAllPayingAccountList(WebUser.TenantId);
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

               var generalReciept = _generalReceiptService.CreateAccount(dateOfReceived, vm.Party,
                   vm.Reference, vm.Description, long.Parse(vm.SelectedAccountHead),
                       long.Parse(vm.SelectedPayingAccount), vm.Amount, vm.ChequeOrDDNo,
                       chequeOrDDDate, EntryType.GeneralReciept,
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
               var generalReceipt = _generalReceiptService.UpdateAccount(dateOfReceived, vm.Party, vm.Reference, vm.Description, long.Parse(vm.SelectedAccountHead),
                      long.Parse(vm.SelectedPayingAccount), vm.Amount, vm.ChequeOrDDNo,
                       chequeOrDDDate, EntryType.GeneralReciept,
                       WebUser.TenantId, vm.Id);
               return generalReceipt;
           }));
        }
        [HttpPost]
        public virtual ActionResult Delete(long id)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var result = _generalReceiptService.Delete(id, WebUser.Id);
               return result;
           }));
        }

    }

}

