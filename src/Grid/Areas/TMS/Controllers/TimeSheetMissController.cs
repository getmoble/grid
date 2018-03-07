using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FluentDateTime;
using Grid.Areas.TMS.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.ViewModels;
using Grid.Infrastructure.Extensions;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.TMS.Controllers
{
    [GridPermission(PermissionCode = 550)]
    public class TimeSheetMissController : TimeSheetBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMissedTimeSheetRepository _missedTimeSheetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TimeSheetMissController(IUserRepository userRepository,
                                       IMissedTimeSheetRepository missedTimeSheetRepository,
                                       IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _missedTimeSheetRepository = missedTimeSheetRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(MissedTimeSheetsSearchViewModel vm)
        {
            ViewBag.SubmittedUserById = new SelectList(_userRepository.GetAllBy(u => u.Id != 1, "Person").ToList().OrderBy(p => p.Person.Name), "Id", "Person.Name", vm.SubmittedUserById);
            Func<IQueryable<MissedTimeSheet>, IQueryable<MissedTimeSheet>> timeSheetFilter = q =>
            {
                q = q.Include("User.Person");

                if (vm.SubmittedUserById.HasValue)
                {
                    q = q.Where(r => r.UserId == vm.SubmittedUserById.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.Date >= vm.StartDate.Value);
                }
                else
                {
                    if (!vm.IsPostBack)
                    {
                        var monday = DateTime.Today.Previous(DayOfWeek.Monday);
                        vm.StartDate = monday;
                        q = q.Where(r => r.Date >= vm.StartDate.Value);
                    }
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.Date <= vm.EndDate.Value);
                }
                else
                {
                    if (!vm.IsPostBack)
                    {
                        var friday = DateTime.Today.Next(DayOfWeek.Friday);
                        vm.EndDate = friday;
                        q = q.Where(r => r.Date <= vm.EndDate.Value);
                        vm.IsPostBack = true;
                    }
                }

                q = q.OrderByDescending(d => d.Date).ThenBy(d => d.CreatedOn);

                return q;
            };

            var result = _missedTimeSheetRepository.Search(timeSheetFilter);
            vm.MissedTimeSheets = result.ToList();
            return View(vm);
        }

        public ActionResult Download(TimeSheetSearchViewModel vm)
        {
            Func<IQueryable<MissedTimeSheet>, IQueryable<MissedTimeSheet>> timeSheetItemFilter = q =>
            {
                q = q.Include("User.Person");

                if (vm.SubmittedUserById.HasValue)
                {
                    q = q.Where(r => r.UserId == vm.SubmittedUserById.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.Date >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.Date <= vm.EndDate.Value);
                }

                q = q.OrderByDescending(d => d.Date).ThenBy(d => d.CreatedOn);

                return q;
            };

            var timeSheetItems = _missedTimeSheetRepository.Search(timeSheetItemFilter);

            return ExportAsCSV(timeSheetItems);
        }

        private ActionResult ExportAsCSV(IEnumerable<MissedTimeSheet> lineItems)
        {
            var sw = new StringBuilder();
            //write the header
            sw.AppendLine("Id,Date,User,Filled On");

            foreach (var record in lineItems)
            {
                sw.AppendLine($"{record.Id},{record.Date},{record.User.Person.Name.RemoveComma()},{record.FilledOn}");
            }

            return File(new UTF8Encoding().GetBytes(sw.ToString()), "text/csv", "MissedTimeSheetExport.csv");
        }
    }
}