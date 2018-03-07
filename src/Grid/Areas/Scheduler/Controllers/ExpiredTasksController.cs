using System;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.EmailService;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Areas.Scheduler.Controllers
{
    public class ExpiredTasksController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskActivityRepository _taskActivityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailComposerService _emailComposerService;
        public ExpiredTasksController(ITaskRepository taskRepository,
                                      ITaskActivityRepository taskActivityRepository,
                                      IUnitOfWork unitOfWork,
                                      EmailComposerService emailComposerService)
        {
            _taskRepository = taskRepository;
            _taskActivityRepository = taskActivityRepository;
            _unitOfWork = unitOfWork;
            _emailComposerService = emailComposerService;
        }

        public ActionResult Index()
        {
            var today = DateTime.UtcNow.Date;
            var expiredTasks = _taskRepository.GetAllBy(t => (t.TaskStatus != ProjectTaskStatus.Completed && t.TaskStatus != ProjectTaskStatus.Cancelled) && t.DueDate < today).ToList();
            foreach (var expiredTask in expiredTasks)
            {
                var taskId = expiredTask.Id;

                var selectedTask = _taskRepository.Get(taskId);
                var dueDate = selectedTask.DueDate?.ToString("d") ?? "No due date";
                var activity = new TaskActivity
                {
                    TaskId = selectedTask.Id,
                    Title = "Missed DueDate",
                    Comment = $"Missed the due date of {dueDate} as of {DateTime.UtcNow:d}, Sent the reminder",
                    CreatedByUserId = 1
                };

                _taskActivityRepository.Create(activity);
                _unitOfWork.Commit();

#if !DEBUG
                    _emailComposerService.TaskMissed(taskId);
#endif
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}