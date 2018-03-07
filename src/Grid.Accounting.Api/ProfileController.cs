using System.Web.Mvc;
using Grid.Api.Infrastructure;
using Grid.BLL.HRMS.Interfaces;

namespace Grid.Accounting.Api
{
    public partial class ProfileController : BaseApiController
    {
        readonly IUserService _userService;
        readonly IAccountService _accountService;

        public ProfileController(IUserService userService, IAccountService accountService)
        {
            _userService = userService;
            _accountService = accountService;
        }  

        [HttpGet]
        public virtual ActionResult Index()
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var user = _userService.GetUserById(WebUser.Id);
                var selectedUser = new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Username,
                    Persons = user.Person
                };
                return selectedUser;
            }));
        }
        
        [HttpPost]
        [SwiftAuthorize(Asset.Property, OperationType.Manage)]
        public virtual ActionResult ChangePassword(ChangePasswordViewModel vm)
        {
             return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var user = _accountService.ChangePassword("", vm.CurrentPassword, vm.ConfirmPassword);
                return Json(user);
            }));
        }
	}
}