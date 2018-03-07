using System.Web.Mvc;
using Grid.Api.Infrastructure;
using Grid.BLL.Accounting.Interfaces;

namespace Grid.Accounting.Api
{
    public partial class AccountHeadController : BaseApiController
    {
        readonly IAccountHeadService _accountHeadService;

        public AccountHeadController(IAccountHeadService accountHeadService)
        {
            _accountHeadService = accountHeadService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var accounts = _accountHeadService.GetAllAccounts(WebUser.TenantId).ToList();
                var vm = new { AccountHeads = accounts };
                return vm;

                //var result = JsonConvert.SerializeObject(vm,
                //                                      Formatting.None,
                //                                      new JsonSerializerSettings()
                //                                      {
                //                                          ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                //                                      });

                //return Content(result, "application/json");

            }));
        }


        [HttpPost]
        public virtual ActionResult Create(AccountHeadViewModel vm)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var accountHead = _accountHeadService.CreateAccount(vm.Name, vm.SelectedAccountHeadId, vm.Description, vm.OpeningBalance, vm.IsTransaction, WebUser.TenantId);
                return accountHead;
            }));
        }

        [HttpPost]
        public virtual ActionResult Edit(AccountHeadViewModel vm)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var accountHead = _accountHeadService.Update(vm.Name, vm.SelectedAccountHeadId, vm.Description, vm.OpeningBalance, vm.IsTransaction, WebUser.Id, vm.Id);
                return accountHead;
            }));

        }

        [HttpPost]
        public virtual ActionResult Delete(long id)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var result = _accountHeadService.Delete(id, WebUser.Id);
                return result;
            }));
        }

    }

}
