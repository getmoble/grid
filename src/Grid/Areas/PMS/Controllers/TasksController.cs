using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.EmailService;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Hubs;
using Grid.Models;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;
using Grid.Features.PMS.ViewModels;
using Grid.Infrastructure.Filters;
using Newtonsoft.Json;

namespace Grid.Areas.PMS.Controllers
{
    [GridPermission(PermissionCode = 300)]
    public class TasksController : ProjectsBaseController
    {
        private readonly INotificationService _notificationService;

        private readonly ITaskRepository _taskRepository;
        private readonly ITaskEffortRepository _taskEffortRepository;
        private readonly ITaskActivityRepository _taskActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly EmailComposerService _emailComposerService;

        public TasksController(INotificationService notificationService,
                               ITaskRepository taskRepository,
                               ITaskActivityRepository taskActivityRepository,
                               ITaskEffortRepository taskEffortRepository,
                               IUserRepository userRepository,
                               IProjectRepository projectRepository,
                               IProjectMemberRepository projectMemberRepository,
                               EmailComposerService emailComposerService,
                               IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _taskRepository = taskRepository;
            _taskActivityRepository = taskActivityRepository;
            _taskEffortRepository = taskEffortRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _projectMemberRepository = projectMemberRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;

            _emailComposerService = emailComposerService;
        }


        public JsonResult AutoCompleteTasks(string query, long projectId)
        {
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id);
            var tasks = _taskRepository.GetAllBy(t => t.Title.Contains(query) && t.AssigneeId == employee.Id && t.ProjectId == projectId).Select(t => new AutoCompleteEntry { Id = t.Id, Value = t.Title, TaskBillingType = t.TaskBilling.ToString() }).ToList();
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllTasksByWorkType(string query)
        {
            // Get task workType 
            TaskBilling workType;
            var getTaskId = _taskRepository.GetBy(u => u.Title == query);
            workType = getTaskId.TaskBilling;
            return Json(workType.ToString(), JsonRequestBehavior.AllowGet);
        }


        private bool DoIManageProject(int projectId)
        {
            // Check whether i have access to this Project as a Manager
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
            var isMember = _projectMemberRepository.Any(m => m.EmployeeId == employee.Id && m.ProjectId == projectId && m.ProjectMemberRole.Role == MemberRole.ProjectManager);
            return isMember;
        }

        public List<Project> GetAllProjectsForMe()
        {
            // Get only Projects to which i am added
            var employee = _employeeRepository.GetBy(u => u.UserId == WebUser.Id, "User,User.Person,ReportingPerson.User.Person,Manager.User.Person,Location,Department,Designation,Shift");
            var myProjects = _projectMemberRepository.GetAllBy(m => m.EmployeeId == employee.Id, "Project").Select(m => m.Project).ToList();

            // Get all public Projects
            var publicProjects = _projectRepository.GetAllBy(p => p.IsPublic).ToList();

            // Merge public to this list
            foreach (var pp in publicProjects)
            {
                if (myProjects.All(p => p.Id != pp.Id))
                {
                    myProjects.Add(pp);
                }
            }

            return myProjects;
        }

