using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.ViewModels;
using Grid.Infrastructure.Filters;

namespace Grid.Api.Controllers
{
    [APIIdentityInjector]
    public class TimeController : GridBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly ITimeSheetLineItemRepository _timeSheetLineItemRepository;

        public TimeController(IUserRepository userRepository,
                              ITimeSheetLineItemRepository timeSheetLineItemRepository)
        {
            _userRepository = userRepository;
            _timeSheetLineItemRepository = timeSheetLineItemRepository;
        }

        [Authorize]
        [HttpPost]
        public JsonResult MyTeam(TimeSheetSearchViewModel vm)
        {
            // Get only mine and my reportees
            var myReportees = _userRepository.GetAllBy(u => u.ReportingPersonId == WebUser.Id && u.EmployeeStatus != EmployeeStatus.Ex).Select(u => u.Id).ToList();
            var userList = _userRepository.GetAllBy(u => myReportees.Contains(u.Id), "Person").ToList();

            Func<IQueryable<TimeSheetLineItem>, IQueryable<TimeSheetLineItem>> timeSheetFilter = q =>
            {
                q = q.Include("TimeSheet.CreatedByUser.Person").Include("Project").Include("Task");

                if (vm.SubmittedUserById.HasValue)
                {
                    q = q.Where(r => r.TimeSheet.CreatedByUserId == vm.SubmittedUserById.Value);
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

                var userListIds = userList.Select(u => u.Id).ToList();
                q = q.Where(r => userListIds.Contains(r.TimeSheet.CreatedByUserId));

                q = q.OrderByDescending(d => d.CreatedOn);

                return q;
            };

            var timeSheets = _timeSheetLineItemRepository.Search(timeSheetFilter);
            var projected = timeSheets.Select(t => new ApiTimesheetLineItemModel(t)).ToList();
            return Json(projected);
        }
    }
}