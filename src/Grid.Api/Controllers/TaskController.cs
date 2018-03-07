using System;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Infrastructure;

namespace Grid.Api.Controllers
{
    public class TaskController: GridBaseController
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public ActionResult MyTasks(EventDataFilter vm)
        {
            Func<IQueryable<Task>, IQueryable<Task>> taskFilter = q =>
            {
                q = q.Where(t => t.AssigneeId == WebUser.Id);

                if (vm.start.HasValue)
                {
                    q = q.Where(t => t.DueDate >= vm.start.Value);
                }

                if (vm.end.HasValue)
                {
                    q = q.Where(t => t.DueDate <= vm.end.Value);
                }

                return q;
            };

            var tasks = _taskRepository.Search(taskFilter);

            var payload = tasks.Select(h => new EventData
            {
                id = h.Id,
                allDay = false,
                title = h.Title,
                start = h.DueDate.GetValueOrDefault().ToString("s"),
                end = h.DueDate?.AddHours(h.ExpectedTime.GetValueOrDefault()).ToString("s") ?? ""
            }).ToList();

            return Json(payload, JsonRequestBehavior.AllowGet);
        }
    }
}
