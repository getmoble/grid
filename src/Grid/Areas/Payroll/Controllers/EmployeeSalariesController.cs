using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Payroll.DAL.Interfaces;
using Grid.Features.Payroll.Entities;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.Payroll.Controllers
{
    [GridPermission(PermissionCode = 210)]
    public class EmployeeSalariesController : GridBaseController
    {
        private readonly IEmployeeSalaryRepository _employeeSalaryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeSalariesController(IEmployeeSalaryRepository employeeSalaryRepository,
                                          IUnitOfWork unitOfWork)
        {
            _employeeSalaryRepository = employeeSalaryRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var employeeSalaries = _employeeSalaryRepository.GetAll();
            return View(employeeSalaries);
        }

        public ActionResult Details(int id)
        {
            var employeeSalary = _employeeSalaryRepository.Get(id);
            return CheckForNullAndExecute(employeeSalary, () => View(employeeSalary));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeSalary employeeSalary)
        {
            if (ModelState.IsValid)
            {
                _employeeSalaryRepository.Create(employeeSalary);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(employeeSalary);
        }

        public ActionResult Edit(int id)
        {
            var employeeSalary = _employeeSalaryRepository.Get(id);
            return CheckForNullAndExecute(employeeSalary, () => View(employeeSalary));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeSalary employeeSalary)
        {
            if (ModelState.IsValid)
            {
               _employeeSalaryRepository.Update(employeeSalary);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(employeeSalary);
        }

        public ActionResult Delete(int id)
        {
            var employeeSalary = _employeeSalaryRepository.Get(id);
            return CheckForNullAndExecute(employeeSalary, () => View(employeeSalary));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _employeeSalaryRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
