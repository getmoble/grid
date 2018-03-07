using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.HRMS.Controllers
{
    [GridPermission(PermissionCode = 210)]
    public class DepartmentsController : GridBaseController
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsController(IDepartmentRepository departmentRepository,
                                     IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var departments = _departmentRepository.GetAll(s => s.OrderBy(d => d.Title)).ToList();
            return View(departments);
        }
      
    }
}
