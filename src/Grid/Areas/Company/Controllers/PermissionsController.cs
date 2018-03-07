using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.ViewModels;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.Company.Controllers
{
    [GridPermission(PermissionCode = 200)]
    public class PermissionsController : GridBaseController
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRoleMemberRepository _roleMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionsController(IPermissionRepository permissionRepository,
                                     IRolePermissionRepository rolePermissionRepository,
                                     IRoleMemberRepository roleMemberRepository,
                                     IUserRepository userRepository,
                                     IUnitOfWork unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _roleMemberRepository = roleMemberRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var permission = _permissionRepository.Get(id);
            var vm = new PermissionDetailsViewModel(permission);

            var roles = _rolePermissionRepository.GetAllBy(p => p.PermissionId == permission.Id, "Role").Select(p => p.Role).ToList();
            var roleIds = roles.Select(r => r.Id).ToList();

            var userIds = _roleMemberRepository.GetAllBy(m => roleIds.Contains(m.RoleId)).Select(u => u.UserId).ToList();
            var users = _userRepository.GetAllBy(u => userIds.Contains(u.Id), "Person").ToList();

            vm.Roles = roles;
            vm.Users = users;

            return View(vm);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                _permissionRepository.Create(permission);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(permission);
        }

        public ActionResult Edit(int id)
        {
            var permission = _permissionRepository.Get(id);
            return CheckForNullAndExecute(permission, () => View(permission));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Permission permission)
        {
            if (ModelState.IsValid)
            {
                _permissionRepository.Update(permission);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(permission);
        }
    }
}
