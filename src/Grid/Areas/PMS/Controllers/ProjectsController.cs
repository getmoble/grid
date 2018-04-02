using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using Grid.Hubs;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.HRMS.DAL.Interfaces;
using Newtonsoft.Json;
using SelectItem = Grid.Models.SelectItem;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Entities;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using Grid.Features.PMS.ViewModels;
using Grid.Features.PMS.ViewModels.Project;
using Grid.Filters;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.PMS.Controllers
{

    public class ProjectsController : ProjectsBaseController
    {
        private readonly INotificationService _notificationService;

        private readonly IProjectRepository _projectRepository;
        private readonly IProjectActivityRepository _projectActivityRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectTechnologyMapRepository _projectTechnologyMapRepository;
        private readonly IProjectMileStoneRepository _projectMileStoneRepository;
        private readonly IProjectBillingRepository _projectBillingRepository;
        private readonly IProjectBillingCorrectionRepository _projectBillingCorrectionRepository;
        private readonly IProjectDocumentRepository _projectDocumentRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskEffortRepository _taskEffortRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly ITimeSheetLineItemRepository _timeSheetLineItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITimeSheetRepository _timeSheetRepository;

        private readonly ITechnologyRepository _technologyRepository;
        private readonly ICRMContactRepository _crmContactRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectsController(INotificationService notificationService,
                                  IProjectRepository projectRepository,
                                  IProjectActivityRepository projectActivityRepository,
                                  IProjectMemberRepository projectMemberRepository,
                                  IProjectBillingRepository projectBillingRepository,
                                  IProjectBillingCorrectionRepository projectBillingCorrectionRepository,
                                  ITaskRepository taskRepository,
                                  ITaskEffortRepository taskEffortRepository,
                                  IProjectTechnologyMapRepository projectTechnologyMapRepository,
                                  IProjectMileStoneRepository projectMileStoneRepository,
                                  IProjectDocumentRepository projectDocumentRepository,
                                  ILeaveRepository leaveRepository,
                                  ITimeSheetLineItemRepository timeSheetLineItemRepository,
                                  IUserRepository userRepository,
                                  IEmployeeRepository employeeRepository,
                                  ITimeSheetRepository timeSheetRepository,

        ITechnologyRepository technologyRepository,
                                  ICRMContactRepository crmContactRepository,
                                  IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;

            _projectRepository = projectRepository;
            _projectActivityRepository = projectActivityRepository;
            _projectBillingRepository = projectBillingRepository;
            _projectMemberRepository = projectMemberRepository;
            _projectBillingCorrectionRepository = projectBillingCorrectionRepository;
            _taskRepository = taskRepository;
            _taskEffortRepository = taskEffortRepository;
            _projectTechnologyMapRepository = projectTechnologyMapRepository;
            _projectMileStoneRepository = projectMileStoneRepository;
            _leaveRepository = leaveRepository;
            _timeSheetLineItemRepository = timeSheetLineItemRepository;
            _projectDocumentRepository = projectDocumentRepository;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _timeSheetRepository = timeSheetRepository;
            _technologyRepository = technologyRepository;
            _crmContactRepository = crmContactRepository;
            _unitOfWork = unitOfWork;
        }

        #region AjaxCalls
        [HttpGet]
        public ActionResult MileStones(EventDataFilter vm)
        {
            Func<IQueryable<ProjectMileStone>, IQueryable<ProjectMileStone>> mileStoneFilter = q =>
            {
                q = q.Where(p => p.ProjectId == vm.id);

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.TargetDate >= vm.start.Value);
                }

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.TargetDate <= vm.end.Value);
                }

                q = q.OrderByDescending(c => c.CreatedOn);

                return q;
            };

            var payload = _projectMileStoneRepository.Search(mileStoneFilter).ToList().Select(h => new EventData
            {
                id = h.Id,
                allDay = false,
                title = h.Title,
                start = h.TargetDate.ToString("s"),
                end = h.TargetDate.ToString("s")
            }).ToList();

            return Json(payload, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Tasks(EventDataFilter vm)
        {
            Func<IQueryable<Task>, IQueryable<Task>> taskFilter = q =>
            {
                q = q.Where(t => t.ProjectId == vm.id);

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.DueDate >= vm.start.Value);
                }

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.DueDate <= vm.end.Value);
                }

                q = q.OrderByDescending(c => c.CreatedOn);

                return q;
            };

            var payload = _taskRepository.Search(taskFilter).ToList().Select(h => new EventData
            {
                id = h.Id,
                allDay = false,
                title = h.Title,
                start = h.StartDate.GetValueOrDefault().ToString("s"),
                end = h.DueDate?.AddHours(h.ExpectedTime.GetValueOrDefault()).ToString("s") ?? ""
            }).ToList();

            return Json(payload, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Leaves(EventDataFilter vm)
        {
            // Restrict to Members of the Project
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");

            var members = _projectMemberRepository.GetAllBy(m => m.ProjectId == vm.id).Select(m => m.EmployeeId).Distinct().ToList();

            Func<IQueryable<Leave>, IQueryable<Leave>> leaveFilter = q =>
            {
                q = q.Include("CreatedByUser.Person").Where(p => members.Contains(p.CreatedByUserId));

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.Start >= vm.start.Value);
                }

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.End <= vm.end.Value);
                }

                q = q.OrderByDescending(c => c.CreatedOn);

                return q;
            };

            var payload = _leaveRepository.Search(leaveFilter).ToList().Select(h => new EventData
            {
                id = h.Id,
                allDay = true,
                title = $"{h.CreatedByUser.Person.Name} - Leave - {h.Status}",
                start = h.Start.ToString("s"),
                end = h.End.ToString("s")
            }).ToList();

            return Json(payload, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetProjectsTree()
        {
            var projects = _projectRepository.GetAll();
            var projectNodes = projects.Select(p => new ProjectTreeNode
            {
                id = p.Id.ToString(CultureInfo.InvariantCulture),
                text = p.Title,
                parent = p.ParentId.ToString()
            }).ToList();
            return Json(projectNodes, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public FileContentResult EffortByDateCSV(int projectId)
        {
            var csv = new StringBuilder();
            csv.AppendLine("x,Total,My Effort");
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");



            var lineItems = _timeSheetLineItemRepository.GetAllBy(l => l.ProjectId == projectId, "TimeSheet,TimeSheet.CreatedByUser.Person")
                             .GroupBy(x => new { x.TimeSheet.Date, x.TimeSheet.CreatedByUserId, x.TimeSheet.CreatedByUser.Person.Name })
                                    .Select(x => new
                                    {
                                        Value = x.Sum(y => y.Effort),
                                        //Id = x.Key.CreatedByUserId,
                                        Day = (DateTime)x.Key.Date,
                                        //Name=x.Key.Name,      
                                        Individual = x.Where(i => i.TimeSheet.CreatedByUserId == WebUser.Id).Sum(y => y.Effort),
                                    })
                                    .ToList();

            foreach (var lineItem in lineItems)
            {
                var date = lineItem.Day.ToShortDateString();
                csv.AppendLine($"{date}, {lineItem.Value},{lineItem.Individual}");

            }

            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "EffortByDateCSV.csv");
        }

        [HttpGet]
        public string GetAllActivitiesForProject(int id)
        {
            var activities = _projectActivityRepository.GetAllBy(r => r.ProjectId == id, o => o.OrderByDescending(r => r.CreatedOn)).ToList();
            var list = JsonConvert.SerializeObject(activities, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return list;
        }
        [HttpPost]
        public JsonResult AddNote(ProjectActivityViewModel vm)
        {
            var selectedProject = _projectRepository.Get(vm.ProjectId);
            if (selectedProject != null)
            {
                // Add it as an Activity
                var newActivity = new ProjectActivity
                {
                    Title = vm.Title,
                    Comment = vm.Comment,
                    ProjectId = selectedProject.Id,
                    CreatedByUserId = WebUser.Id
                };

                _projectActivityRepository.Create(newActivity);
                _unitOfWork.Commit();

                return Json(true);
            }

            return Json(false);
        }
        [HttpPost]
        public JsonResult DeleteNote(int id)
        {
            _projectActivityRepository.Delete(id);
            _unitOfWork.Commit();

            return Json(true);
        }

        #endregion

        private bool DoIHaveAccessToProject(int projectId)
        {
            // Check whether i have access to this Project as a Manager
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
            var isMember = _projectMemberRepository.Any(m => m.EmployeeId == employee.Id && m.ProjectId == projectId && m.ProjectMemberRole.Role == MemberRole.ProjectManager) || WebUser.IsAdmin;
            return isMember;
        }

        [GridPermission(PermissionCode = 300)]
        public ActionResult Index(ProjectSearchViewModel vm)
        {
            ViewBag.IsAdmin = WebUser.IsAdmin;

            ViewBag.ParentId = new SelectList(_projectRepository.GetAllBy(p => p.ParentId == null).OrderBy(p => p.Title), "Id", "Title");

            Func<IQueryable<Project>, IQueryable<Project>> projectFilter = q =>
            {
                q = q.Include(p => p.Client).Include(p => p.ParentProject);

                if (vm.IsPublic.HasValue)
                {
                    q = q.Where(r => r.IsPublic == vm.IsPublic.Value);
                }

                if (vm.ClientId.HasValue)
                {
                    q = q.Where(r => r.ClientId == vm.ClientId.Value);
                }

                if (vm.ParentId.HasValue)
                {
                    q = q.Where(r => r.ParentId == vm.ParentId.Value);
                }

                if (!string.IsNullOrEmpty(vm.Title))
                {
                    q = q.Where(r => r.Title.Contains(vm.Title));
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.Status == vm.Status.Value);
                }

                if (vm.Billing.HasValue)
                {
                    q = q.Where(r => r.Billing == vm.Billing.Value);
                }

                if (!vm.ShowClosedProjects)
                {
                    q = q.Where(r => r.Status != ProjectStatus.Closed);
                }

                if (!WebUser.IsAdmin)
                {
                    // Restrict to Projects to which i am a Member or project is public 
                    var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
                    var myProjects = _projectMemberRepository.GetAllBy(m => m.EmployeeId == employee.Id).Select(m => m.ProjectId).Distinct().ToList();
                    q = q.Where(p => myProjects.Contains(p.Id));
                }

                q = q.OrderByDescending(d => d.UpdatedOn);

                return q;
            };

            vm.Projects = _projectRepository.Search(projectFilter).ToList();
            return View(vm);
        }

        [GridPermission(PermissionCode = 300)]
        public ActionResult Tree(ProjectSearchViewModel vm)
        {
            // Special Case for getting parent Id = null
            ViewBag.ParentId = new SelectList(_projectRepository.GetAllBy(p => p.ParentId == null).OrderBy(p => p.Title), "Id", "Title");

            Func<IQueryable<Project>, IQueryable<Project>> projectFilter = q =>
            {
                q = q.Include(p => p.Client).Include(p => p.ParentProject);

                if (vm.IsPublic.HasValue)
                {
                    q = q.Where(r => r.IsPublic == vm.IsPublic.Value);
                }

                if (vm.ClientId.HasValue)
                {
                    q = q.Where(r => r.ClientId == vm.ClientId.Value);
                }

                if (vm.ParentId.HasValue)
                {
                    q = q.Where(r => r.ParentId == vm.ParentId.Value);
                }

                if (!string.IsNullOrEmpty(vm.Title))
                {
                    q = q.Where(r => r.Title.Contains(vm.Title));
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.Status == vm.Status.Value);
                }

                if (vm.Billing.HasValue)
                {
                    q = q.Where(r => r.Billing == vm.Billing.Value);
                }

                if (!WebUser.IsAdmin)
                {
                    // Restrict to Projects to which i am a Member
                    var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
                    var myProjects = _projectMemberRepository.GetAllBy(m => m.EmployeeId == employee.Id).Select(m => m.ProjectId).Distinct().ToList();
                    q = q.Where(p => myProjects.Contains(p.Id));
                }

                q = q.OrderByDescending(c => c.UpdatedOn);

                return q;
            };

            vm.Projects = _projectRepository.Search(projectFilter).ToList();

            vm.TreeNodes = vm.Projects.Select(p => new ProjectTreeNode
            {
                id = p.Id.ToString(CultureInfo.InvariantCulture),
                text = p.Title,
                parent = p.ParentId?.ToString(CultureInfo.InvariantCulture) ?? "#"
            }).ToList();

            return View(vm);
        }

        [GridPermission(PermissionCode = 300)]
        public ActionResult Details(int id)
        {
            // Check whether i have access to this Project details 
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
            var isMember = _projectMemberRepository.Any(m => m.EmployeeId == employee.Id && m.ProjectId == id);
            if (!isMember && !WebUser.IsAdmin)
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            var project = _projectRepository.Get(id, "Client,Client.Person,ParentProject");

            if (project == null)
            {
                return HttpNotFound();
            }

            // My Projects -> Projects to which i have access
            var myProjects = _projectMemberRepository.GetAllBy(m => m.EmployeeId == employee.Id).Select(m => m.ProjectId).Distinct().ToList();
            var projectsList = _projectRepository.GetAllBy(p => myProjects.Contains(p.Id) && !(p.Status == ProjectStatus.Closed || p.Status == ProjectStatus.Cancelled)).Select(p => new SelectItem
            {
                Id = p.Id,
                Title = p.Title
            }).OrderBy(p => p.Title).ToList();


            if (WebUser.IsAdmin == true)
            {
                ViewBag.webUser = WebUser.IsAdmin;
                var allProjects = _projectRepository.GetAllBy(p => !(p.Status == ProjectStatus.Closed || p.Status == ProjectStatus.Cancelled)).Select(p => new SelectItem
                {
                    Id = p.Id,
                    Title = p.Title
                }).OrderBy(p => p.Title).ToList();

                ViewBag.AllProject = new SelectList(allProjects, "Id", "Title", project.Id);
            }

            ViewBag.MyProjectsId = new SelectList(projectsList, "Id", "Title", project.Id);

            // Set Billing Access
            var hasBillingAccess = _projectMemberRepository.Any(m => m.EmployeeId == employee.Id && m.ProjectId == project.Id && m.ProjectMemberRole.Role == MemberRole.Sales) || WebUser.IsAdmin;
            ViewBag.HasBillingAccess = hasBillingAccess;

            // Set PM Access
            var hasPMAccess = _projectMemberRepository.Any(m => m.EmployeeId == employee.Id && m.ProjectId == project.Id && m.ProjectMemberRole.Role == MemberRole.ProjectManager) || WebUser.IsAdmin;
            ViewBag.HasPMAccess = hasPMAccess;

            // Set Lead Access
            var hasLeadAccess = _projectMemberRepository.Any(m => m.EmployeeId == employee.Id && m.ProjectId == project.Id && m.ProjectMemberRole.Role == MemberRole.Lead) || WebUser.IsAdmin;
            ViewBag.HasLeadAccess = hasLeadAccess;


            var projectBillings = _projectBillingRepository.GetAllBy(b => b.ProjectId == project.Id);
            var projectMembers = _projectMemberRepository.GetAllBy(m => m.ProjectId == project.Id, "MemberEmployee.User.Person");
            var projectMileStones = _projectMileStoneRepository.GetAllBy(m => m.ProjectId == project.Id);
            var projectDocuments = _projectDocumentRepository.GetAllBy(p => p.ProjectId == project.Id);
            var projectBillingCorrections = _projectBillingCorrectionRepository.GetAllBy(b => b.ProjectId == project.Id);

            List<Task> tasks;
            if (WebUser.IsAdmin || hasPMAccess || hasLeadAccess)
            {
                tasks = _taskRepository.GetAllBy(m => m.ProjectId == project.Id, o => o.OrderByDescending(p => p.DueDate)).ToList();
            }
            else
            {
                tasks = _taskRepository.GetAllBy(m => m.ProjectId == project.Id && (m.AssigneeId == employee.Id || m.CreatedByUserId == WebUser.Id), o => o.OrderByDescending(p => p.DueDate)).ToList();
            }

            var technologies = _projectTechnologyMapRepository.GetAllBy(r => r.ProjectId == project.Id, "Technology").Select(t => t.Technology).ToList();

            // Timesheet Details 
            var projectContributions = _timeSheetLineItemRepository.GetAllBy(p => p.ProjectId == project.Id, "Timesheet")
                                                    .GroupBy(g => g.TimeSheet.CreatedByUserId)
                                                    .Select(i => new ProjectContribution
                                                    {
                                                        UserId = i.Key,
                                                        Effort = i.Sum(k => k.Effort)
                                                    }).ToList();

            // Cannot use Project members as public projects doesnt have any project members
            var allUsers = _userRepository.GetAll("Person").ToList();
            foreach (var projectContribution in projectContributions)
            {
                var selectedUser = allUsers.FirstOrDefault(p => p.Id == projectContribution.UserId);
                if (selectedUser != null)
                    projectContribution.Name = selectedUser.Person.Name;
            }

            var projectViewModel = new ProjectViewModel
            {
                Id = project.Id,
                ClientId = project.ClientId,
                Client = project.Client,
                Title = project.Title,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status.Value,
                ParentId = project.ParentId,
                ParentProject = project.ParentProject,
                Billing = project.Billing,
                ExpectedBillingAmount = project.ExpectedBillingAmount,
                ProjectBillings = projectBillings.ToList(),
                ProjectBillingCorrections = projectBillingCorrections.ToList(),
                ProjectMembers = projectMembers.ToList(),
                ProjectMileStonesStones = projectMileStones.ToList(),
                Tasks = tasks,
                ProjectDocuments = projectDocuments.ToList(),
                CreatedOn = project.CreatedOn,
                Contributions = projectContributions,
                Technologies = technologies,
                Settings =
                    !string.IsNullOrEmpty(project.Setting)
                        ? JsonConvert.DeserializeObject<ProjectSettings>(project.Setting)
                        : new ProjectSettings()
            };

            // Take saved Settings or fallback to default.

            return View(projectViewModel);
        }

        [GridPermission(PermissionCode = 240)]
        [MultiSelectList("Technology", "Technologies")]
        [SelectList("CRMContact", "ClientId")]
        [SelectList("Project", "ParentId")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [GridPermission(PermissionCode = 240)]
        [MultiSelectList("Technology", "Technologies", MultiSelectListState.Recreate)]
        [SelectList("CRMContact", "ClientId", SelectListState.Recreate)]
        [SelectList("Project", "ParentId", SelectListState.Recreate)]
        public ActionResult Create(NewProjectViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
                var newProject = new Project
                {
                    ClientId = vm.ClientId,
                    Title = vm.Title,
                    Description = vm.Description,
                    StartDate = vm.StartDate,
                    EndDate = vm.EndDate,
                    Status = vm.Status,
                    Billing = vm.Billing,
                    ParentId = vm.ParentId,
                    IsPublic = vm.IsPublic,
                    InheritMembers = vm.InheritMembers,
                    IsClosed = vm.IsClosed,
                    CreatedByUserId = WebUser.Id
                };


                _projectRepository.Create(newProject);
                _unitOfWork.Commit();

                // Add user as a Project Member
                var projectMember = new ProjectMember
                {
                    ProjectId = newProject.Id,
                    EmployeeId = employee.Id,
                    // EmployeeId = WebUser.Id,
                    //ProjectMemberRoleId = projectMember.ProjectMemberRoleId,
                    Billing = Billing.NonBillable,
                    Rate = 0,
                    CreatedByUserId = WebUser.Id
                };

                _projectMemberRepository.Create(projectMember);
                _unitOfWork.Commit();

                // If Need to Copy, copy from Project Project
                if (vm.InheritMembers && vm.ParentId.HasValue)
                {
                    var parentMembers = _projectMemberRepository.GetAllBy(m => m.ProjectId == vm.ParentId.Value).ToList();
                    foreach (var member in parentMembers)
                    {
                        // Add user as a Project Member
                        var newProjectMember = new ProjectMember
                        {
                            ProjectId = newProject.Id,
                            EmployeeId = employee.Id,
                            ProjectMemberRoleId = member.ProjectMemberRoleId,
                            Billing = member.Billing,
                            CreatedByUserId = WebUser.Id
                        };

                        _projectMemberRepository.Create(newProjectMember);
                    }

                    _unitOfWork.Commit();
                }

                // Map the Technologies
                if (vm.TechnologyIds != null)
                {
                    foreach (var technologyId in vm.TechnologyIds)
                    {
                        var newMap = new ProjectTechnologyMap
                        {
                            ProjectId = newProject.Id,
                            TechnologyId = technologyId
                        };

                        _projectTechnologyMapRepository.Create(newMap);
                    }

                    _unitOfWork.Commit();
                }

                return RedirectToAction("Index");
            }

            return View(vm);
        }

        [GridPermission(PermissionCode = 300)]
        public ActionResult Edit(int id)
        {
            if (!DoIHaveAccessToProject(id))
            {
                return RedirectToAction("NotAuthorized", "Error", new { area = "" });
            }

            var project = _projectRepository.Get(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var mappedTechnologies = _projectTechnologyMapRepository.GetAllBy(m => m.ProjectId == project.Id).Select(m => m.TechnologyId).ToList();

            ViewBag.Technologies = new MultiSelectList(_technologyRepository.GetAll(), "Id", "Title", mappedTechnologies);
            ViewBag.ClientId = new SelectList(_crmContactRepository.GetAll("Person"), "Id", "Person.Name", project.ClientId);
            ViewBag.ParentId = new SelectList(_projectRepository.GetAll(), "Id", "Title", project.ParentId);

            var vm = new EditProjectViewModel(project);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [GridPermission(PermissionCode = 300)]
        [MultiSelectList("Technology", "Technologies", MultiSelectListState.Recreate)]
        [SelectList("CRMContact", "ClientId", SelectListState.Recreate)]
        [SelectList("Project", "ParentId", SelectListState.Recreate)]
        public ActionResult Edit(EditProjectViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (!DoIHaveAccessToProject(vm.Id))
                {
                    return RedirectToAction("NotAuthorized", "Error", new { area = "" });
                }

                var selectedProject = _projectRepository.Get(vm.Id);

                if (selectedProject != null)
                {
                    selectedProject.ClientId = vm.ClientId;
                    selectedProject.Title = vm.Title;
                    selectedProject.Description = vm.Description;
                    selectedProject.StartDate = vm.StartDate;
                    selectedProject.EndDate = vm.EndDate;
                    selectedProject.Status = vm.Status;
                    selectedProject.Billing = vm.Billing;
                    selectedProject.ExpectedBillingAmount = vm.ExpectedBillingAmount;
                    selectedProject.ParentId = vm.ParentId;
                    selectedProject.IsPublic = vm.IsPublic;

                    selectedProject.UpdatedByUserId = WebUser.Id;

                    _projectRepository.Update(selectedProject);
                    _unitOfWork.Commit();


                    // Remove the existing mapped Technologies 
                    var existingMaps = _projectTechnologyMapRepository.GetAllBy(m => m.ProjectId == selectedProject.Id);
                    foreach (var map in existingMaps)
                    {
                        _projectTechnologyMapRepository.Delete(map);
                    }

                    _unitOfWork.Commit();

                    // Map the New Technologies
                    if (vm.TechnologyIds != null)
                    {
                        foreach (var technologyId in vm.TechnologyIds)
                        {
                            var newMap = new ProjectTechnologyMap
                            {
                                ProjectId = vm.Id,
                                TechnologyId = technologyId
                            };

                            _projectTechnologyMapRepository.Create(newMap);
                        }

                        _unitOfWork.Commit();
                    }

                    return RedirectToAction("Index");
                }
            }

            return View(vm);
        }

        [GridPermission(PermissionCode = 240)]
        public ActionResult Delete(int id)
        {
            var project = _projectRepository.Get(id);
            return CheckForNullAndExecute(project, () => View(project));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [GridPermission(PermissionCode = 240)]
        public ActionResult DeleteConfirmed(int id)
        {
            _projectRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult CreateTask(int projectId)
        {
            var projectMembers = _projectMemberRepository.GetAllBy(p => p.ProjectId == projectId, "Employee.User.Person").ToList().Select(m => m.MemberEmployee).ToList();
            ViewBag.AssigneeId = new SelectList(projectMembers, "Id", "Person.Name", WebUser.Id);

            var vm = new CreateTaskViewModel
            {
                TaskStatus = ProjectTaskStatus.Notstarted,
                Priority = TaskPriority.Normal,
                StartDate = DateTime.Now
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTask(CreateTaskViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newTask = new Task
                {
                    Title = vm.Title,
                    Description = vm.Description,
                    ExpectedTime = vm.ExpectedTime,
                    ActualTime = vm.ActualTime,
                    TaskStatus = vm.TaskStatus,
                    Priority = vm.Priority,
                    AssigneeId = vm.AssigneeId,
                    StartDate = vm.StartDate,
                    DueDate = vm.DueDate,
                    ProjectId = vm.ProjectId,
                    ParentId = vm.ParentId,
                    CreatedByUserId = WebUser.Id
                };

                _taskRepository.Create(newTask);
                _unitOfWork.Commit();

                // Update Task Code 
                var selectedTask = _taskRepository.Get(newTask.Id);
                if (selectedTask != null)
                {
                    selectedTask.Code = $"TK{newTask.Id}";

                    _taskRepository.Update(selectedTask);
                    _unitOfWork.Commit();

                    var assignedToUser = _userRepository.Get(selectedTask.AssigneeId.GetValueOrDefault());
                    if (assignedToUser != null)
                        _notificationService.NotifyUser("New Task", selectedTask.Title, assignedToUser.Code);
                }

                return RedirectToAction("Details", "Projects", new { Id = newTask.ProjectId });
            }

            var projectMembers = _projectMemberRepository.GetAllBy(p => p.ProjectId == vm.ProjectId, "MemberEmployee.User.Person").ToList().Select(m => m.MemberEmployee).ToList();
            ViewBag.AssigneeId = new SelectList(projectMembers, "Id", "Person.Name");
            return View(vm);
        }

        public ActionResult EditTask(int id)
        {
            var task = _taskRepository.Get(id);
            var projectMembers = _projectMemberRepository.GetAllBy(p => p.ProjectId == task.ProjectId, "MemberEmployee.User.Person").ToList().Select(m => m.MemberEmployee).ToList();

            ViewBag.ParentId = new SelectList(_taskRepository.GetAll(), "Id", "Id", task.ParentId);
            ViewBag.AssigneeId = new SelectList(projectMembers, "Id", "Person.Name", task.AssigneeId);
            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "Id", "Title", task.ProjectId);

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTask(Task task)
        {
            if (ModelState.IsValid)
            {
                var selectedTask = _taskRepository.Get(task.Id);

                if (selectedTask != null)
                {
                    selectedTask.Title = task.Title;
                    selectedTask.Description = task.Description;
                    selectedTask.ExpectedTime = task.ExpectedTime;
                    selectedTask.TaskStatus = task.TaskStatus;
                    selectedTask.Priority = task.Priority;
                    selectedTask.AssigneeId = task.AssigneeId;
                    selectedTask.DueDate = task.DueDate;
                    selectedTask.StartDate = task.StartDate;

                    selectedTask.UpdatedByUserId = WebUser.Id;

                    _taskRepository.Update(selectedTask);
                    _unitOfWork.Commit();

                    return RedirectToAction("Details", "Projects", new { Id = selectedTask.ProjectId });
                }
            }

            ViewBag.ParentId = new SelectList(_taskRepository.GetAll(), "Id", "Id");

            var projectMembers = _projectMemberRepository.GetAllBy(p => p.ProjectId == task.ProjectId, "MemberEmployee.User.Person").ToList().Select(m => m.MemberEmployee).ToList();
            ViewBag.AssigneeId = new SelectList(projectMembers, "Id", "Person.Name", task.AssigneeId);

            ViewBag.CreatedByUserId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name");
            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "Id", "Title", task.ProjectId);

            return View(task);
        }

        public ActionResult ChangeTaskStatus(int id)
        {
            var task = _taskRepository.Get(id);

            var vm = new ChangeTaskStatusViewModel
            {
                TaskId = task.Id,
                TaskStatus = task.TaskStatus
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeTaskStatus(ChangeTaskStatusViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var selectedTask = _taskRepository.Get(vm.TaskId);

                if (selectedTask != null)
                {
                    selectedTask.TaskStatus = vm.TaskStatus;
                    selectedTask.UpdatedByUserId = WebUser.Id;

                    _taskRepository.Update(selectedTask);
                    _unitOfWork.Commit();

                    // Add Task Effort 
                    var effort = new TaskEffort
                    {
                        TaskId = selectedTask.Id,
                        Effort = vm.Effort,
                        Comments = vm.Comments,
                        CreatedByUserId = WebUser.Id
                    };

                    _taskEffortRepository.Create(effort);
                    _unitOfWork.Commit();

                    return RedirectToAction("Details", "Projects", new { Id = selectedTask.ProjectId });
                }
            }

            return View(vm);
        }

        public ActionResult TaskDetails(long id)
        {
            var task = _taskRepository.GetBy(t => t.Id == id, "Assignee.User.Person,CreatedByUser.Person,Project");
            return CheckForNullAndExecute(task, () => View(task));
        }


        public ActionResult Settings(int projectId)
        {
            var project = _projectRepository.Get(projectId);
            var settings = !string.IsNullOrEmpty(project.Setting) ? JsonConvert.DeserializeObject<ProjectSettings>(project.Setting) : new ProjectSettings { ProjectId = project.Id };
            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(ProjectSettings settings)
        {
            var project = _projectRepository.Get(settings.ProjectId);
            project.Setting = JsonConvert.SerializeObject(settings);
            _projectRepository.Update(project);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Projects", new { project.Id });
        }
    }
}
