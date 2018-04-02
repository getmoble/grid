using Grid.Api.Models;
using Grid.Api.Models.PMS;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using System;
using System.Web.Mvc;

namespace Grid.Api.Controllers
{
    public class ProjectMemberRolesController : GridApiBaseController
    {
        private readonly IProjectMemberRoleRepository _projectMemberRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectMemberRolesController(IProjectMemberRoleRepository projectMemberRoleRepository,
                                   IUnitOfWork unitOfWork)
        {
            _projectMemberRoleRepository = projectMemberRoleRepository;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public JsonResult Get(int id)
        {
            var apiResult = TryExecute(() => _projectMemberRoleRepository.Get(id, "Department"), "Member Roles fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(ProjectMemberRoleModel memberRolesModel)
        {
            ApiResult<ProjectMemberRole> apiResult;

            if (ModelState.IsValid)
            {
                if (memberRolesModel.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var memberRole = _projectMemberRoleRepository.Get(memberRolesModel.Id);
                        memberRole.DepartmentId = memberRolesModel.DepartmentId;
                        memberRole.Title = memberRolesModel.Title;
                        memberRole.Description = memberRolesModel.Description;
                        memberRole.Role = memberRolesModel.Role;
                        memberRole.UpdatedByUserId = WebUser.Id;
                        memberRole.UpdatedOn = DateTime.UtcNow;

                        
                        _projectMemberRoleRepository.Update(memberRole);
                        _unitOfWork.Commit();
                        return memberRole;
                    }, "Member Role updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var memberRole = new ProjectMemberRole
                        {
                            Title = memberRolesModel.Title,
                            Description = memberRolesModel.Description,
                            DepartmentId = memberRolesModel.DepartmentId,
                            Id = memberRolesModel.Id,
                            Role = memberRolesModel.Role,
                            CreatedByUserId = WebUser.Id,
                            CreatedOn = DateTime.UtcNow,

                    };
                        _projectMemberRoleRepository.Create(memberRole);
                        _unitOfWork.Commit();
                        return memberRole;
                    }, "Member Role created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<ProjectMemberRole>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _projectMemberRoleRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Software deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
