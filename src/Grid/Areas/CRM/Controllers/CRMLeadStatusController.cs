using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.CRM.Controllers
{
    [GridPermission(PermissionCode = 220)]
    public class CRMLeadStatusController : CRMBaseController
    {
        private readonly ICRMLeadStatusRepository _crmLeadStatusRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CRMLeadStatusController(ICRMLeadStatusRepository crmLeadStatusRepository,
                                       IUnitOfWork unitOfWork)
        {
            _crmLeadStatusRepository = crmLeadStatusRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var crmLeadStatues = _crmLeadStatusRepository.GetAllBy(c => c.OrderByDescending(f => f.CreatedOn));
            return View(crmLeadStatues);
        }

        public ActionResult Details(int id)
        {
            var crmLeadStatus = _crmLeadStatusRepository.Get(id);
            return CheckForNullAndExecute(crmLeadStatus, () => View(crmLeadStatus));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CRMLeadStatus cRMLeadStatus)
        {
            if (ModelState.IsValid)
            {
                cRMLeadStatus.CreatedByUserId = WebUser.Id;

                _crmLeadStatusRepository.Create(cRMLeadStatus);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(cRMLeadStatus);
        }

        public ActionResult Edit(int id)
        {
            var crmLeadStatus = _crmLeadStatusRepository.Get(id);
            return CheckForNullAndExecute(crmLeadStatus, () => View(crmLeadStatus));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CRMLeadStatus cRMLeadStatus)
        {
            if (ModelState.IsValid)
            {
                var selectedLeadStatus = _crmLeadStatusRepository.Get(cRMLeadStatus.Id);

                if (selectedLeadStatus != null)
                {
                    selectedLeadStatus.Name = cRMLeadStatus.Name;
                    selectedLeadStatus.UpdatedByUserId = WebUser.Id;

                    _crmLeadStatusRepository.Update(cRMLeadStatus);
                    _unitOfWork.Commit();

                    return RedirectToAction("Index");
                }

            }
            return View(cRMLeadStatus);
        }

        public ActionResult Delete(int id)
        {
            var crmLeadStatus = _crmLeadStatusRepository.Get(id);
            return CheckForNullAndExecute(crmLeadStatus, () => View(crmLeadStatus));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _crmLeadStatusRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
