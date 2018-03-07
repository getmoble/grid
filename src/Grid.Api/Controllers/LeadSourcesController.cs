using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Api.Controllers
{
    public class LeadSourcesController : GridApiBaseController
    {
        private readonly ICRMLeadSourceRepository _crmLeadSourceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LeadSourcesController(ICRMLeadSourceRepository crmLeadSourceRepository,
                                   IUnitOfWork unitOfWork)
        {
            _crmLeadSourceRepository = crmLeadSourceRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _crmLeadSourceRepository.GetAll(), "Lead Sources Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _crmLeadSourceRepository.Get(id), "Lead Source fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(CRMLeadSource vm)
        {
            ApiResult<CRMLeadSource> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var selectedLeadSource = _crmLeadSourceRepository.Get(vm.Id);
                        selectedLeadSource.Title = vm.Title;
                        selectedLeadSource.Description = vm.Description;
                        selectedLeadSource.UpdatedByUserId = WebUser.Id;
                        _crmLeadSourceRepository.Update(selectedLeadSource);
                        _unitOfWork.Commit();
                        return selectedLeadSource;
                    }, "Lead Source updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var newLeadSource = new CRMLeadSource
                        {
                            Title = vm.Title,
                            Description = vm.Description,
                            CreatedByUserId = WebUser.Id
                        };
                        _crmLeadSourceRepository.Create(newLeadSource);
                        _unitOfWork.Commit();
                        return newLeadSource;
                    }, "Lead Source created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<CRMLeadSource>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _crmLeadSourceRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Lead Source deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
