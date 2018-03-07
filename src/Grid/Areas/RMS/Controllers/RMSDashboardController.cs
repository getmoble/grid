using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Entities;
using Grid.Features.RMS.Entities.Enums;
using Grid.Features.RMS.ViewModels;
using Grid.Infrastructure;

namespace Grid.Areas.RMS.Controllers
{
    public class RMSDashboardController : GridBaseController
    {
        private readonly IRequirementRepository _requirementRepository;
        private readonly IRequirementCategoryRepository _requirementCategoryRepository;
        private readonly ICRMLeadSourceRepository _crmLeadSourceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RMSDashboardController(IRequirementRepository requirementRepository,
                                      IRequirementCategoryRepository requirementCategoryRepository,
                                      ICRMLeadSourceRepository crmLeadSourceRepository,
                                      IUnitOfWork unitOfWork)
        {
            _requirementRepository = requirementRepository;
            _requirementCategoryRepository = requirementCategoryRepository;
            _crmLeadSourceRepository = crmLeadSourceRepository;
            _unitOfWork = unitOfWork;
        }

        #region AjaxCalls
        public FileContentResult RequirementsByCategoryCSV(RequirementSearchViewModel vm)
        {
            Func<IQueryable<Requirement>, IQueryable<Requirement>> requirementFilter = q =>
            {
                if (vm.SourceId.HasValue)
                {
                    q = q.Where(r => r.SourceId == vm.SourceId.Value);
                }

                if (vm.CategoryId.HasValue)
                {
                    q = q.Where(r => r.CategoryId == vm.CategoryId.Value);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.RequirementStatus == vm.Status.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn <= vm.EndDate.Value);
                }

                return q;
            };

            var csv = new StringBuilder();
            var requirementsByCategory = _requirementRepository.Search(requirementFilter).GroupBy(l => l.Category.Title)
                                           .Select(x => new
                                           {
                                               x.Key,
                                               Value = x.Count()
                                           })
                                            .ToList();

            var keys = string.Join(",", requirementsByCategory.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", requirementsByCategory.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "RequirementsByCategoryCSV.csv");
        }
        public FileContentResult RequirementsByStatusCSV(RequirementSearchViewModel vm)
        {
            Func<IQueryable<Requirement>, IQueryable<Requirement>> requirementFilter = q =>
            {
                if (vm.SourceId.HasValue)
                {
                    q = q.Where(r => r.SourceId == vm.SourceId.Value);
                }

                if (vm.CategoryId.HasValue)
                {
                    q = q.Where(r => r.CategoryId == vm.CategoryId.Value);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.RequirementStatus == vm.Status.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn >= vm.StartDate.Value);
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn <= vm.EndDate.Value);
                }

                return q;
            };

            var csv = new StringBuilder();
            var requirementsByStatus = _requirementRepository.Search(requirementFilter).GroupBy(l => l.RequirementStatus)
                                           .Select(x => new
                                           {
                                               x.Key,
                                               Value = x.Count()
                                           })
                                            .ToList();

            var keys = string.Join(",", requirementsByStatus.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", requirementsByStatus.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "RequirementsByStatusCSV.csv");
        }
        #endregion  

        public ActionResult Index(RequirementSearchViewModel vm)
        {
            var firstDay = DateTime.UtcNow.AddDays(-31);
            var lastDay = DateTime.UtcNow;

            ViewBag.CategoryId = new SelectList(_requirementCategoryRepository.GetAll(), "Id", "Title", vm.CategoryId);
            ViewBag.SourceId = new SelectList(_crmLeadSourceRepository.GetAll(), "Id", "Title", vm.SourceId);

            Func<IQueryable<Requirement>, IQueryable<Requirement>> requirementFilter = q =>
            {
                if (vm.SourceId.HasValue)
                {
                    q = q.Where(r => r.SourceId == vm.SourceId.Value);
                }

                if (vm.CategoryId.HasValue)
                {
                    q = q.Where(r => r.CategoryId == vm.CategoryId.Value);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.RequirementStatus == vm.Status.Value);
                }

                if (vm.StartDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn >= vm.StartDate.Value);
                    firstDay = vm.StartDate.Value;
                }

                if (vm.EndDate.HasValue)
                {
                    q = q.Where(r => r.CreatedOn <= vm.EndDate.Value);
                    lastDay = vm.EndDate.Value;
                }

                return q;
            };

            

            vm.Total = _requirementRepository.Count();

            Func<IQueryable<Requirement>, IQueryable<Requirement>> postedRequirementFilter = q =>
            {
                q = q.Where(r => r.CreatedOn >= firstDay && r.PostedOn != null);
                return requirementFilter(q);
            };

            Func<IQueryable<Requirement>, IQueryable<Requirement>> closedRequirementFilter = q =>
            {
                q = q.Where(r => r.CreatedOn >= firstDay && r.RequirementStatus == RequirementStatus.Won);
                return requirementFilter(q);
            };

            Func<IQueryable<Requirement>, IQueryable<Requirement>> lostRequirementFilter = q =>
            {
                q = q.Where(r => r.CreatedOn >= firstDay && r.RequirementStatus == RequirementStatus.Lost);
                return requirementFilter(q);
            };

            var postedRequirements = _requirementRepository.Search(postedRequirementFilter)
                                                         .ToList()
                                                         .GroupBy(r => r.CreatedOn.ToShortDateString())
                                                         .Select(r => new { Day = r.Key, Total = r.Count() });
            var closedRequirements = _requirementRepository.Search(closedRequirementFilter)
                                                         .ToList()
                                                         .GroupBy(r => r.CreatedOn.ToShortDateString())
                                                         .Select(r => new { Day = r.Key, Total = r.Count() });
            var lostRequirements = _requirementRepository.Search(lostRequirementFilter)
                                                         .ToList()
                                                         .GroupBy(r => r.CreatedOn.ToShortDateString())
                                                         .Select(r => new { Day = r.Key, Total = r.Count() });
            var dateArray = new List<string> { "x" };
            var postedArray = new List<string> { "new" };
            var closedArray = new List<string> { "won" };
            var lostArray = new List<string> { "lost" };

            var counter = firstDay;
            while (counter <= lastDay)
            {
                var selectedDay = counter.ToShortDateString();
                dateArray.Add(selectedDay);

                var selectedPost = postedRequirements.FirstOrDefault(p => p.Day == selectedDay);
                postedArray.Add(selectedPost?.Total.ToString() ?? "0");

                var selectedClosed = closedRequirements.FirstOrDefault(p => p.Day == selectedDay);
                closedArray.Add(selectedClosed != null ? selectedClosed.Total.ToString() : "0");

                var selectedLost = lostRequirements.FirstOrDefault(p => p.Day == selectedDay);
                lostArray.Add(selectedLost != null ? selectedLost.Total.ToString() : "0");


                counter = counter.AddDays(1);
            }

            ViewBag.Dates = dateArray;
            ViewBag.Posts = postedArray;
            ViewBag.Closed = closedArray;
            ViewBag.Lost = lostArray;

            return View(vm);
        }
    }
}