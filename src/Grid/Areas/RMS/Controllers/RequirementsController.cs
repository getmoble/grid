using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Text;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.EmailService;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Entities;
using Grid.Features.RMS.Entities.Enums;
using Grid.Features.RMS.Services.Interfaces;
using Grid.Features.RMS.ViewModels;
using Grid.Features.RMS.ViewModels.Requirements;
using Grid.Filters;
using Grid.Infrastructure.Extensions;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.RMS.Controllers
{
    [GridPermission(PermissionCode = 225)]
    public class RequirementsController : GridBaseController
    {
        private readonly IRequirementRepository _requirementRepository;
        private readonly IRequirementActivityRepository _requirementActivityRepository;
        private readonly IRequirementDocumentRepository _requirementDocumentRepository;
        private readonly IRequirementTechnologyMapRepository _requirementTechnologyMapRepository;
        private readonly IRequirementCategoryRepository _requirementCategoryRepository;

        private readonly ITechnologyRepository _technologyRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICRMContactRepository _crmContactRepository;
        private readonly ICRMLeadSourceRepository _crmLeadSourceRepository;

        private readonly IRequirementService _requirementService;
        private readonly EmailComposerService _emailComposerService;
        private readonly IUnitOfWork _unitOfWork;

        public RequirementsController(IRequirementRepository requirementRepository,
                                      IRequirementActivityRepository requirementActivityRepository,
                                      IRequirementDocumentRepository requirementDocumentRepository,
                                      IRequirementTechnologyMapRepository requirementTechnologyMapRepository,
                                      IRequirementCategoryRepository requirementCategoryRepository,
                                      ITechnologyRepository technologyRepository,
                                      IUserRepository userRepository,
                                      ICRMContactRepository crmContactRepository,
                                      ICRMLeadSourceRepository crmLeadSourceRepository,
                                      IRequirementService requirementService,
                                      EmailComposerService emailComposerService,
                                      IUnitOfWork unitOfWork)
        {
            _requirementRepository = requirementRepository;
            _requirementActivityRepository = requirementActivityRepository;
            _requirementDocumentRepository = requirementDocumentRepository;
            _requirementTechnologyMapRepository = requirementTechnologyMapRepository;
            _requirementCategoryRepository = requirementCategoryRepository;
            _requirementService = requirementService;

            _technologyRepository = technologyRepository;
            _userRepository = userRepository;
            _crmContactRepository = crmContactRepository;
            _crmLeadSourceRepository = crmLeadSourceRepository;

            _unitOfWork = unitOfWork;
            _emailComposerService = emailComposerService;
        }

        private void AddRequirementActivity(RequirementActivity newActivity)
        {
            newActivity.CreatedByUserId = WebUser.Id;

            _requirementActivityRepository.Create(newActivity);
            _unitOfWork.Commit();

            // Send Email, Email Template name is hard corded - Need to change later
            // Replace the hard coded emails with settings or a team.
            #if !DEBUG
                _emailComposerService.SendRequirementUpdateEmail(newActivity.Id);
            #endif
        }

        private void AddRequirementActivity(RequirementActivity newActivity, bool dontOverrideDate)
        {
            newActivity.CreatedOn = newActivity.CreatedOn;
            newActivity.CreatedByUserId = WebUser.Id;

            _requirementActivityRepository.Create(newActivity);
            _unitOfWork.Commit();

            // Send Email, Email Template name is hard corded - Need to change later
            // Replace the hard coded emails with settings or a team.
            #if !DEBUG
                _emailComposerService.SendRequirementUpdateEmail(newActivity.Id);
            #endif
        }

        #region Ajax Calls
        [HttpGet]
        public string GetAllActivitiesForRequirement(int id)
        {
            var activities = _requirementActivityRepository.GetAllBy(r => r.RequirementId == id, o => o.OrderByDescending(r => r.CreatedOn)).ToList();
            var list = JsonConvert.SerializeObject(activities, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return list;
        }

        [HttpPost]
        public JsonResult SubmitForTechnicalReview(ChangeRequirementViewModel vm)
        {
            var selectedRequirement = _requirementRepository.Get(vm.RequirementId);
            if (selectedRequirement != null)
            {
                selectedRequirement.RequirementStatus = RequirementStatus.NeedTechnicalReview;
                selectedRequirement.UpdatedByUserId = WebUser.Id;

                // Add it as an Activity
                var newActivity = new RequirementActivity
                {
                    Title = "Needs Technical Review",
                    Comment = vm.Comment,
                    RequirementId = selectedRequirement.Id
                };

                AddRequirementActivity(newActivity);

                _requirementRepository.Update(selectedRequirement);
                _unitOfWork.Commit();

                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult AddNote(ChangeRequirementViewModel vm)
        {
            var selectedRequirement = _requirementRepository.Get(vm.RequirementId);
            if (selectedRequirement != null)
            {
                // Add it as an Activity
                var newActivity = new RequirementActivity
                {
                    Title = vm.Title,
                    Comment = vm.Comment,
                    RequirementId = selectedRequirement.Id,
                    CreatedOn = vm.CreatedOn
                };

                AddRequirementActivity(newActivity, true);

                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult SubmitTechnicalReviewContent(ChangeRequirementViewModel vm)
        {
            var selectedRequirement = _requirementRepository.Get(vm.RequirementId);
            if (selectedRequirement != null)
            {
                selectedRequirement.RequirementStatus = RequirementStatus.TechnicalReviewCompleted;
                selectedRequirement.UpdatedByUserId = WebUser.Id;

                // Add it as an Activity
                var newActivity = new RequirementActivity
                {
                    Title = "Technical Review Submitted",
                    Comment = vm.Comment,
                    RequirementId = selectedRequirement.Id
                };

                AddRequirementActivity(newActivity);

                _requirementRepository.Update(selectedRequirement);
                _unitOfWork.Commit();

                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult SubmitProposal(ChangeRequirementViewModel vm)
        {
            var selectedRequirement = _requirementRepository.Get(vm.RequirementId);
            if (selectedRequirement != null)
            {
                selectedRequirement.RequirementStatus = RequirementStatus.Proposed;
                selectedRequirement.RespondedOn = DateTime.UtcNow;
                selectedRequirement.UpdatedByUserId = WebUser.Id;

                // Add it as an Activity
                var newActivity = new RequirementActivity
                {
                    Title = "Proposed",
                    Comment = vm.Comment,
                    RequirementId = selectedRequirement.Id
                };

                AddRequirementActivity(newActivity);

                _requirementRepository.Update(selectedRequirement);
                _unitOfWork.Commit();

                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult ChangeRequirementStatus(ChangeRequirementStatusViewModel vm)
        {
            var selectedRequirement = _requirementRepository.Get(vm.RequirementId);
            if (selectedRequirement != null)
            {
                selectedRequirement.RequirementStatus = vm.Status;
                selectedRequirement.UpdatedByUserId = WebUser.Id;

                // Add it as an Activity
                var newActivity = new RequirementActivity
                {
                    Title = vm.Status.ToString(),
                    Comment = vm.Comment,
                    RequirementId = selectedRequirement.Id
                };

                AddRequirementActivity(newActivity);

                _requirementRepository.Update(selectedRequirement);
                _unitOfWork.Commit();

                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteRequirementActivity(int id)
        {
            _requirementActivityRepository.Delete(id);
            _unitOfWork.Commit();

            return Json(true);
        }
        #endregion

        [SelectList("LoggedInUser", "AssignedToUserId")]
        [SelectList("RequirementCategory", "CategoryId")]
        [SelectList("CRMLeadSource", "SourceId")]
        public ActionResult Index(RequirementSearchViewModel vm)
        {
            Func<IQueryable<Requirement>, IQueryable<Requirement>> requirementFilter = q =>
            {
                q = q.Include(r => r.Category).Include(r => r.Source);

                if (vm.AssignedToUserId.HasValue)
                {
                    q = q.Where(r => r.AssignedToUserId == vm.AssignedToUserId.Value);
                }

                if (vm.SourceId.HasValue)
                {
                    q = q.Where(r => r.SourceId == vm.SourceId.Value);
                }

                if (vm.CategoryId.HasValue)
                {
                    q = q.Where(r => r.CategoryId == vm.CategoryId.Value);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.RequirementStatus == vm.Status.Value);
                }

                if (!string.IsNullOrEmpty(vm.Title))
                {
                    q = q.Where(r => r.Title.Contains(vm.Title));
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn <= vm.EndDate.Value);
                }

                return q;
            };

            vm.Requirements = _requirementRepository.SearchPage(requirementFilter, o => o.OrderByDescending(c => c.CreatedOn), vm.GetPageNo(), vm.PageSize);

            return View(vm);
        }

        [SelectList("LoggedInUser", "AssignedToUserId")]
        [SelectList("RequirementCategory", "CategoryId")]
        [SelectList("CRMLeadSource", "SourceId")]
        public ActionResult Download(RequirementSearchViewModel vm)
        {
            Func<IQueryable<Requirement>, IQueryable<Requirement>> requirementFilter = q =>
            {
                q = q.Include(r => r.Category).Include(r => r.Source);

                if (vm.AssignedToUserId.HasValue)
                {
                    q = q.Where(r => r.AssignedToUserId == vm.AssignedToUserId.Value);
                }

                if (vm.SourceId.HasValue)
                {
                    q = q.Where(r => r.SourceId == vm.SourceId.Value);
                }

                if (vm.CategoryId.HasValue)
                {
                    q = q.Where(r => r.CategoryId == vm.CategoryId.Value);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.RequirementStatus == vm.Status.Value);
                }

                if (!string.IsNullOrEmpty(vm.Title))
                {
                    q = q.Where(r => r.Title.Contains(vm.Title));
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn <= vm.EndDate.Value);
                }

                q = q.OrderByDescending(c => c.CreatedOn);

                return q;
            };

            var requirements = _requirementRepository.Search(requirementFilter).ToList();
            return ExportAsCSV(requirements);
        }

        private ActionResult ExportAsCSV(IEnumerable<Requirement> requirements)
        {
            var sw = new StringBuilder();
            //write the header
            sw.AppendLine("Id,Title,Source,Category,Billing Type,Budget,Status,Posted On,Responded On,Description");

            foreach (var record in requirements)
            {
                sw.AppendLine($"{record.Id},{record.Title.RemoveComma()},{record.Source.Title},{record.Category.Title.RemoveComma()},{record.BillingType},{record.Budget},{record.RequirementStatus},{record.PostedOn},{record.RespondedOn},{record.Description.RemoveComma()}");
            }

            return File(new UTF8Encoding().GetBytes(sw.ToString()), "text/csv", "RequirementExport.csv");
        }

        public ActionResult Details(int id)
        {
            var requirement = _requirementRepository.GetBy(r => r.Id == id, "Contact.Person,AssignedToUser.Person,CreatedByUser.Person");
            var technologies = _requirementTechnologyMapRepository.GetAllBy(m => m.RequirementId == requirement.Id, "Technology").Select(m => m.Technology).ToList();
            var docs = _requirementDocumentRepository.GetAllBy(d => d.RequirementId == requirement.Id);
            var vm = new RequirementDetailsViewModel(requirement)
            {
                RequirementDocuments = docs.ToList(),
                Technologies = technologies
            };
            return View(vm);
        }

        [SelectList("LoggedInUser", "AssignedToUserId")]
        [SelectList("RequirementCategory", "CategoryId")]
        [SelectList("CRMLeadSource", "SourceId")]
        [SelectList("CRMContact", "ContactId")]
        [MultiSelectList("Technology", "Technologies")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewRequirementViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var requirement = new Requirement
                {
                    AssignedToUserId = vm.AssignedToUserId,
                    ContactId = vm.ContactId,
                    SourceId = vm.SourceId,
                    CategoryId = vm.CategoryId,
                    Title = vm.Title,
                    Description = vm.Description,
                    Url = vm.Url,
                    BillingType = vm.BillingType,
                    Budget = vm.Budget,
                    PostedOn = vm.PostedOn.ToUniversalTime(),
                    RequirementStatus = RequirementStatus.New,
                    CreatedByUserId = WebUser.Id
                };


                _requirementRepository.Create(requirement);
                _unitOfWork.Commit();

                // Map the Technologies
                if (vm.TechnologyIds != null)
                {
                    foreach (var technologyId in vm.TechnologyIds)
                    {
                        var newMap = new RequirementTechnologyMap
                        {
                            RequirementId = requirement.Id,
                            TechnologyId = technologyId
                        };

                        _requirementTechnologyMapRepository.Create(newMap);
                    }

                    _unitOfWork.Commit();
                }
                
                // Add the activities
                var postedActivity = new RequirementActivity
                {
                    Title = "Posted in Source",
                    Comment = "Requirement was posted",
                    RequirementId = requirement.Id,
                    CreatedOn = requirement.PostedOn.ToUniversalTime(),
                    CreatedByUserId = WebUser.Id
                };

                _requirementActivityRepository.Create(postedActivity);

                var createdActivity = new RequirementActivity
                {
                    Title = "Created",
                    Comment = WebUser.Name + " created the requirement in Grid",
                    RequirementId = requirement.Id,
                    CreatedByUserId = WebUser.Id
                };

                _requirementActivityRepository.Create(createdActivity);

                _unitOfWork.Commit();


                // Send Email, Email Template name is hard corded - Need to change later
                // Replace the hard coded emails with settings or a team.
                #if !DEBUG
                    _emailComposerService.SendNewRequirementEmail(requirement.Id);
                #endif

                return RedirectToAction("Details", "Requirements", new { id = requirement.Id });
            }

            ViewBag.Technologies = new MultiSelectList(_technologyRepository.GetAll(), "Id", "Title", vm.TechnologyIds);
            ViewBag.AssignedToUserId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", vm.AssignedToUserId);
            ViewBag.ContactId = new SelectList(_crmContactRepository.GetAll("Person"), "Id", "Person.Name", vm.ContactId);
            ViewBag.CategoryId = new SelectList(_requirementCategoryRepository.GetAll(), "Id", "Title", vm.CategoryId);
            ViewBag.SourceId = new SelectList(_crmLeadSourceRepository.GetAll(), "Id", "Title", vm.SourceId);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActivity(RequirementActivityViewModel activity)
        {
            if (ModelState.IsValid)
            {
                var newActivity = new RequirementActivity
                {
                    Comment = activity.NewComment,
                    RequirementId = activity.Id,
                    CreatedByUserId = WebUser.Id
                };


                _requirementActivityRepository.Create(newActivity);
                _unitOfWork.Commit();
            }

            return RedirectToAction("Details", new { id = activity.Id });
        }

        public ActionResult Edit(int id)
        {
            var requirement = _requirementRepository.Get(id);

            if (requirement == null)
            {
                return HttpNotFound();
            }

            var mappedTechnologies = _requirementTechnologyMapRepository.GetAllBy(m => m.RequirementId == requirement.Id).Select(m => m.TechnologyId).ToList();

            ViewBag.Technologies = new MultiSelectList(_technologyRepository.GetAll(), "Id", "Title", mappedTechnologies);
            ViewBag.AssignedToUserId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", requirement.AssignedToUserId);
            ViewBag.ContactId = new SelectList(_crmContactRepository.GetAll("Person"), "Id", "Person.Name", requirement.ContactId);
            ViewBag.CategoryId = new SelectList(_requirementCategoryRepository.GetAll(), "Id", "Title", requirement.CategoryId);
            ViewBag.SourceId = new SelectList(_crmLeadSourceRepository.GetAll(), "Id", "Title", requirement.SourceId);

            var vm = new EditRequirementViewModel(requirement);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditRequirementViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var selectedRequirement = _requirementRepository.Get(vm.Id);

                if (selectedRequirement != null)
                {
                    selectedRequirement.AssignedToUserId = vm.AssignedToUserId;
                    selectedRequirement.ContactId = vm.ContactId;
                    selectedRequirement.SourceId = vm.SourceId;
                    selectedRequirement.CategoryId = vm.CategoryId;
                    selectedRequirement.Title = vm.Title;
                    selectedRequirement.Description = vm.Description;
                    selectedRequirement.Url = vm.Url;
                    selectedRequirement.BillingType = vm.BillingType;
                    selectedRequirement.Budget = vm.Budget;
                    selectedRequirement.PostedOn = vm.PostedOn;

                    selectedRequirement.UpdatedByUserId = WebUser.Id;

                    _requirementRepository.Update(selectedRequirement);
                    _unitOfWork.Commit();

                    // Remove the existing mapped Technologies 
                    var existingMaps = _requirementTechnologyMapRepository.GetAllBy(m => m.RequirementId == selectedRequirement.Id);
                    foreach (var map in existingMaps)
                    {
                        _requirementTechnologyMapRepository.Delete(map);
                    }

                    _unitOfWork.Commit();

                    // Map the New Technologies
                    if (vm.TechnologyIds != null)
                    {
                        foreach (var technologyId in vm.TechnologyIds)
                        {
                            var newMap = new RequirementTechnologyMap
                            {
                                RequirementId = vm.Id,
                                TechnologyId = technologyId
                            };
                            _requirementTechnologyMapRepository.Create(newMap);
                        }

                        _unitOfWork.Commit();
                    }
                    
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Technologies = new MultiSelectList(_technologyRepository.GetAll(), "Id", "Title", vm.TechnologyIds);
            ViewBag.AssignedToUserId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", vm.AssignedToUserId);
            ViewBag.ContactId = new SelectList(_crmContactRepository.GetAll("Person"), "Id", "Person.Name", vm.ContactId);
            ViewBag.CategoryId = new SelectList(_requirementCategoryRepository.GetAll(), "Id", "Title", vm.CategoryId);
            ViewBag.SourceId = new SelectList(_crmLeadSourceRepository.GetAll(), "Id", "Title", vm.SourceId);

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var requirement = _requirementRepository.Get(id);

            if (requirement == null)
            {
                return HttpNotFound();
            }

            return View(requirement);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var operationResult = _requirementService.Delete(id);
            if (operationResult.Status)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, operationResult.Message);
            return View();
        }
    }
}
