using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Models;
using Newtonsoft.Json;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Features.CRM.Services.Interfaces;
using Grid.Features.CRM.ViewModels;
using Grid.Features.CRM.ViewModels.CRMLead;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.CRM.Controllers
{
    [GridPermission(PermissionCode = 220)]
    public class CRMLeadsController : CRMBaseController
    {
        private readonly ICRMLeadRepository _crmLeadRepository;
        private readonly ICRMLeadSourceRepository _crmLeadSourceRepository;
        private readonly ICRMLeadStatusRepository _crmLeadStatusRepository;
        private readonly ICRMLeadCategoryRepository _crmLeadCategoryRepository;
        private readonly ICRMLeadActivityRepository _crmLeadActivityRepository;
        private readonly ICRMLeadTechnologyMapRepository _crmLeadTechnologyMapRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICRMLeadService _crmLeadService;

        public CRMLeadsController(ICRMLeadRepository crmLeadRepository,
                                  ICRMLeadStatusRepository crmLeadStatusRepository,
                                  ICRMLeadSourceRepository crmLeadSourceRepository,
                                  ICRMLeadCategoryRepository crmLeadCategoryRepository,
                                  ICRMLeadActivityRepository crmLeadActivityRepository,
                                  ICRMLeadTechnologyMapRepository crmLeadTechnologyMapRepository,
                                  IUserRepository userRepository,
                                  ITechnologyRepository technologyRepository,
                                  IUnitOfWork unitOfWork,
                                  ICRMLeadService crmLeadService)
        {
            _crmLeadRepository = crmLeadRepository;
            _crmLeadSourceRepository = crmLeadSourceRepository;
            _crmLeadStatusRepository = crmLeadStatusRepository;
            _crmLeadCategoryRepository = crmLeadCategoryRepository;
            _crmLeadActivityRepository = crmLeadActivityRepository;
            _crmLeadTechnologyMapRepository = crmLeadTechnologyMapRepository;
            _userRepository = userRepository;
            _technologyRepository = technologyRepository;
            _unitOfWork = unitOfWork;
            _crmLeadService = crmLeadService;
        }

        #region Ajax Calls
        [HttpGet]
        public string GetAllActivitiesForCRMLead(int id)
        {
            var activities = _crmLeadActivityRepository.GetAllBy(r => r.CRMLeadId == id, o => o.OrderByDescending(r => r.CreatedOn)).ToList();
            var list = JsonConvert.SerializeObject(activities, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return list;
        }

        
        [HttpPost]
        public JsonResult AddNote(CRMActivityViewModel vm)
        {
            var selectedLead = _crmLeadRepository.Get(vm.CRMLeadId);
            if (selectedLead != null)
            {
                // Add it as an Activity
                var newActivity = new CRMLeadActivity
                {
                    Title = vm.Title,
                    Comment = vm.Comment,
                    CRMLeadId = selectedLead.Id,
                    CreatedByUserId = WebUser.Id
                };

                _crmLeadActivityRepository.Create(newActivity);
                _unitOfWork.Commit();

                return Json(true);
            }
            
            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteNote(int id)
        {
            _crmLeadActivityRepository.Delete(id);
            _unitOfWork.Commit();

            return Json(true);
        }

        [HttpPost]
        public JsonResult ChangeCRMLeadStatus(CRMActivityViewModel vm)
        {
            var selectedCRMLead = _crmLeadRepository.Get(vm.CRMLeadId);
            if (selectedCRMLead != null)
            {
                selectedCRMLead.LeadStatusId = vm.StatusId.GetValueOrDefault();
                selectedCRMLead.UpdatedByUserId = WebUser.Id;

                // Send Email Here to Tech Team

                var selectedStatus = _crmLeadStatusRepository.Get(vm.StatusId.GetValueOrDefault());
                // Add it as an Activity
                if (selectedStatus != null)
                {
                    var newActivity = new CRMLeadActivity
                    {
                        Title = selectedStatus.Name,
                        Comment = vm.Comment,
                        CRMLeadId = selectedCRMLead.Id,
                        CreatedByUserId = WebUser.Id
                    };

                    _crmLeadActivityRepository.Create(newActivity);
                }

                _crmLeadRepository.Update(selectedCRMLead);
                _unitOfWork.Commit();
                return Json(true);
            }
            
            return Json(false);
        }

        private string GetCRMLeadDetails(CRMLead crmLead)
        {
            var builder = new StringBuilder();
            builder.Append("<p>Already a Lead exists with the same details</p>");
            builder.Append($"<p>Name is: {crmLead.Person.Name}</p>");
            builder.Append($"<p>Email is: {crmLead.Person.Email}</p>");
            builder.Append($"<p>Secondary Email is: {crmLead.Person.SecondaryEmail}</p>");
            builder.Append($"<p>Phone No is: {crmLead.Person.PhoneNo}</p>");
            builder.Append($"<p>Office Phone No is: {crmLead.Person.OfficePhone}</p>");
            builder.Append($"<a target='_blank' href='/CRM/CRMLeads/Details/{crmLead.Id}'>View Details</a>");
            return builder.ToString();
        }

        [HttpPost]
        public JsonResult ValidateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var selectedCRMLead = _crmLeadRepository.GetBy(c => c.Person.Email == email || c.Person.SecondaryEmail == email, "Person");
                if (selectedCRMLead != null)
                {
                    return Json(new RemoteValidationResult
                    {
                        Status = false,
                        Message = GetCRMLeadDetails(selectedCRMLead)
                    });
                }

                return Json(new RemoteValidationResult
                {
                    Status = true,
                    Message = "No Lead Found"
                });
            }

            return Json(new RemoteValidationResult
            {
                Status = true,
                Message = "No Lead Found"
            });
        }

        [HttpPost]
        public JsonResult ValidatePhoneNo(string phoneNo)
        {
            if (!string.IsNullOrEmpty(phoneNo))
            {
                var selectedCRMLead = _crmLeadRepository.GetBy(c => c.Person.PhoneNo == phoneNo || c.Person.OfficePhone == phoneNo, "Person");
                if (selectedCRMLead != null)
                {
                    return Json(new RemoteValidationResult
                    {
                        Status = false,
                        Message = GetCRMLeadDetails(selectedCRMLead)
                    });
                }

                return Json(new RemoteValidationResult
                {
                    Status = true,
                    Message = "No Lead Found"
                });
            }

            return Json(new RemoteValidationResult
            {
                Status = true,
                Message = "No Lead Found"
            });
        }

        #endregion

        private bool DoIHaveCRMManageAccess()
        {
            return WebUser.Permissions.Contains(222)|| WebUser.IsAdmin;
        }

        [SelectList("User", "AssignedToUserId")]
        [SelectList("CRMLeadSource", "LeadSourceId")]
        [SelectList("CRMLeadCategory", "CategoryId")]
        [SelectList("CRMLeadStatus", "LeadStatusId")]
        public ActionResult Index(CRMLeadSearchViewModel vm)
        {
            ViewBag.HasCRMManageAccess = DoIHaveCRMManageAccess();

            Func<IQueryable<CRMLead>, IQueryable<CRMLead>> crmLeadFilter = (q) =>
            {
                q = q.Include("AssignedToUser.Person")
                                   .Include(c => c.LeadSource)
                                   .Include(c => c.LeadSourceUser)
                                   .Include(c => c.LeadStatus)
                                   .Include(c => c.Person);

                if (vm.LeadSourceId.HasValue)
                {
                    q = q.Where(r => r.LeadSourceId == vm.LeadSourceId.Value);
                }

                if (vm.CategoryId.HasValue)
                {
                    q = q.Where(r => r.CategoryId == vm.CategoryId.Value);
                }

                if (vm.AssignedToUserId.HasValue)
                {
                    q = q.Where(r => r.AssignedToUserId == vm.AssignedToUserId.Value);
                }

                if (vm.LeadStatusId.HasValue)
                {
                    q = q.Where(r => r.LeadStatusId == vm.LeadStatusId.Value);
                }

                if (!string.IsNullOrEmpty(vm.FirstName))
                {
                    q = q.Where(r => r.Person.FirstName.Contains(vm.FirstName));
                }

                if (!string.IsNullOrEmpty(vm.LastName))
                {
                    q = q.Where(r => r.Person.LastName.Contains(vm.LastName));
                }

                if (!string.IsNullOrEmpty(vm.City))
                {
                    q = q.Where(r => r.Person.City.Contains(vm.City));
                }

                if (!string.IsNullOrEmpty(vm.Designation))
                {
                    q = q.Where(r => r.Person.Designation.Contains(vm.Designation));
                }

                // If you don't have manage Access, restrict to assigned to yours only
                if (!DoIHaveCRMManageAccess())
                {
                    q = q.Where(l => l.AssignedToUserId == WebUser.Id);
                }

                return q;
            };

            vm.Leads = _crmLeadRepository.SearchPage(crmLeadFilter, o => o.OrderByDescending(c => c.CreatedOn), vm.GetPageNo(), vm.PageSize);

            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var crmLead = _crmLeadRepository.Get(id, "AssignedToUser.Person");

            if (crmLead == null)
            {
                return HttpNotFound();
            }

            // Check whether i have access to this Lead details 
            var hasAccess = crmLead.AssignedToUserId == WebUser.Id || DoIHaveCRMManageAccess();
            if (!hasAccess)
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            ViewBag.HasCRMManageAccess = DoIHaveCRMManageAccess();

            var technologies = _crmLeadTechnologyMapRepository.GetAllBy(r => r.LeadId == crmLead.Id, "Technology").Select(t => t.Technology).ToList();
            var crmLeadStatuses = _crmLeadStatusRepository.GetAll();
            ViewBag.LeadStatusId = new SelectList(crmLeadStatuses, "Id", "Name", crmLead.LeadStatusId);

            var vm = new CRMLeadDetailsViewModel(crmLead)
            {
                Technologies = technologies
            };

            return View(vm);
        }

        [SelectList("LoggedInUser", "AssignedToUserId")]
        [MultiSelectList("Technology", "Technologies")]
        [SelectList("CRMLeadSource", "LeadSourceId")]
        [SelectList("CRMLeadCategory", "CategoryId")]
        [SelectList("CRMLeadStatus", "LeadStatusId")]
        [SelectList("User", "LeadSourceUserId")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("LoggedInUser", "AssignedToUserId", SelectListState.Recreate)]
        [MultiSelectList("Technology", "Technologies", MultiSelectListState.Recreate)]
        [SelectList("CRMLeadSource", "LeadSourceId", SelectListState.Recreate)]
        [SelectList("CRMLeadCategory", "CategoryId", SelectListState.Recreate)]
        [SelectList("CRMLeadStatus", "LeadStatusId", SelectListState.Recreate)]
        [SelectList("User", "LeadSourceUserId", SelectListState.Recreate)]
        public ActionResult Create(NewCRMLeadViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newCRMLead = new CRMLead
                    {
                        AssignedToUserId = vm.AssignedToUserId,
                        LeadSourceId = vm.LeadSourceId,
                        CategoryId = vm.CategoryId,
                        LeadStatusId = vm.LeadStatusId,
                        Person = vm.Person,
                        Expertise = vm.Expertise,
                        Description = vm.Description,
                        RecievedOn = vm.RecievedOn,
                        CreatedByUserId = WebUser.Id
                    };

                    _crmLeadRepository.Create(newCRMLead);
                    _unitOfWork.Commit();

                    // Map the Technologies
                    if (vm.TechnologyIds != null)
                    {
                        foreach (var technologyId in vm.TechnologyIds)
                        {
                            var newMap = new CRMLeadTechnologyMap
                            {
                                LeadId = newCRMLead.Id,
                                TechnologyId = technologyId
                            };

                            _crmLeadTechnologyMapRepository.Create(newMap);
                        }

                        _unitOfWork.Commit();
                    }
                    
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please fill all mandatory fields");
            }

            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var cRMLead = _crmLeadRepository.Get(id);

            if (cRMLead == null)
            {
                return HttpNotFound();
            }

            // Check whether i have access to this Lead details 
            var hasAccess = cRMLead.AssignedToUserId == WebUser.Id || DoIHaveCRMManageAccess();
            if (!hasAccess)
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            var mappedTechnologies = _crmLeadTechnologyMapRepository.GetAllBy(m => m.LeadId == cRMLead.Id).Select(m => m.TechnologyId).ToList();

            ViewBag.Technologies = new MultiSelectList(_technologyRepository.GetAll(), "Id", "Title", mappedTechnologies);
            ViewBag.AssignedToUserId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", cRMLead.AssignedToUserId);
            ViewBag.LeadSourceId = new SelectList(_crmLeadSourceRepository.GetAll(), "Id", "Title", cRMLead.LeadSourceId);
            ViewBag.CategoryId = new SelectList(_crmLeadCategoryRepository.GetAll(), "Id", "Title", cRMLead.CategoryId);
            ViewBag.LeadSourceUserId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", cRMLead.LeadSourceUserId);
            ViewBag.LeadStatusId = new SelectList(_crmLeadStatusRepository.GetAll(), "Id", "Name", cRMLead.LeadStatusId);

            var vm = new EditCRMLeadViewModel(cRMLead);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("LoggedInUser", "AssignedToUserId", SelectListState.Recreate)]
        [MultiSelectList("Technology", "Technologies", MultiSelectListState.Recreate)]
        [SelectList("CRMLeadSource", "LeadSourceId", SelectListState.Recreate)]
        [SelectList("CRMLeadCategory", "CategoryId", SelectListState.Recreate)]
        [SelectList("CRMLeadStatus", "LeadStatusId", SelectListState.Recreate)]
        [SelectList("User", "LeadSourceUserId", SelectListState.Recreate)]
        public ActionResult Edit(EditCRMLeadViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var selectedLead = _crmLeadRepository.GetBy(l => l.Id == vm.Id);

                if (selectedLead != null)
                {
                    // Check whether i have access to this Lead details 
                    var hasAccess = selectedLead.AssignedToUserId == WebUser.Id || DoIHaveCRMManageAccess();
                    if (!hasAccess)
                    {
                        return RedirectToAction("NotAuthorized", "Error", new { area = "" });
                    }

                    selectedLead.AssignedToUserId = vm.AssignedToUserId;
                    selectedLead.LeadSourceId = vm.LeadSourceId;
                    selectedLead.LeadSourceUserId = vm.LeadSourceUserId;
                    selectedLead.CategoryId = vm.CategoryId;
                    
                    selectedLead.Person.FirstName = vm.Person.FirstName;
                    selectedLead.Person.LastName = vm.Person.LastName;
                    selectedLead.Person.Gender = vm.Person.Gender;
                    selectedLead.Person.Email = vm.Person.Email;
                    selectedLead.Person.Organization = vm.Person.Organization;
                    selectedLead.Person.Designation = vm.Person.Designation;
                    selectedLead.Person.PhoneNo = vm.Person.PhoneNo;
                    selectedLead.Person.SecondaryEmail = vm.Person.SecondaryEmail;
                    selectedLead.Person.OfficePhone = vm.Person.OfficePhone;
                    selectedLead.Person.Website = vm.Person.Website;
                    selectedLead.Person.Skype = vm.Person.Skype;
                    selectedLead.Person.Facebook = vm.Person.Facebook;
                    selectedLead.Person.Twitter = vm.Person.Twitter;
                    selectedLead.Person.GooglePlus = vm.Person.GooglePlus;
                    selectedLead.Person.LinkedIn = vm.Person.LinkedIn;
                    selectedLead.Person.City = vm.Person.City;
                    selectedLead.Person.Country = vm.Person.Country;
                    selectedLead.Person.Address = vm.Person.Address;
                    selectedLead.Person.CommunicationAddress = vm.Person.CommunicationAddress;
                    selectedLead.Person.DateOfBirth = vm.Person.DateOfBirth;

                    selectedLead.Expertise = vm.Expertise;
                    selectedLead.Description = vm.Description;
                    selectedLead.RecievedOn = vm.RecievedOn;
                    
                    selectedLead.UpdatedByUserId = WebUser.Id;

                    _crmLeadRepository.Update(selectedLead);
                    _unitOfWork.Commit();


                    // Remove the existing mapped Technologies 
                    var existingMaps = _crmLeadTechnologyMapRepository.GetAllBy(m => m.LeadId == selectedLead.Id);
                    foreach (var map in existingMaps)
                    {
                        _crmLeadTechnologyMapRepository.Delete(map);
                    }

                    _unitOfWork.Commit();

                    if (vm.TechnologyIds != null)
                    {
                        // Map the New Technologies
                        foreach (var technologyId in vm.TechnologyIds)
                        {
                            var newMap = new CRMLeadTechnologyMap
                            {
                                LeadId = vm.Id,
                                TechnologyId = technologyId
                            };

                            _crmLeadTechnologyMapRepository.Create(newMap);
                        }
                        
                        _unitOfWork.Commit();
                    }
                    
                    return RedirectToAction("Index");
                }
            }

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            // Check whether i have access to this Lead details 
            var hasAccess = DoIHaveCRMManageAccess();
            if (!hasAccess)
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            var crmLead = _crmLeadRepository.Get(id);
            return CheckForNullAndExecute(crmLead, () => View(crmLead));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Check whether i have access to this Lead details 
            var hasAccess = DoIHaveCRMManageAccess();
            if (!hasAccess)
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            var status = _crmLeadService.Delete(id);
            if (status)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