        [HttpGet]
        public ActionResult GetAllAssigneesByprojectId(long id)
        {            
            var getProjectMembers = _projectMemberRepository.GetAllBy(u => u.ProjectId == id, "MemberEmployee.User.Person");
            var result = getProjectMembers.Select(i => new
            {
                Id = i.EmployeeId,
                Name = i.MemberEmployee.User.Person.Name
            }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public List<User> GetAllMyReportees()
        {
            // Get only mine and my reportees
            var myReportees = _userRepository.GetAllBy(u => u.Id == WebUser.Id || (u.ReportingPersonId == WebUser.Id && u.EmployeeStatus != EmployeeStatus.Ex)).Select(u => u.Id).ToList();
            var userList = _userRepository.GetAllBy(u => myReportees.Contains(u.Id), "Person").ToList();
            return userList;
        }

        public ActionResult Index(TaskSearchViewModel vm)
        {
            var myProjects = GetAllProjectsForMe();
            ViewBag.AssignedToId = new SelectList(_userRepository.GetAll("Person"), "Id", "Person.Name", vm.AssignedToId);
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Title", vm.ProjectId);

            Func<IQueryable<Task>, IQueryable<Task>> taskFilter = q =>
            {
                q = q.Include("Assignee.User.Person").Include("CreatedByUser.Person").Include(t => t.Project);

                if (vm.AssignedToId.HasValue)
                {
                    q = q.Where(r => r.AssigneeId == vm.AssignedToId.Value);
                }

                if (vm.ProjectId.HasValue)
                {
                    q = q.Where(r => r.ProjectId == vm.ProjectId.Value);
                }

                if (!string.IsNullOrEmpty(vm.Title))
                {
                    q = q.Where(r => r.Title.Contains(vm.Title));
                }

                if (vm.HideCompleted)
                {
                    q = q.Where(r => r.TaskStatus != ProjectTaskStatus.Cancelled && r.TaskStatus != ProjectTaskStatus.Completed);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.TaskStatus == vm.Status.Value);
                }

                // Restrict to my tasks
                q = q.Where(t => t.AssigneeId == WebUser.Id);

                q = q.OrderByDescending(c => c.CreatedOn);

                return q;
            };

            vm.Tasks = _taskRepository.Search(taskFilter).ToList().Select(t => new TaskViewModel(t)).ToList();
            return View(vm);
        }

        public ActionResult MyTeam(TaskSearchViewModel vm)
        {
            var myProjects = GetAllProjectsForMe();
            var userList = GetAllMyReportees();

            ViewBag.AssignedToId = new SelectList(userList, "Id", "Person.Name");
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Title");

            Func<IQueryable<Task>, IQueryable<Task>> taskFilter = q =>
            {
                q = q.Include("Assignee.User.Person").Include("CreatedByUser.Person").Include(t => t.Project);

                if (vm.AssignedToId.HasValue)
                {
                    q = q.Where(r => r.AssigneeId == vm.AssignedToId.Value);
                }

                if (vm.ProjectId.HasValue)
                {
                    q = q.Where(r => r.ProjectId == vm.ProjectId.Value);
                }

                if (!string.IsNullOrEmpty(vm.Title))
                {
                    q = q.Where(r => r.Title.Contains(vm.Title));
                }

                if (vm.HideCompleted)
                {
                    q = q.Where(r => r.TaskStatus != ProjectTaskStatus.Cancelled && r.TaskStatus != ProjectTaskStatus.Completed);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.TaskStatus == vm.Status.Value);
                }

                if (!WebUser.IsAdmin)
                {
                    // Restrict to my team members
                    var userListIds = userList.Select(u => u.Id).ToList();
                    q = q.Where(t => t.AssigneeId.HasValue && userListIds.Contains(t.AssigneeId.Value));
                }

                q = q.OrderByDescending(c => c.CreatedOn);

                return q;
            };

            vm.Tasks = _taskRepository.Search(taskFilter).ToList().Select(t => new TaskViewModel(t)).ToList();
            return View(vm);
        }

        public ActionResult Calendar()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var task = _taskRepository.Get(id, "Assignee.User.Person,CreatedByUser.Person,Project");
            var activities = _taskActivityRepository.GetAllBy(l => l.TaskId == task.Id, o => o.OrderByDescending(t => t.CreatedOn));

            if (task == null)
            {
                return HttpNotFound();
            }

            var vm = new TaskDetailsViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                ExpectedTime = task.ExpectedTime,
                ActualTime = task.ActualTime,
                TaskStatus = task.TaskStatus,
                Priority = task.Priority,
                TaskBilling = task.TaskBilling,
                AssigneeId = task.AssigneeId,
                Assignee = task.Assignee,
                StartDate = task.StartDate,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
                Project = task.Project,
                ParentId = task.ParentId,
                Parent = task.Parent,
                CreatedOn = task.CreatedOn,
                CreatedByUserId = task.CreatedByUserId,
                CreatedByUser = task.CreatedByUser,
                TaskActivities = activities.ToList()
            };

            ViewBag.CanManageTask = task.CreatedByUserId == WebUser.Id || DoIManageProject(task.ProjectId);

