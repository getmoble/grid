using System;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.ViewModels;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.EmailService;

namespace Grid.Api.Controllers
{
    public class TasksController : GridApiBaseController
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskEffortRepository _taskEffortRepository;
        private readonly ITaskActivityRepository _taskActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailComposerService _emailComposerService;


        public TasksController(
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

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _taskRepository.GetAll("Project"), "Tasks Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _taskRepository.Get(id, "Assignee.User.Person"), "Task fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Task vm)
        {
            ApiResult<Task> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
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
                            selectedTask.TaskBilling = vm.TaskBilling;

                            selectedTask.UpdatedByUserId = WebUser.Id;

                            _taskRepository.Update(selectedTask);
                            _unitOfWork.Commit();

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

#if !DEBUG
                        _emailComposerService.TaskUpdated(selectedTask.Id);
#endif
                        }
                            return selectedTask;
                    }, "Task updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
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
                            CreatedByUserId = WebUser.Id,
                            TaskBilling = vm.TaskBilling
                            
                    };

                        _taskRepository.Create(newTask);
                        _unitOfWork.Commit();

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

                        }

#if !DEBUG
                    _emailComposerService.TaskCreated(newTask.Id);
#endif
                                                
                        return vm;
                    }, "Task created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Task>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _taskRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Task deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
