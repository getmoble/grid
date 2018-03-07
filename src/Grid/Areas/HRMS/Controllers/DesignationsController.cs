using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.ViewModels;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.HRMS.Controllers
{
    [GridPermission(PermissionCode = 210)]
    public class DesignationsController : GridBaseController
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDesignationRepository _designationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DesignationsController(IDesignationRepository designationRepository,
                                      IDepartmentRepository departmentRepository,
                                      IUnitOfWork unitOfWork)
        {
            _designationRepository = designationRepository;
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        [SelectList("Department", "DepartmentId")]
        public ActionResult Index(DesignationSearchViewModel vm)
        {
            Func<IQueryable<Designation>, IQueryable<Designation>> designationFilter = (q) =>
            {
                q = q.Include("Department");

                if (vm.DepartmentId.HasValue)
                {
                    q = q.Where(d => d.DepartmentId == vm.DepartmentId.Value);
                }

                if (vm.Band.HasValue)
                {
                    q = q.Where(d => d.Band == vm.Band.Value);
                }

                return q;
            };

            var designations = _designationRepository.Search(designationFilter);
            vm.Designations = designations.ToList();

            return View(vm);
        }
      
    }
}
