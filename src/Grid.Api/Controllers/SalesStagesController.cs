using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Api.Controllers
{
    public class SalesStagesController : GridApiBaseController
    {
        private readonly ICRMSalesStageRepository _crmSalesStageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SalesStagesController(ICRMSalesStageRepository crmSalesStageRepository,
                                   IUnitOfWork unitOfWork)
        {
            _crmSalesStageRepository = crmSalesStageRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _crmSalesStageRepository.GetAll(), "Sales Stages Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _crmSalesStageRepository.Get(id), "Sales Stage fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(CRMSalesStage salesStage)
        {
            ApiResult<CRMSalesStage> apiResult;

            if (ModelState.IsValid)
            {
                if (salesStage.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {

                        var selectedSalesStage = _crmSalesStageRepository.Get(salesStage.Id);
                        selectedSalesStage.Name = salesStage.Name;
                        selectedSalesStage.Status = salesStage.Status;
                        selectedSalesStage.UpdatedByUserId = WebUser.Id;                      
                        _crmSalesStageRepository.Update(selectedSalesStage);
                        _unitOfWork.Commit();
                        return selectedSalesStage;
                    }, "Lead Source Period updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var newSalesStage = new CRMSalesStage
                        {
                            Name = salesStage.Name,
                            Status = salesStage.Status,
                            CreatedByUserId = WebUser.Id
                        };
                        _crmSalesStageRepository.Create(newSalesStage);
                        _unitOfWork.Commit();
                        return newSalesStage;
                    }, "Lead Source Period created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<CRMSalesStage>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _crmSalesStageRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Sales Stage deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