            return View(vm);
        }

        public ActionResult Create(int? projectId)
        {
            var myProjects = GetAllProjectsForMe();
            var userList = GetAllMyReportees();

            ViewBag.AssigneeId = new SelectList(userList, "Id", "Person.Name", WebUser.Id);
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Title", projectId);

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
        public ActionResult Create(CreateTaskViewModel vm)
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

#if !DEBUG
                    _emailComposerService.TaskCreated(newTask.Id);
#endif

                var selectedTask = _taskRepository.Get(newTask.Id, "Assignee.User.Person");
                // Log as a Task Activity
                var dueDate = selectedTask.DueDate?.ToString("d") ?? "No due date";
                var assignedTo = selectedTask.AssigneeId.HasValue ? selectedTask.Assignee.User.Person.Name : "No Assignee";
                var activity = new TaskActivity
                {
                    TaskId = selectedTask.Id,
                    Title = "Created",
                    Comment = $"{WebUser.Name} created the Task at {selectedTask.CreatedOn.ToString("G")} with state {selectedTask.TaskStatus}, expected hours {selectedTask.ExpectedTime}, due date as {dueDate} and assigned to {assignedTo}",
                    CreatedByUserId = WebUser.Id
                };

                _taskActivityRepository.Create(activity);
                _unitOfWork.Commit();

                // Update Task Code 
                if (selectedTask != null)
                {
                    selectedTask.Code = $"TK{newTask.Id}";
                    _taskRepository.Update(selectedTask);
                    _unitOfWork.Commit();

                    var assignedToUser = _userRepository.Get(selectedTask.AssigneeId.GetValueOrDefault());
                    if (assignedToUser != null)
                    {
                        _notificationService.NotifyUser("New Task", selectedTask.Title, assignedToUser.Code);
                    }
                }

                return RedirectToAction("Index");
            }

            var myProjects = GetAllProjectsForMe();
            var userList = GetAllMyReportees();

            ViewBag.AssigneeId = new SelectList(userList, "Id", "Person.Name", vm.AssigneeId);
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Title", vm.ProjectId);

            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var task = _taskRepository.Get(id);

            var myProjects = GetAllProjectsForMe();
            var userList = GetAllMyReportees();

            ViewBag.AssigneeId = new SelectList(userList, "Id", "Person.Name", task.AssigneeId);
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Title", task.ProjectId);

            var vm = new EditTaskViewModel(task);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditTaskViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var selectedTask = _taskRepository.Get(vm.Id, "Assignee.User.Person");

                if (selectedTask != null)
                {
                    selectedTask.ProjectId = vm.ProjectId;
                    selectedTask.Title = vm.Title;
                    selectedTask.Description = vm.Description;
                    selectedTask.ExpectedTime = vm.ExpectedTime;
                    selectedTask.TaskStatus = vm.TaskStatus;
                    selectedTask.Priority = vm.Priority;
                    selectedTask.AssigneeId = vm.AssigneeId;
                    selectedTask.StartDate = vm.StartDate;
                    selectedTask.DueDate = vm.DueDate;

                    selectedTask.UpdatedByUserId = WebUser.Id;

                    _taskRepository.Update(selectedTask);
                    _unitOfWork.Commit();

#if !DEBUG
                        _emailComposerService.TaskUpdated(selectedTask.Id);
#endif

                    // Log as a Task Activity
                    var dueDate = selectedTask.DueDate?.ToString("d") ?? "No due date";
                    var assignedTo = selectedTask.AssigneeId.HasValue ? selectedTask.Assignee.User.Person.Name : "No Assignee";
                    var activity = new TaskActivity
                    {
                        TaskId = selectedTask.Id,
                        Title = "Updated",
                        Comment = $"{WebUser.Name} updated the Task at {DateTime.UtcNow.ToString("G")} with state {selectedTask.TaskStatus}, expected hours {selectedTask.ExpectedTime}, due date as {dueDate} and assigned to {assignedTo}",
                        CreatedByUserId = WebUser.Id
                    };

                    _taskActivityRepository.Create(activity);
                    _unitOfWork.Commit();

                    return RedirectToAction("Index");
                }
            }

            var myProjects = GetAllProjectsForMe();
            var userList = GetAllMyReportees();

            ViewBag.AssigneeId = new SelectList(userList, "Id", "Person.Name", vm.AssigneeId);
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Title", vm.ProjectId);

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var task = _taskRepository.Get(id);
            return CheckForNullAndExecute(task, () => View(task));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _taskRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTaskStatus(int id)
        {
            var task = _taskRepository.Get(id);
            var vm = new ChangeTaskStatusViewModel { TaskId = task.Id, TaskStatus = task.TaskStatus };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeTaskStatus(ChangeTaskStatusViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var selectedTask = _taskRepository.Get(vm.Id, "Assignee.User.Person");
                if (selectedTask != null)
                {
                    selectedTask.TaskStatus = vm.TaskStatus;
                    selectedTask.UpdatedByUserId = WebUser.Id;

                    _taskRepository.Update(selectedTask);
                    _unitOfWork.Commit();


#if !DEBUG
                        _emailComposerService.TaskUpdated(selectedTask.Id);
#endif

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

                    // Log as a Task Activity
                    var activity = new TaskActivity
                    {
                        TaskId = selectedTask.Id,
                        Title = "Changed Status",
                        Comment = $"{WebUser.Name} changed status of the Task at {DateTime.UtcNow.ToString("G")} to state {selectedTask.TaskStatus}, with comments - {vm.Comments}",
                        CreatedByUserId = WebUser.Id
                    };

                    _taskActivityRepository.Create(activity);
                    _unitOfWork.Commit();

                    // Notify Creator of the Task
                    var createdByUser = _userRepository.Get(selectedTask.CreatedByUserId);
                    if (createdByUser != null)
                    {
                        var message = $"{selectedTask.Title} has been changed to {selectedTask.TaskStatus}";
                        _notificationService.NotifyUser("Task Status Changed", message, createdByUser.Code);
                    }

                    return RedirectToAction("Details", "Tasks", new { selectedTask.Id });
                }
            }

            return View(vm);
        }
    }
}
