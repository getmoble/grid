using System;
using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Api.Models.RMS;
using Grid.Features.Common;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Entities;

namespace Grid.Api.Controllers
{
    public class RequirementsController : GridApiBaseController
    {
        private readonly IRequirementRepository _requirementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequirementsController(IRequirementRepository requirementRepository,
                                   IUnitOfWork unitOfWork)
        {
            _requirementRepository = requirementRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _requirementRepository.GetAll().Select(r => new RequirementModel(r)).ToList();
            }, "Requirements Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() =>
            {
                var selectedRequirement = _requirementRepository.Get(id);
                return new RequirementModel(selectedRequirement);
            }, "Requirement fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(RequirementUpdateModel vm)
        {
            ApiResult<Requirement> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var selectedRequirement = _requirementRepository.Get(vm.Id);
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
                        selectedRequirement.RespondedOn = vm.RespondedOn;

                        selectedRequirement.UpdatedByUserId = WebUser.Id;
                        selectedRequirement.UpdatedOn = DateTime.UtcNow;
                        _requirementRepository.Update(selectedRequirement);
                        _unitOfWork.Commit();
                        return selectedRequirement;
                    }, "Requirement updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var newRequirement = new Requirement
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
                            PostedOn = vm.PostedOn,
                            RespondedOn = vm.RespondedOn,
                            CreatedByUserId = WebUser.Id
                        };

                        _requirementRepository.Create(newRequirement);
                        _unitOfWork.Commit();
                        return newRequirement;
                    }, "Requirement created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Requirement>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _requirementRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Requirement deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
