using System.Linq;
using System.Web.Mvc;
using Grid.Api.Filters;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;

namespace Grid.Api.Controllers
{
    public class PermissionsController : GridApiBaseController
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionsController(IPermissionRepository permissionRepository,
                                     IRolePermissionRepository rolePermissionRepository,
                                   IUnitOfWork unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index(PermissionFilterModel vm)
        {
            if (vm.RoleId.HasValue)
            {
                var apiResult = TryExecute(() =>
                {
                    return _rolePermissionRepository.GetAllBy(r => r.RoleId == vm.RoleId.Value, o => o.OrderByDescending(l => l.CreatedOn), "Permission").Select(r => r.Permission).ToList();
                }, "Permissions Fetched sucessfully");

                return Json(apiResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var apiResult = TryExecute(() =>
                {
                    return _permissionRepository.GetAll(o => o.OrderByDescending(l => l.CreatedOn));
                }, "Permissions Fetched sucessfully");

                return Json(apiResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _permissionRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Permission deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}

