using System;
using System.Globalization;
using System.Web.Mvc;
using Grid.BLL.Accounting.Interfaces;

namespace Grid.Accounting.Api
{
    public partial class ContraController : Controller
    {
        readonly IContraService _contraService;


        public ContraController(IContraService contraService)
        {
            _contraService = contraService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var contra = _contraService.GetAllAccounts(WebUser.TenantId);
               var payingAccountHead = _contraService.GetAllPayingAccountList(WebUser.TenantId).ToList();
               var vm = new { Contras = contra, PayingAccountHeads = payingAccountHead };
               return vm;
           }));
        }

        [HttpPost]
        public virtual ActionResult Create(ContraViewModel vm)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var dateOfTransfer = DateTime.ParseExact(vm.DateOfTransfer, "dd/MM/yyyy", CultureInfo.InvariantCulture);

               var contra = _contraService.CreateAccount(dateOfTransfer, vm.Reference,
                   vm.Description, long.Parse(vm.SelectedFromAccount),
                       long.Parse(vm.SelectedToAccount), vm.Amount, EntryType.BankCashTransfer,
                       WebUser.TenantId);
               return contra;
           }));
        }

        [HttpPost]
        public virtual ActionResult Edit(ContraViewModel vm)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var dateOfTransfer = DateTime.ParseExact(vm.DateOfTransfer, "dd/MM/yyyy", CultureInfo.InvariantCulture);
               var contra = _contraService.UpdateAccount(dateOfTransfer, vm.Reference,
                   vm.Description, long.Parse(vm.SelectedFromAccount),
                       long.Parse(vm.SelectedToAccount), vm.Amount, EntryType.BankCashTransfer,
                       WebUser.TenantId, vm.Id);
               return contra;

           }));
        }
        [HttpPost]
        public virtual ActionResult Delete(long id)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var result = _contraService.Delete(id, WebUser.Id);
               return result;
           }));
        }

    }

}
