using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.CRM.Controllers
{
    [GridPermission(PermissionCode = 220)]
    public class CRMSalesStagesController : CRMBaseController
    {
        private readonly ICRMSalesStageRepository _crmSalesSalesStageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CRMSalesStagesController(ICRMSalesStageRepository crmSalesSalesStageRepository,
                                        IUnitOfWork unitOfWork)
        {
            _crmSalesSalesStageRepository = crmSalesSalesStageRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var salestages = _crmSalesSalesStageRepository.GetAllBy(c => c.OrderByDescending(d => d.CreatedOn));
            return View(salestages);
        }

        public ActionResult Details(int id)
        {
            var crmSalesStage = _crmSalesSalesStageRepository.Get(id);
            return CheckForNullAndExecute(crmSalesStage, () => View(crmSalesStage));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CRMSalesStage cRMSalesStage)
        {
            if (ModelState.IsValid)
            {
                cRMSalesStage.CreatedByUserId = WebUser.Id;

                _crmSalesSalesStageRepository.Create(cRMSalesStage);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(cRMSalesStage);
        }

        public ActionResult Edit(int id)
        {
            var crmSalesStage = _crmSalesSalesStageRepository.Get(id);
            return CheckForNullAndExecute(crmSalesStage, () => View(crmSalesStage));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CRMSalesStage cRMSalesStage)
        {
            if (ModelState.IsValid)
            {
                var selectedSaleStage = _crmSalesSalesStageRepository.Get(cRMSalesStage.Id);

                if (selectedSaleStage != null)
                {
                    selectedSaleStage.Name = cRMSalesStage.Name;
                    selectedSaleStage.Status = cRMSalesStage.Status;
                    selectedSaleStage.UpdatedByUserId = WebUser.Id;

                    _crmSalesSalesStageRepository.Update(cRMSalesStage);
                    _unitOfWork.Commit();

                    return RedirectToAction("Index");
                }
            }
            return View(cRMSalesStage);
        }

        public ActionResult Delete(int id)
        {
            var crmSalesStage = _crmSalesSalesStageRepository.Get(id);
            return CheckForNullAndExecute(crmSalesStage, () => View(crmSalesStage));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _crmSalesSalesStageRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
