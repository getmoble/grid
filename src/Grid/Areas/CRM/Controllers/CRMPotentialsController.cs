using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Features.CRM.Services.Interfaces;
using Grid.Features.CRM.ViewModels;
using Grid.Features.HRMS.DAL.Interfaces;
using Newtonsoft.Json;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.CRM.Controllers
{
    [GridPermission(PermissionCode = 220)]
    public class CRMPotentialsController : CRMBaseController
    {
        private readonly ICRMLeadRepository _crmLeadRepository;
        private readonly ICRMPotentialRepository _crmPotentialRepository;
        private readonly ICRMPotentialActivityRepository _crmPotentialActivityRepository;
        private readonly ICRMSalesStageRepository _crmSalesStageRepository;
        private readonly ICRMPotentialCategoryRepository _crmPotentialCategoryRepository;
        private readonly ICRMContactRepository _crmContactRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICRMPotentialService _crmPotentialService;

        public CRMPotentialsController(ICRMLeadRepository crmLeadRepository,
                                       ICRMPotentialRepository crmPotentialRepository,
                                       ICRMPotentialActivityRepository crmPotentialActivityRepository,
                                       ICRMSalesStageRepository crmSalesStageRepository,
                                       ICRMPotentialCategoryRepository crmPotentialCategoryRepository,
                                       ICRMContactRepository crmContactRepository,
                                       IUserRepository userRepository,
                                       IUnitOfWork unitOfWork,
                                       ICRMPotentialService crmPotentialService)
        {
            _crmLeadRepository = crmLeadRepository;
            _crmPotentialRepository = crmPotentialRepository;
            _crmPotentialActivityRepository = crmPotentialActivityRepository;
            _crmSalesStageRepository = crmSalesStageRepository;
            _crmPotentialCategoryRepository = crmPotentialCategoryRepository;
            _crmContactRepository = crmContactRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _crmPotentialService = crmPotentialService;
        }

        #region Ajax Calls
        [HttpGet]
        public string GetAllActivitiesForCRMPotential(int id)
        {
            var activities = _crmPotentialActivityRepository.GetAllBy(r => r.CRMPotentialId == id, o => o.OrderByDescending(r => r.CreatedOn)).ToList();
            var list = JsonConvert.SerializeObject(activities, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return list;
        }


        [HttpPost]
        public JsonResult AddNote(CRMActivityViewModel vm)
        {
            var selectedPotential = _crmPotentialRepository.Get(vm.CRMPotentialId);
            if (selectedPotential != null)
            {
                // Add it as an Activity
                var newActivity = new CRMPotentialActivity
                {
                    Title = vm.Title,
                    Comment = vm.Comment,
                    CRMPotentialId = selectedPotential.Id,
                    CreatedByUserId = WebUser.Id
                };

                _crmPotentialActivityRepository.Create(newActivity);
                _unitOfWork.Commit();

                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteNote(int id)
        {
            _crmPotentialActivityRepository.Delete(id);
            _unitOfWork.Commit();

            return Json(true);
        }

        [HttpPost]
        public JsonResult ChangeSalesStage(CRMActivityViewModel vm)
        {
            var selectedCRMPotential = _crmPotentialRepository.Get(vm.CRMPotentialId);
            if (selectedCRMPotential != null)
            {
                selectedCRMPotential.SalesStageId = vm.StatusId.GetValueOrDefault();
                selectedCRMPotential.UpdatedByUserId = WebUser.Id;

                // Send Email Here to Tech Team

                var selectedStatus = _crmSalesStageRepository.Get(vm.StatusId.GetValueOrDefault());
                // Add it as an Activity
                if (selectedStatus != null)
                {
                    var newActivity = new CRMPotentialActivity
                    {
                        Title = selectedStatus.Name,
                        Comment = vm.Comment,
                        CRMPotentialId = selectedCRMPotential.Id,
                        CreatedByUserId = WebUser.Id
                    };

                    _crmPotentialActivityRepository.Create(newActivity);
                }

                _crmPotentialRepository.Update(selectedCRMPotential);
                _unitOfWork.Commit();

                return Json(true);
            }

            return Json(false);
        }


        #endregion

        public ActionResult Index()
        {
            var crmPotentials = _crmPotentialRepository.GetAllBy(o => o.OrderByDescending(s => s.CreatedOn), "AssignedToUser,Contact,SalesStage").ToList();
            return View(crmPotentials);
        }

        public ActionResult Details(int id)
        {
            var crmPotential = _crmPotentialRepository.GetBy(c => c.Id == id, "Contact.Person,AssignedToUser.Person,SalesStage");
            ViewBag.SalesStagesId = new SelectList(_crmSalesStageRepository.GetAll(), "Id", "Name", crmPotential.SalesStageId);
            var vm = new CRMPotentialDetailsViewModel(crmPotential);
            return View(vm);
        }

        [SelectList("LoggedInUser", "AssignedToUserId")]
        [SelectList("CRMPotentialCategory", "CategoryId")]
        [SelectList("CRMSalesStage", "SalesStageId")]
        [SelectList("CRMContact", "ContactId")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("LoggedInUser", "AssignedToUserId", SelectListState.Recreate)]
        [SelectList("CRMPotentialCategory", "CategoryId", SelectListState.Recreate)]
        [SelectList("CRMSalesStage", "SalesStageId", SelectListState.Recreate)]
        [SelectList("CRMContact", "ContactId", SelectListState.Recreate)]
        public ActionResult Create(CRMPotential cRMPotential)
        {
            if (ModelState.IsValid)
            {
                cRMPotential.CreatedByUserId = WebUser.Id;

                cRMPotential.Contact.CreatedByUserId = WebUser.Id;

                _crmPotentialRepository.Create(cRMPotential);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(cRMPotential);
        }

        [SelectList("CRMPotentialCategory", "CategoryId")]
        [SelectList("CRMSalesStage", "SalesStageId")]
        public ActionResult Convert(int id)
        {
            var selectedLead = _crmLeadRepository.GetBy(l => l.Id == id, "Person");
            var vm = new ConvertLeadViewModel();
            if (selectedLead != null)
            {
                vm.LeadId = selectedLead.Id;
                ViewBag.AssignedToUserId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", selectedLead.AssignedToUserId);
                vm.Person = selectedLead.Person;
                vm.Description = selectedLead.Description;
            }
            
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Convert(ConvertLeadViewModel vm)
        {
            var status = _crmPotentialService.Convert(vm.CreateAccount,
                                                      vm.CreatePotential,
                                                      vm.LeadId, 
                                                      vm.AssignedToUserId, 
                                                      vm.CategoryId, 
                                                      vm.ExpectedAmount,
                                                      vm.ExpectedCloseDate, 
                                                      vm.Description, 
                                                      vm.EnquiredOn, 
                                                      vm.SalesStageId, 
                                                      WebUser.Id);
            if (status)
            {
                return RedirectToAction("Index");
            }

            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var cRMPotential = _crmPotentialRepository.Get(id);

            ViewBag.CategoryId = new SelectList(_crmPotentialCategoryRepository.GetAll(), "Id", "Title", cRMPotential.CategoryId);
            ViewBag.AssignedToUserId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", cRMPotential.AssignedToUserId);
            ViewBag.ContactId = new SelectList(_crmContactRepository.GetAll("Person"), "Id", "Person.Name", cRMPotential.ContactId);
            ViewBag.SalesStageId = new SelectList(_crmSalesStageRepository.GetAll(), "Id", "Name", cRMPotential.SalesStageId);

            return View(cRMPotential);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("LoggedInUser", "AssignedToUserId", SelectListState.Recreate)]
        [SelectList("CRMPotentialCategory", "CategoryId", SelectListState.Recreate)]
        [SelectList("CRMSalesStage", "SalesStageId", SelectListState.Recreate)]
        [SelectList("CRMContact", "ContactId", SelectListState.Recreate)]
        public ActionResult Edit(CRMPotential cRMPotential)
        {
            if (ModelState.IsValid)
            {
                var selectedPotential = _crmPotentialRepository.GetBy(l => l.Id == cRMPotential.Id, "Contact.Person");

                if (selectedPotential != null)
                {
                    selectedPotential.AssignedToUserId = cRMPotential.AssignedToUserId;
                    selectedPotential.CategoryId = cRMPotential.CategoryId;
                    selectedPotential.ExpectedAmount = cRMPotential.ExpectedAmount;
                    selectedPotential.ExpectedCloseDate = cRMPotential.ExpectedCloseDate;
                    selectedPotential.EnquiredOn = cRMPotential.EnquiredOn;
                    selectedPotential.Description = cRMPotential.Description;

                    selectedPotential.UpdatedByUserId = WebUser.Id;
                }


                _crmPotentialRepository.Update(selectedPotential);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(cRMPotential);
        }

        public ActionResult Delete(int id)
        {
            var crmPotential = _crmPotentialRepository.Get(id);
            return CheckForNullAndExecute(crmPotential, () => View(crmPotential));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var status = _crmPotentialService.Delete(id);
            if (status)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
