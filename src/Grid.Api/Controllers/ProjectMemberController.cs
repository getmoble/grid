using Grid.Api.Models;
using Grid.Api.Models.PMS;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Infrastructure.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Grid.Api.Controllers
{
    [GridPermission(PermissionCode = 300)]
    class ProjectMemberController : GridApiBaseController
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProjectMemberController(IProjectMemberRepository projectMemberRepository,
                                        IUserRepository userRepository,
                                        IProjectRepository projectRepository,
                                        IEmployeeRepository employeeRepository,
                                        IUnitOfWork unitOfWork)
        {
            _projectMemberRepository = projectMemberRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _projectMemberRepository.GetAll().Select(h => new ProjectModel(h)).ToList();
            }, "Projects Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = _projectMemberRepository.Get(id, "MemberEmployee.User.Person");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(ProjectMember vm)
        {
            ApiResult<ProjectMember> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var project = _projectMemberRepository.Get(vm.Id);
                        project.EmployeeId = vm.EmployeeId;
                        project.Rate = vm.Rate;
                        project.ProjectMemberRoleId = vm.ProjectMemberRoleId;
                        project.ProjectId = vm.ProjectId;
                        project.Billing = vm.Billing;
                        project.UpdatedByUserId = WebUser.Id;
                        project.MemberStatus = vm.MemberStatus;
                        project.UpdatedOn = DateTime.UtcNow;

                        _projectMemberRepository.Update(project);
                        _unitOfWork.Commit();
                        return project;
                    }, "ProjectMember updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var newProject = new ProjectMember
                        {
                            EmployeeId = vm.EmployeeId,
                            Rate = vm.Rate,
                            ProjectMemberRoleId = vm.ProjectMemberRoleId,
                            ProjectId = vm.ProjectId,
                            Billing = vm.Billing,
                            MemberStatus = vm.MemberStatus,
                            CreatedByUserId = WebUser.Id
                        };
                        _projectMemberRepository.Create(newProject);
                        _unitOfWork.Commit();
                        return newProject;
                    }, "ProjectMember created sucessfully");
                }

            }
            else
            {
                apiResult = ApiResultFromModelErrors<ProjectMember>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _projectRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "ProjectMember deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
