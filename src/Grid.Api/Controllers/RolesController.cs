using System.Linq;
using System.Web.Mvc;
using Grid.Api.Filters;
using Grid.Api.Models;
using Grid.Api.Models.Company;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Api.Controllers
{
    public class RolesController : GridApiBaseController
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RolesController(IRoleRepository roleRepository,
                               IRolePermissionRepository rolePermissionRepository,
                               IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index(RoleFilterModel vm)
        {
            if (vm.PermissionId.HasValue)
            {
                var apiResult = TryExecute(() =>
                {
                    return _rolePermissionRepository.GetAllBy(p => p.PermissionId == vm.PermissionId.Value, o => o.OrderByDescending(l => l.CreatedOn), "Role").Select(p => p.Role).ToList();
                }, "Roles Fetched sucessfully");

                return Json(apiResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var apiResult = TryExecute(() =>
                {
                    return _roleRepository.GetAll(o => o.OrderByDescending(l => l.CreatedOn));
                }, "Roles Fetched sucessfully");

                return Json(apiResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() =>
            {
                var selectedRole = _roleRepository.Get(id);
                var permissions = _rolePermissionRepository.GetAllBy(r => r.RoleId == id).ToList();

                var result = new RoleModel
                {
                    Id = selectedRole.Id,
                    Name = selectedRole.Name,
                    Description = selectedRole.Description,
                    //PermissionIds = selectedRole.Permissions,
                    PermissionIds = permissions.Select(p => p.PermissionId).ToList(),
                    CreatedOn = selectedRole.CreatedOn
                };

                return result;

            }, "Role Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(RoleModel vm)
        {
            ApiResult<Role> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var selectedRole = _roleRepository.Get(vm.Id);

                        selectedRole.Name = vm.Name;
                        selectedRole.Description = vm.Description;

                        // Clean up all Permissions 
                        var permissions = _rolePermissionRepository.GetAllBy(r => r.RoleId == vm.Id).ToList();
                        foreach (var permission in permissions)
                        {
                            _rolePermissionRepository.Delete(permission);
                        }

                        _unitOfWork.Commit();

                        foreach (var permission in vm.PermissionIds)
                        {
                            var rolePermission = new RolePermission
                            {
                                RoleId = vm.Id,
                                PermissionId = permission
                            };
                            _rolePermissionRepository.Create(rolePermission);
                        }

                        _roleRepository.Update(selectedRole);
                        _unitOfWork.Commit();

                        return selectedRole;

                    }, "Role updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
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

                        return newRole;
                    }, "Role created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Role>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _roleRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Role deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
