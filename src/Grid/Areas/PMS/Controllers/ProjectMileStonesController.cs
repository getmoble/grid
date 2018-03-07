using System;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.PMS.Controllers
{
    [GridPermission(PermissionCode = 300)]
    public class ProjectMileStonesController : ProjectsBaseController
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectMileStoneRepository _projectMileStoneRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectMileStonesController(IProjectMemberRepository projectMemberRepository,
                                           IProjectMileStoneRepository projectMileStoneRepository,
                                           IProjectRepository projectRepository,
                                           IUserRepository userRepository,
                                           IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectMileStoneRepository = projectMileStoneRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        private bool DoIHaveAccessToProject(int projectId)
        {
            // Check whether i have access to this Project as Manager
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
            var isMember = _projectMemberRepository.Any(m => m.EmployeeId == employee.Id && m.ProjectId == projectId && m.Role == MemberRole.ProjectManager) || WebUser.IsAdmin;
            return isMember;
        }

        public ActionResult Details(int id)
        {
            var projectMileStone = _projectMileStoneRepository.Get(id);

            if (projectMileStone == null)
            {
                return HttpNotFound();
            }

            if (!DoIHaveAccessToProject(projectMileStone.ProjectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            return View(projectMileStone);
        }

        public ActionResult Create(int projectId)
        {
            if (!DoIHaveAccessToProject(projectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            var mileStone = new ProjectMileStone
            {
                ProjectId = projectId,
                TargetDate = DateTime.Now
            };

            return View(mileStone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectMileStone projectMileStone)
        {
            if (ModelState.IsValid)
            {
                if (!DoIHaveAccessToProject(projectMileStone.ProjectId))
                {
                    return RedirectToAction("NotAuthorized", "Error", new { area = "" });
                }

                projectMileStone.CreatedByUserId = WebUser.Id;

                _projectMileStoneRepository.Create(projectMileStone);
                _unitOfWork.Commit();

                return RedirectToAction("Details", "Projects", new { Id = projectMileStone.ProjectId });
            }

            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "Id", "Title", projectMileStone.ProjectId);
            return View(projectMileStone);
        }

        public ActionResult Edit(int id)
        {
            var projectMileStone = _projectMileStoneRepository.Get(id);

            if (projectMileStone == null)
            {
                return HttpNotFound();
            }

            if (!DoIHaveAccessToProject(projectMileStone.ProjectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "Id", "Title", projectMileStone.ProjectId);
            return View(projectMileStone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectMileStone projectMileStone)
        {
            if (ModelState.IsValid)
            {
                if (!DoIHaveAccessToProject(projectMileStone.ProjectId))
                {
                    return RedirectToAction("NotAuthorized", "Error", new { area = "" });
                }

                var selectedProjectMileStone = _projectMileStoneRepository.GetBy(l => l.Id == projectMileStone.Id);

                if (selectedProjectMileStone != null)
                {
                    selectedProjectMileStone.Title = projectMileStone.Title;
                    selectedProjectMileStone.Description = projectMileStone.Description;
                    selectedProjectMileStone.TargetDate = projectMileStone.TargetDate;
                    selectedProjectMileStone.Status = projectMileStone.Status;
                    selectedProjectMileStone.UpdatedByUserId = WebUser.Id;

                    _projectMileStoneRepository.Update(selectedProjectMileStone);
                    _unitOfWork.Commit();
                    return RedirectToAction("Details", "Projects", new { Id = selectedProjectMileStone.ProjectId });
                }
            }

            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "Id", "Title", projectMileStone.ProjectId);
            return View(projectMileStone);
        }

        public ActionResult Delete(int id)
        {
            var projectMileStone = _projectMileStoneRepository.Get(id);
            if (projectMileStone == null)
            {
                return HttpNotFound();
            }

            if (!DoIHaveAccessToProject(projectMileStone.ProjectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            return View(projectMileStone);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var projectMileStone = _projectMileStoneRepository.Get(id);
            if (!DoIHaveAccessToProject(projectMileStone.ProjectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }
            _projectMileStoneRepository.Delete(projectMileStone);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Projects", new { Id = projectMileStone.ProjectId });
        }
    }
}
