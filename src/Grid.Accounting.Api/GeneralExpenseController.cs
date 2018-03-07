using System;
using System.Globalization;
using System.Web.Mvc;
using Grid.Api.Infrastructure;
using Grid.BLL.Accounting.Interfaces;

namespace Grid.Accounting.Api
{
    public partial class GeneralExpenseController : BaseApiController
    {
        readonly IGeneralExpenseService _generalExpenseService;

        public GeneralExpenseController(IGeneralExpenseService generalExpenseService)
        {
            _generalExpenseService = generalExpenseService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var generalExpense = _generalExpenseService.GetAllAccounts(WebUser.TenantId);
                var accountHeads = _generalExpenseService.GetAllExpenseHead(WebUser.TenantId);
                var payingAccount = _generalExpenseService.GetAllPayingAccountList(WebUser.TenantId);
                var vm = new { ExpenseHeads = accountHeads, GeneralExpenses = generalExpense, PayingAccounts = payingAccount };
                return vm;
            }));
        }

        [HttpPost]
        public virtual ActionResult Create(GeneralExpenseViewModel vm)
        {

            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var dateOfPayed = DateTime.ParseExact(vm.DateOfPayed, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime? chequeOrDDDate = null;
                if (vm.ChequeDDDate != null)
                    chequeOrDDDate = DateTime.ParseExact(vm.ChequeDDDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture);
                var generalExpense = _generalExpenseService.CreateAccount(dateOfPayed, vm.Party, vm.Reference,
                    vm.Description,
                    long.Parse(vm.SelectedAccountHead),
                    long.Parse(vm.SelectedPayingAccount), vm.Amount, vm.ChequeOrDDNo,
                    chequeOrDDDate, EntryType.GeneralExpense,
                    WebUser.TenantId);
                return generalExpense;

            }));
        }
        [HttpPost]
        public virtual ActionResult Edit(GeneralExpenseViewModel vm)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var dateOfPayed = DateTime.ParseExact(vm.DateOfPayed, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime? chequeOrDDDate = null;
                if (vm.ChequeDDDate != null)
                    chequeOrDDDate = DateTime.ParseExact(vm.ChequeDDDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture);
                var generalExpense = _generalExpenseService.UpdateAccount(dateOfPayed, vm.Party, vm.Reference,
                    vm.Description, long.Parse(vm.SelectedAccountHead),
                    long.Parse(vm.SelectedPayingAccount), vm.Amount, vm.ChequeOrDDNo,
                    chequeOrDDDate, EntryType.GeneralExpense,
                    WebUser.TenantId, vm.Id);
                return generalExpense;

            }));
        }
        [HttpPost]
        public virtual ActionResult Delete(long id)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var result = _generalExpenseService.Delete(id, WebUser.TenantId);
                return result;
            }));
        }

    }

}
