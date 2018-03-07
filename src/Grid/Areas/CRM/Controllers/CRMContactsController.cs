using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Features.CRM.Services.Interfaces;
using Grid.Features.CRM.ViewModels;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.CRM.Controllers
{
    [GridPermission(PermissionCode = 220)]
    public class CRMContactsController : CRMBaseController
    {
        private readonly ICRMAccountRepository _crmAccountRepository;
        private readonly ICRMContactRepository _crmContactRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICRMContactService _crmContactService;

        public CRMContactsController(ICRMContactRepository crmContactRepository,
                                     ICRMAccountRepository crmAccountRepository,
                                     IUnitOfWork unitOfWork,
                                     ICRMContactService crmContactService)
        {
            _crmContactRepository = crmContactRepository;
            _crmAccountRepository = crmAccountRepository;
            _unitOfWork = unitOfWork;
            _crmContactService = crmContactService;
        }

        [SelectList("CRMAccount", "AccountId")]
        public ActionResult Index(ContactSearchViewModel vm)
        {
            Func<IQueryable<CRMContact>, IQueryable<CRMContact>> contactFilter = q =>
            {
                q = q.Include(c => c.ParentAccount).Include(c => c.Person);

                if (vm.AccountId.HasValue)
                {
                    q = q.Where(r => r.ParentAccountId == vm.AccountId.Value);
                }

                q = q.OrderByDescending(c => c.CreatedOn);

                return q;
            };

            var contacts = _crmContactRepository.Search(contactFilter);
            vm.Contacts = contacts.ToList();

            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var crmContact = _crmContactRepository.Get(id);
            return CheckForNullAndExecute(crmContact, () => View(crmContact));
        }

        [SelectList("CRMAccount", "ParentAccountId")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("CRMAccount", "ParentAccountId", SelectListState.Recreate)]
        public ActionResult Create(CRMContact cRMContact)
        {
            if (ModelState.IsValid)
            {
                cRMContact.CreatedByUserId = WebUser.Id;

                _crmContactRepository.Create(cRMContact);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(cRMContact);
        }

        public ActionResult Edit(int id)
        {
            var crmContact = _crmContactRepository.Get(id);
            ViewBag.ParentAccountId = new SelectList(_crmAccountRepository.GetAll(), "Id", "Title", crmContact.ParentAccountId);
            return CheckForNullAndExecute(crmContact, () => View(crmContact));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("CRMAccount", "ParentAccountId", SelectListState.Recreate)]
        public ActionResult Edit(CRMContact cRMContact)
        {
            if (ModelState.IsValid)
            {
                var selectedContact = _crmContactRepository.GetBy(l => l.Id == cRMContact.Id);

                if (selectedContact != null)
                {
                    selectedContact.ParentAccountId = cRMContact.ParentAccountId;
                    selectedContact.Person.FirstName = cRMContact.Person.FirstName;
                    selectedContact.Person.LastName = cRMContact.Person.LastName;
                    selectedContact.Person.Gender = cRMContact.Person.Gender;
                    selectedContact.Person.Email = cRMContact.Person.Email;
                    selectedContact.Person.Organization = cRMContact.Person.Organization;
                    selectedContact.Person.Designation = cRMContact.Person.Designation;
                    selectedContact.Person.PhoneNo = cRMContact.Person.PhoneNo;
                    selectedContact.Person.SecondaryEmail = cRMContact.Person.SecondaryEmail;
                    selectedContact.Person.OfficePhone = cRMContact.Person.OfficePhone;
                    selectedContact.Person.Website = cRMContact.Person.Website;
                    selectedContact.Person.Skype = cRMContact.Person.Skype;
                    selectedContact.Person.Facebook = cRMContact.Person.Facebook;
                    selectedContact.Person.Twitter = cRMContact.Person.Twitter;
                    selectedContact.Person.GooglePlus = cRMContact.Person.GooglePlus;
                    selectedContact.Person.LinkedIn = cRMContact.Person.LinkedIn;
                    selectedContact.Person.City = cRMContact.Person.City;
                    selectedContact.Person.Country = cRMContact.Person.Country;
                    selectedContact.Person.Address = cRMContact.Person.Address;
                    selectedContact.Person.CommunicationAddress = cRMContact.Person.CommunicationAddress;
                    selectedContact.Person.DateOfBirth = cRMContact.Person.DateOfBirth;
                    selectedContact.Expertise = cRMContact.Expertise;
                    selectedContact.Comments = cRMContact.Comments;

                    selectedContact.UpdatedByUserId = WebUser.Id;

                    _crmContactRepository.Update(selectedContact);
                    _unitOfWork.Commit();

                    return RedirectToAction("Index");
                }
            }

            return View(cRMContact);
        }

        public ActionResult Delete(int id)
        {
            var crmContact = _crmContactRepository.Get(id);
            return CheckForNullAndExecute(crmContact, () => View(crmContact));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var operationResult = _crmContactService.Delete(id);
            if (operationResult.Status)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, operationResult.Message);
            return View();
        }
    }
}
