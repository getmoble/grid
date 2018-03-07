using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Features.CRM.Services.Interfaces;
using Grid.Features.CRM.ViewModels;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.CRM.Controllers
{
    [GridPermission(PermissionCode = 220)]
    public class CRMAccountsController : CRMBaseController
    {
        private readonly ICRMAccountRepository _crmAccountRepository;
        private readonly ICRMContactRepository _crmContactRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICRMAccountService _crmAccountService;

        public CRMAccountsController(ICRMAccountRepository crmAccountRepository,
                                     ICRMContactRepository crmContactRepository,
                                     IUserRepository userRepository,
                                     IUnitOfWork unitOfWork,
                                     ICRMAccountService crmAccountService)
        {
            _crmAccountRepository = crmAccountRepository;
            _crmContactRepository = crmContactRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _crmAccountService = crmAccountService;
        }

        public ActionResult Index()
        {
            var crmAccounts = _crmAccountRepository.GetAll("AssignedToEmployee.User.Person,Parent").OrderByDescending(c => c.CreatedOn);
            return View(crmAccounts.ToList());
        }

        public ActionResult Details(int id)
        {
            var crmAccount = _crmAccountRepository.GetBy(a => a.Id == id, "AssignedToEmployee.User.Person");
            var contacts = _crmContactRepository.GetAllBy(c => c.ParentAccountId == crmAccount.Id, "Person").ToList();
            var vm = new AccountDetailsViewModel(crmAccount) {Contacts = contacts};
            return View(vm);
        }

        [SelectList("User", "AssignedToUserId")]
        [SelectList("CRMAccount", "ParentId")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("User", "AssignedToUserId", SelectListState.Recreate)]
        [SelectList("CRMAccount", "ParentId", SelectListState.Recreate)]
        public ActionResult Create(CRMAccount cRMAccount)
        {
            if (ModelState.IsValid)
            {
                cRMAccount.CreatedByUserId = WebUser.Id;

                _crmAccountRepository.Create(cRMAccount);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(cRMAccount);
        }

        public ActionResult Edit(int id)
        {
            var cRMAccount = _crmAccountRepository.Get(id);

            ViewBag.AssignedToUserId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", cRMAccount.AssignedToEmployeeId);
            ViewBag.ParentId = new SelectList(_crmAccountRepository.GetAll(), "Id", "Title", cRMAccount.ParentId);

            return View(cRMAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("User", "AssignedToUserId", SelectListState.Recreate)]
        [SelectList("CRMAccount", "ParentId", SelectListState.Recreate)]
        public ActionResult Edit(CRMAccount cRMAccount)
        {
            if (ModelState.IsValid)
            {
                var selectedAccount = _crmAccountRepository.Get(cRMAccount.Id);

                if (selectedAccount != null)
                {
                    selectedAccount.Title = cRMAccount.Title;
                    selectedAccount.Industry = cRMAccount.Industry;
                    selectedAccount.EmployeeCount = cRMAccount.EmployeeCount;
                    selectedAccount.FoundedOn = cRMAccount.FoundedOn;
                    selectedAccount.Email = cRMAccount.Email;
                    selectedAccount.PhoneNo = cRMAccount.PhoneNo;
                    selectedAccount.SecondaryEmail = cRMAccount.SecondaryEmail;
                    selectedAccount.OfficePhone = cRMAccount.OfficePhone;
                    selectedAccount.Website = cRMAccount.Website;
                    selectedAccount.Facebook = cRMAccount.Facebook;
                    selectedAccount.Twitter = cRMAccount.Twitter;
                    selectedAccount.GooglePlus = cRMAccount.GooglePlus;
                    selectedAccount.LinkedIn = cRMAccount.LinkedIn;
                    selectedAccount.City = cRMAccount.City;
                    selectedAccount.Country = cRMAccount.Country;
                    selectedAccount.Address = cRMAccount.Address;
                    selectedAccount.CommunicationAddress = cRMAccount.CommunicationAddress;
                    selectedAccount.Expertise = cRMAccount.Expertise;
                    selectedAccount.Description = cRMAccount.Description;
                    selectedAccount.AssignedToEmployeeId = cRMAccount.AssignedToEmployeeId;
                    selectedAccount.ParentId = cRMAccount.ParentId;
                    selectedAccount.UpdatedByUserId = WebUser.Id;

                    _crmAccountRepository.Update(selectedAccount);
                    _unitOfWork.Commit();

                    return RedirectToAction("Index");
                }
            }

            return View(cRMAccount);
        }

        public ActionResult Delete(int id)
        {
            var crmAccount = _crmAccountRepository.Get(id);
            return CheckForNullAndExecute(crmAccount, () => View(crmAccount));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var operationResult = _crmAccountService.Delete(id);
            if (operationResult.Status)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, operationResult.Message);
            return View();
        }
    }
}
