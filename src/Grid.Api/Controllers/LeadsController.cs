using System;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;
using Grid.Infrastructure.Filters;

namespace Grid.Api.Controllers
{
    [APIIdentityInjector]
    public class LeadsController : GridApiBaseController
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICRMLeadRepository _crmLeadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LeadsController(IPersonRepository personRepository,
                               ICRMLeadRepository crmLeadRepository,
                               IUnitOfWork unitOfWork)
        {
            _personRepository = personRepository;
            _crmLeadRepository = crmLeadRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// This is used by the Logiticks Front End website, So we can remove it only after migration.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult Create(LeadModel vm)
        {
            try
            {
                var newPerson = new Person
                {
                    Email = vm.Email,
                    PhoneNo = vm.Phone,
                    Country = vm.Country,
                    FirstName = !string.IsNullOrEmpty(vm.FirstName) ? vm.FirstName : "Unknown",
                    LastName = !string.IsNullOrEmpty(vm.LastName) ? vm.LastName : "Unknown"
                };

                _personRepository.Create(newPerson);

                var newCRMLead = new CRMLead
                {
                    Description = vm.Message,
                    AssignedToUserId = vm.CreatedByUserId,
                    LeadSourceId = vm.LeadSourceId,
                    CategoryId = vm.CategoryId,
                    LeadStatusId = vm.StatusId,
                    PersonId = newPerson.Id,
                    RecievedOn = DateTime.UtcNow,
                    CreatedByUserId = vm.CreatedByUserId
                };

                _crmLeadRepository.Create(newCRMLead);
                _unitOfWork.Commit();

                var result = new ApiResult<bool>
                {
                    Status = true,
                    Message = "Success"
                };

                return Json(result);

            }
            catch (Exception ex)
            {
                var result = new ApiResult<bool>
                {
                    Status = false,
                    Message = ex.Message
                };

                return Json(result);
            }
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _crmLeadRepository.GetAll(), "Leads Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _crmLeadRepository.Get(id), "Lead fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(CRMLead crmLead)
        {
            ApiResult<CRMLead> apiResult;

            if (ModelState.IsValid)
            {
                if (crmLead.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _crmLeadRepository.Update(crmLead);
                        _unitOfWork.Commit();
                        return crmLead;
                    }, "Lead updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _crmLeadRepository.Create(crmLead);
                        _unitOfWork.Commit();
                        return crmLead;
                    }, "Lead created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<CRMLead>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _crmLeadRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Lead deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}