using System;
using System.Globalization;
using System.Web.Mvc;
using Grid.Api.Infrastructure;
using Grid.BLL.Accounting.Interfaces;

namespace Grid.Accounting.Api
{
    public partial class JournalController : BaseApiController
    {
        readonly IJournalService _journelService;


        public JournalController(IJournalService journalService)
        {
            _journelService = journalService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var journal = _journelService.GetAllAccounts(WebUser.TenantId);
               var nonTransactionAccounts = _journelService.NonTransactionAccount(WebUser.TenantId).ToList();
               var vm = new { Contras = journal, PayingAccountHeads = nonTransactionAccounts };
               return vm;
           }));
        }

        [HttpPost]
        public virtual ActionResult Create(JournalViewModel vm)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
             {
                 var dateOfTransfer = DateTime.ParseExact(vm.DateOfTransfer, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                 var journal = _journelService.CreateAccount(dateOfTransfer, vm.Reference,
                     vm.Description, long.Parse(vm.SelectedFromAccount),
                         long.Parse(vm.SelectedToAccount), vm.Amount, EntryType.Journal,
                         WebUser.TenantId);
                 return journal;
             }));
        }

        [HttpPost]
        public virtual ActionResult Edit(JournalViewModel vm)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var dateOfTransfer = DateTime.ParseExact(vm.DateOfTransfer, "dd/MM/yyyy", CultureInfo.InvariantCulture);
               var journal = _journelService.UpdateAccount(dateOfTransfer, vm.Reference,
                   vm.Description, long.Parse(vm.SelectedFromAccount),
                       long.Parse(vm.SelectedToAccount), vm.Amount, EntryType.Journal,
                       WebUser.TenantId, vm.Id);
               return journal;

           }));
        }
        [HttpPost]
        public virtual ActionResult Delete(long id)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var result = _journelService.Delete(id, WebUser.Id);
                return result;
            }));
        }


    }
}
