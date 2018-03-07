using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Payroll.DAL.Interfaces;
using Grid.Features.Payroll.Entities;
using Grid.Filters;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.Payroll.Controllers
{
    [GridPermission(PermissionCode = 210)]
    public class SalaryComponentsController : GridBaseController
    {
        private readonly ISalaryComponentRepository _salaryComponentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SalaryComponentsController(ISalaryComponentRepository salaryComponentRepository,
                                          IUnitOfWork unitOfWork)
        {
            _salaryComponentRepository = salaryComponentRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var salaryComponents = _salaryComponentRepository.GetAll("ParentComponent");
            return View(salaryComponents);
        }

        public ActionResult Details(int id)
        {
            var salaryComponent = _salaryComponentRepository.Get(id);
            return CheckForNullAndExecute(salaryComponent, () => View(salaryComponent));
        }

        [SelectList("SalaryComponent", "ParentComponentId")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("SalaryComponent", "ParentComponentId", SelectListState.Recreate)]
        public ActionResult Create(SalaryComponent salaryComponent)
        {
            if (ModelState.IsValid)
            {
                _salaryComponentRepository.Create(salaryComponent);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

           return View(salaryComponent);
        }

        public ActionResult Edit(int id)
        {
            var salaryComponent = _salaryComponentRepository.Get(id);
            ViewBag.ParentComponentId = new SelectList(_salaryComponentRepository.GetAll(), "Id", "Title", salaryComponent.ParentComponentId);
            return CheckForNullAndExecute(salaryComponent, () => View(salaryComponent));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("SalaryComponent", "ParentComponentId", SelectListState.Recreate)]
        public ActionResult Edit(SalaryComponent salaryComponent)
        {
            if (ModelState.IsValid)
            {
                _salaryComponentRepository.Update(salaryComponent);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(salaryComponent);
        }

        public ActionResult Delete(int id)
        {
            var salaryComponent = _salaryComponentRepository.Get(id);
            return CheckForNullAndExecute(salaryComponent, () => View(salaryComponent));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _salaryComponentRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
