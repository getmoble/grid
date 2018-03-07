using System;
using System.IO;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using Grid.Features.PMS.ViewModels;
using Grid.Infrastructure.Filters;
using Grid.Features.HRMS.DAL.Interfaces;

namespace Grid.Areas.PMS.Controllers
{
    [GridPermission(PermissionCode = 300)]
    public class ProjectBillingsController : ProjectsBaseController
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectBillingRepository _projectBillingRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectBillingsController(IProjectMemberRepository projectMemberRepository,
                                         IProjectBillingRepository projectBillingRepository,
                                         IProjectRepository projectRepository,
                                         IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectBillingRepository = projectBillingRepository;
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        private bool DoIHaveAccessToProject(int projectId)
        {
            // Check whether i have access to this Project as Sales
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
            var isMember = _projectMemberRepository.Any(m => m.EmployeeId == employee.Id && m.ProjectId == projectId && m.Role == MemberRole.Sales) || WebUser.IsAdmin;
            return isMember;
        }

        public ActionResult Create(int projectId)
        {
            if (!DoIHaveAccessToProject(projectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            var vm = new ProjectBillingViewModel
            {
                ProjectId = projectId,
                BillingDate = DateTime.UtcNow
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectBillingViewModel projectBilling)
        {
            if (ModelState.IsValid)
            {
                if (!DoIHaveAccessToProject(projectBilling.ProjectId))
                {
                    return RedirectToAction("NotAuthorized", "Error", new { area = "" });
                }

                var newBilling = new ProjectBilling
                {
                    ProjectId = projectBilling.ProjectId,
                    BillingDate = projectBilling.BillingDate,
                    Amount = projectBilling.Amount,
                    Comments = projectBilling.Comments
                };

                if (projectBilling.Document != null && projectBilling.Document.ContentLength > 0)
                {
                    try
                    {
                        var fileName = projectBilling.Document.FileName;
                        var path = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(fileName));
                        projectBilling.Document.SaveAs(path);
                        newBilling.DocumentPath = fileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message;
                    }
                }


                newBilling.CreatedByUserId = WebUser.Id;

                _projectBillingRepository.Create(newBilling);
                _unitOfWork.Commit();

                return RedirectToAction("Details", "Projects", new { Id = projectBilling.ProjectId });
            }

            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "Id", "Title", projectBilling.ProjectId);
            return View(projectBilling);
        }

        public ActionResult Edit(int id)
        {
            var projectBilling = _projectBillingRepository.Get(id);
            if (projectBilling == null)
            {
                return HttpNotFound();
            }

            if (!DoIHaveAccessToProject(projectBilling.ProjectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "Id", "Title", projectBilling.ProjectId);
            return View(projectBilling);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectBillingViewModel projectBilling)
        {
            if (ModelState.IsValid)
            {
                if (!DoIHaveAccessToProject(projectBilling.ProjectId))
                {
                    return RedirectToAction("NotAuthorized", "Error", new { area = "" });
                }

                var selectedBilling = _projectBillingRepository.Get(projectBilling.Id);

                if (projectBilling.Document != null && projectBilling.Document.ContentLength > 0)
                {
                    if (selectedBilling != null)
                    {
                        selectedBilling.ProjectId = projectBilling.ProjectId;
                        selectedBilling.Amount = projectBilling.Amount;
                        selectedBilling.BillingDate = projectBilling.BillingDate;
                        selectedBilling.Comments = projectBilling.Comments;

                        try
                        {
                            var fileName = projectBilling.Document.FileName;
                            var path = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(fileName));
                            projectBilling.Document.SaveAs(path);
                            selectedBilling.DocumentPath = fileName;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message;
                        }
                    }

                }

                if (selectedBilling != null)
                {
                    selectedBilling.UpdatedByUserId = WebUser.Id;

                    _projectBillingRepository.Update(selectedBilling);
                    _unitOfWork.Commit();
                }
                
                return RedirectToAction("Details", "Projects", new { Id = projectBilling.ProjectId });
            }
            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "Id", "Title", projectBilling.ProjectId);
            return View(projectBilling);
        }

        public ActionResult Delete(int id)
        {
            var projectBilling = _projectBillingRepository.Get(id);
            if (projectBilling == null)
            {
                return HttpNotFound();
            }

            if (!DoIHaveAccessToProject(projectBilling.ProjectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            return View(projectBilling);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var projectBilling = _projectBillingRepository.Get(id);

            if (!DoIHaveAccessToProject(projectBilling.ProjectId))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            _projectBillingRepository.Delete(projectBilling);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Projects", new { Id = projectBilling.ProjectId });
        }
    }
}
