using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Text;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.ViewModels;
using Grid.Infrastructure.Extensions;

namespace Grid.Areas.TMS.Controllers
{
    public class TimeSheetLineItemsController : TimeSheetBaseController
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITimeSheetLineItemRepository _timeSheetLineItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TimeSheetLineItemsController(ITimeSheetLineItemRepository timeSheetLineItemRepository,
                                            IProjectRepository projectRepository,
                                            IUserRepository userRepository,
                                            IUnitOfWork unitOfWork)
        {
            _timeSheetLineItemRepository = timeSheetLineItemRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(TimeSheetEntrySearchViewModel vm)
        {
            ViewBag.ProjectId = new MultiSelectList(_projectRepository.GetAll().OrderBy(p => p.Title), "Id", "Title", vm.ProjectId);
            ViewBag.UserId = new MultiSelectList(_userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.Id != 1, "Person").ToList().OrderBy(p => p.Person.Name), "Id", "Person.Name", vm.UserId);

            Func<IQueryable<TimeSheetLineItem>, IQueryable<TimeSheetLineItem>> timeSheetLineItemFilter = q =>
            {
                q = q.Include(t => t.Project).Include("TimeSheet.CreatedByUser.Person");

                if (vm.ProjectId != null && vm.ProjectId.Any())
                {
                    q = q.Where(r => vm.ProjectId.Contains(r.ProjectId));
                }

                if (vm.UserId != null && vm.UserId.Any())
                {
                    q = q.Where(r => vm.UserId.Contains(r.TimeSheet.CreatedByUserId));
                }

                if (vm.State.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.State == vm.State.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date <= vm.EndDate.Value);
                }

                if (vm.WorkType.HasValue)
                {
                    q = q.Where(r => r.WorkType == vm.WorkType.Value);
                }

                return q;
            };

            vm.TimeSheetLineItems = _timeSheetLineItemRepository.SearchPage(timeSheetLineItemFilter, o => o.OrderByDescending(c => c.TimeSheet.Date), vm.GetPageNo(), vm.PageSize);

            return View(vm);
        }

        public ActionResult Download(TimeSheetEntrySearchViewModel vm)
        {
            Func<IQueryable<TimeSheetLineItem>, IQueryable<TimeSheetLineItem>> timeSheetLineItemFilter = q =>
            {
                q = q.Include(t => t.Project).Include("TimeSheet.CreatedByUser.Person").Include("TimeSheet.CreatedByUser.Department").Include("Project").Include("Task");

                if (vm.ProjectId != null && vm.ProjectId.Any())
                {
                    q = q.Where(r => vm.ProjectId.Contains(r.ProjectId));
                }

                if (vm.UserId != null && vm.UserId.Any())
                {
                    q = q.Where(r => vm.UserId.Contains(r.TimeSheet.CreatedByUserId));
                }

                if (vm.State.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.State == vm.State.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.Date <= vm.EndDate.Value);
                }

                if (vm.WorkType.HasValue)
                {
                    q = q.Where(r => r.WorkType == vm.WorkType.Value);
                }

                q = q.OrderByDescending(d => d.CreatedOn);

                return q;
            };

            var timeSheets = _timeSheetLineItemRepository.Search(timeSheetLineItemFilter);
            return ExportAsCSV(timeSheets);
        }

        private ActionResult ExportAsCSV(IEnumerable<TimeSheetLineItem> lineItems)
        {
            var sw = new StringBuilder();
            //write the header
            sw.AppendLine("Id,TimeSheetId,Project,Date,TaskId,Task,Effort,Billable,Comments,Created By,Created On,Project Type,Department");

            foreach (var record in lineItems)
            {
                sw.AppendLine($"{record.Id},{record.TimeSheetId},{record.Project.Title.RemoveComma()},{record.TimeSheet.Date.ToShortDateString()},{record.TaskId},{record.TaskSummary.RemoveComma()},{record.Effort},{record.WorkType}, ,{record.TimeSheet.CreatedByUser.Person.Name},{record.CreatedOn},{record.Project.ProjectType},{record.TimeSheet.CreatedByUser.Department.Title}");
            }

            return File(new UTF8Encoding().GetBytes(sw.ToString()), "text/csv", "TimeSheetExport.csv");
        }

        public ActionResult Details(int id)
        {
            var timeSheetLineItem = _timeSheetLineItemRepository.Get(id, "Project,TimeSheet.CreatedByUser.Person");
            return CheckForNullAndExecute(timeSheetLineItem, () => View(timeSheetLineItem));
        }
    }
}
