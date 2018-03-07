using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Api.Controllers
{
    public class LeadStatusController : GridApiBaseController
    {
        private readonly ICRMLeadStatusRepository _crmLeadStatusRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LeadStatusController(ICRMLeadStatusRepository crmLeadStatusRepository,
                                   IUnitOfWork unitOfWork)
        {
            _crmLeadStatusRepository = crmLeadStatusRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _crmLeadStatusRepository.GetAll(), "Lead Statuses Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _crmLeadStatusRepository.Get(id), "Lead Status fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(CRMLeadStatus vm)
        {
            ApiResult<CRMLeadStatus> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var selectedLeadStatus = _crmLeadStatusRepository.Get(vm.Id);
                        selectedLeadStatus.Name = vm.Name;
                        selectedLeadStatus.UpdatedByUserId = WebUser.Id;
                        _crmLeadStatusRepository.Update(selectedLeadStatus);
                        _unitOfWork.Commit();
                        return selectedLeadStatus;
                    }, "Lead Status updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var newLeadStatus = new CRMLeadStatus
                        {
                            Name = vm.Name,
                            CreatedByUserId = WebUser.Id
                        };
                        _crmLeadStatusRepository.Create(newLeadStatus);
                        _unitOfWork.Commit();
                        return newLeadStatus;
                    }, "Lead Status created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<CRMLeadStatus>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _crmLeadStatusRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Lead Status deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
