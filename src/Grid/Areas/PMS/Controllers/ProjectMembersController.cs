using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using Grid.Filters;
using Grid.Infrastructure.Filters;
using Grid.Api.Models;
using Grid.Api.Models.PMS;

namespace Grid.Areas.PMS.Controllers
{
    [GridPermission(PermissionCode = 300)]
    public class ProjectMembersController : ProjectsBaseController
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProjectMembersController(IProjectMemberRepository projectMemberRepository,
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

        private bool DoIHaveAccessToProject(int projectId)
        {
            // Check whether i have access to this Project as Manager
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
            var isMember = _projectMemberRepository.Any(m => m.EmployeeId == employee.Id && m.ProjectId == projectId && m.ProjectMemberRole.Role == MemberRole.ProjectManager) || WebUser.IsAdmin;
            return isMember;
        }

        [SelectList("User", "UserId", SelectListState.Recreate)]
        [SelectList("Project", "ProjectId")]
        public ActionResult Create(int projectId)
        {
            if (!DoIHaveAccessToProject(projectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("User", "UserId", SelectListState.Recreate)]
        [SelectList("Project", "ProjectId", SelectListState.Recreate)]
        public ActionResult Create(ProjectMember projectMember)
        {
            if (ModelState.IsValid)
            {
                if (!DoIHaveAccessToProject(projectMember.ProjectId))
                {
                    return RedirectToAction("NotAuthorized", "Error", new { area = "" });
                }

                projectMember.CreatedByUserId = WebUser.Id;

                _projectMemberRepository.Create(projectMember);
                _unitOfWork.Commit();
                return RedirectToAction("Details", "Projects", new { Id = projectMember.ProjectId });
            }

            return View(projectMember);
        }
        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = _projectMemberRepository.Get(id, "MemberEmployee.User.Person");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult GetInactiveUsers(int projectId)
        //{
        //    ViewBag.UserId = WebUser.IsAdmin;
        //    bool isProjectLead;
        //    var user = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,Location,Department,Designation,Shift");
        //    isProjectLead = _projectMemberRepository.Any(m => m.EmployeeId == user.Id && m.Role == MemberRole.ProjectManager) || WebUser.IsAdmin;
        //    ViewBag.Role = isProjectLead;
        //    var apiResult = _projectMemberRepository.GetAllBy(i => i.ProjectId == projectId, "MemberEmployee.User.Person,Project").ToList();
        //    var inactiveUsers = apiResult.Where(u => u.MemberStatus == MemberStatus.InActive).OrderBy(i => i.MemberStatus).ToList().Select(h => new ProjectModel(h)).ToList();
        //    return PartialView("InactiveUsers", inactiveUsers);
        //}

        [HttpPost]
        public ActionResult CreateProjectMember(ProjectMember projectMember)
        {
            if (projectMember.Id > 0)
            {
                var selectedProjectMember = _projectMemberRepository.Get(projectMember.Id);

                if (selectedProjectMember != null)
                {
                    selectedProjectMember.ProjectMemberRoleId = projectMember.ProjectMemberRoleId;
                    selectedProjectMember.Billing = projectMember.Billing;
                    selectedProjectMember.Rate = projectMember.Rate;
                    selectedProjectMember.EmployeeId = projectMember.EmployeeId;
                    selectedProjectMember.UpdatedByUserId = WebUser.Id;
                    selectedProjectMember.MemberStatus = projectMember.MemberStatus;
                    _projectMemberRepository.Update(selectedProjectMember);
                    _unitOfWork.Commit();
                    return RedirectToAction("Details", "Projects", new { Id = projectMember.ProjectId });
                }

            }
            else
            {
                if (ModelState.IsValid)
                {
                    var alreadyMember = _projectMemberRepository.GetBy(i => i.EmployeeId == projectMember.EmployeeId && i.ProjectId == projectMember.ProjectId);

                    if (alreadyMember == null)
                    {
                        projectMember.CreatedByUserId = WebUser.Id;
                        _projectMemberRepository.Create(projectMember);
                        _unitOfWork.Commit();

                        return RedirectToAction("Details", "Projects", new { Id = projectMember.ProjectId });

                    }
                    else
                    {
                        return Json(false);
                    }

                }
            }
            return View(projectMember);
        }


        [HttpGet]
        public ActionResult ChangeMemberStatus(int id)
        {
            var selectedMember = _projectMemberRepository.Get(id);

            if (selectedMember != null)
            {
                if (selectedMember.MemberStatus == MemberStatus.Active)
                {
                    selectedMember.MemberStatus = MemberStatus.InActive;

                }
                else
                {
                    selectedMember.MemberStatus = MemberStatus.Active;
                }
                selectedMember.UpdatedByUserId = WebUser.Id;

                _projectMemberRepository.Update(selectedMember);
                _unitOfWork.Commit();

            }
            return Json(selectedMember, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit(int id)
        {
            var projectMember = _projectMemberRepository.Get(id);
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
            if (projectMember == null)
            {
                return HttpNotFound();
            }

            if (!DoIHaveAccessToProject(projectMember.ProjectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }
            ViewBag.UserId = new SelectList(_employeeRepository.GetAllBy(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex, "Person").ToList().OrderBy(u => u.User.Person.Name), "Id", "User.Person.Name", projectMember.EmployeeId);
            // ViewBag.UserId = new SelectList(_userRepository.GetAllBy(u => u.Id != 1 && u.EmployeeStatus != EmployeeStatus.Ex, "Person").ToList().OrderBy(u => u.Person.Name), "Id", "Person.Name", projectMember.UserId);
            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "Id", "Title", projectMember.ProjectId);
            return View(projectMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("User", "UserId", SelectListState.Recreate)]
        [SelectList("Project", "ProjectId", SelectListState.Recreate)]
        public ActionResult Edit(ProjectMember projectMember)
        {
            if (ModelState.IsValid)
            {
                if (!DoIHaveAccessToProject(projectMember.ProjectId))
                {
                    return RedirectToAction("NotAuthorized", "Error", new { area = "" });
                }

                var selectedProjectMember = _projectMemberRepository.Get(projectMember.Id, "ProjectMemberRole");

                if (selectedProjectMember != null)
                {
                    selectedProjectMember.ProjectMemberRoleId = projectMember.ProjectMemberRoleId;
                    selectedProjectMember.Billing = projectMember.Billing;
                    selectedProjectMember.Rate = projectMember.Rate;
                    selectedProjectMember.UpdatedByUserId = WebUser.Id;

                    _projectMemberRepository.Update(selectedProjectMember);
                    _unitOfWork.Commit();
                    return RedirectToAction("Details", "Projects", new { Id = projectMember.ProjectId });
                }
            }

            return View(projectMember);
        }

        public ActionResult Delete(int id)
        {
            var projectMember = _projectMemberRepository.Get(id);
            if (projectMember == null)
            {
                return HttpNotFound();
            }

            if (!DoIHaveAccessToProject(projectMember.ProjectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            return View(projectMember);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var projectMember = _projectMemberRepository.Get(id);

            if (!DoIHaveAccessToProject(projectMember.ProjectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            _projectMemberRepository.Delete(projectMember);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Projects", new { Id = projectMember.ProjectId });
        }
    }
}
