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
    public class RolesController : GridBaseController
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRoleMemberRepository _roleMemberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RolesController(IPermissionRepository permissionRepository,
                               IRoleRepository roleRepository,
                               IRolePermissionRepository rolePermissionRepository,
                               IRoleMemberRepository roleMemberRepository,
                               IUnitOfWork unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            _roleMemberRepository = roleMemberRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var role = _roleRepository.Get(id);

            var permissions = _rolePermissionRepository.GetAllBy(r => r.RoleId == role.Id, "Permission").Select(r => r.Permission).ToList();
            var members = _roleMemberRepository.GetAllBy(r => r.RoleId == role.Id, "User.Person").ToList().Select(m => m.User).ToList();

            var vm = new RoleDetailsViewModel(role)
            {
                Permissions = permissions,
                Users = members
            };

            return View(vm);
        }

        public ActionResult Create()
        {
            ViewBag.RolePermissions = new MultiSelectList(_permissionRepository.GetAll().ToList(), "Id", "Title");
            var vm = new CreateRoleViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateRoleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newRole = new Role
                {
                    Name = vm.Name,
                    Description = vm.Description
                };

                _roleRepository.Create(newRole);
                _unitOfWork.Commit();

                foreach (var permission in vm.PermissionIds)
                {
                    var rolePermission = new RolePermission
                    {
                        RoleId = newRole.Id,
                        PermissionId = permission
                    };
                    _rolePermissionRepository.Create(rolePermission);
                }

                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewBag.RolePermissions = new MultiSelectList(_permissionRepository.GetAll(), "Id", "Title");
            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var role = _roleRepository.Get(id);
            var permissions = _rolePermissionRepository.GetAllBy(r => r.RoleId == role.Id).Select(r => r.PermissionId).ToList();

            ViewBag.RolePermissions = new MultiSelectList(_permissionRepository.GetAll().ToList(), "Id", "Title", permissions);

            var vm = new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedOn = role.CreatedOn
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditRoleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var role = _roleRepository.Get(vm.Id);

                role.Name = vm.Name;
                role.Description = vm.Description;

                // Clean up all Permissions 
                var permissions = _rolePermissionRepository.GetAllBy(r => r.RoleId == role.Id).ToList();
                foreach (var permission in permissions)
                {
                    _rolePermissionRepository.Delete(permission);
                }

                _unitOfWork.Commit();

                foreach (var permission in vm.PermissionIds)
                {
                    var rolePermission = new RolePermission()
                    {
                        RoleId = vm.Id,
                        PermissionId = permission
                    };
                    _rolePermissionRepository.Create(rolePermission);
                }

                _unitOfWork.Commit();

                _roleRepository.Update(role);

                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewBag.RolePermissions = new MultiSelectList(_permissionRepository.GetAll(), "Id", "Title");
            return View(vm);
        }
    }
}
