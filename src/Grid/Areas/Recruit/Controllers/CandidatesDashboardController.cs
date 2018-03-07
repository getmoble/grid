using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.ViewModels;

namespace Grid.Areas.Recruit.Controllers
{
    public class CandidatesDashboardController : RecruitBaseController
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CandidatesDashboardController(ICandidateRepository candidateRepository,
                                             IUnitOfWork unitOfWork)
        {
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
        }

        #region AjaxCalls
        public FileContentResult CandidatesByDesignationCSV(CandidateSearchViewModel vm)
        {
            Func<IQueryable<Candidate>, IQueryable<Candidate>> candidateFilter = q =>
            {
                q = q.Include("Person");

                if (vm.Source.HasValue)
                {
                    q = q.Where(r => r.Source == vm.Source.Value);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.Status == vm.Status.Value);
                }


                if (!string.IsNullOrEmpty(vm.Organization))
                {
                    q = q.Where(r => r.Person.Organization.Contains(vm.Organization));
                }

                return q;
            };

            var csv = new StringBuilder();
            var candidatesByDesignation = _candidateRepository.Search(candidateFilter).GroupBy(l => l.Person.Designation)
                                           .Select(x => new
                                           {
                                               x.Key,
                                               Value = x.Count()
                                           })
                                            .ToList();

            var keys = string.Join(",", candidatesByDesignation.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", candidatesByDesignation.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "CandidatesByDesignationCSV.csv");
        }
        public FileContentResult CandidatesBySourceCSV(CandidateSearchViewModel vm)
        {
            Func<IQueryable<Candidate>, IQueryable<Candidate>> candidateFilter = q =>
            {
                q = q.Include("Person");

                if (vm.Source.HasValue)
                {
                    q = q.Where(r => r.Source == vm.Source.Value);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.Status == vm.Status.Value);
                }


                if (!string.IsNullOrEmpty(vm.Organization))
                {
                    q = q.Where(r => r.Person.Organization.Contains(vm.Organization));
                }

                return q;
            };

            var csv = new StringBuilder();
            var candidatesBySource = _candidateRepository.Search(candidateFilter).GroupBy(l => l.Source)
                                           .Select(x => new
                                           {
                                               x.Key,
                                               Value = x.Count()
                                           })
                                            .ToList();

            var keys = string.Join(",", candidatesBySource.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", candidatesBySource.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "CandidatesBySourceCSV.csv");
        }
        #endregion

        public ActionResult Index(CandidateSearchViewModel vm)
        {
            var firstDay = DateTime.UtcNow.AddDays(-31);
            var lastDay = DateTime.UtcNow;

            Func<IQueryable<Candidate>, IQueryable<Candidate>> candidateFilter = q =>
            {
                q = q.Include("Person");

                if (vm.Source.HasValue)
                {
                    q = q.Where(r => r.Source == vm.Source.Value);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.Status == vm.Status.Value);
                }


                if (!string.IsNullOrEmpty(vm.Organization))
                {
                    q = q.Where(r => r.Person.Organization.Contains(vm.Organization));
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

                q = q.Where(r => r.RecievedOn >= firstDay);

                return q;
            };

            vm.Total = _candidateRepository.Count();
            var postedRequirements = _candidateRepository.Search(candidateFilter)
                                                         .ToList()
                                                         .GroupBy(r => r.RecievedOn.ToShortDateString())
                                                         .Select(r => new { Day = r.Key, Total = r.Count() });
            var dateArray = new List<string> { "x" };
            var postedArray = new List<string> { "new" };

            var counter = firstDay;
            while (counter <= lastDay)
            {
                var selectedDay = counter.ToShortDateString();
                dateArray.Add(selectedDay);

                var selectedPost = postedRequirements.FirstOrDefault(p => p.Day == selectedDay);
                postedArray.Add(selectedPost?.Total.ToString() ?? "0");

                counter = counter.AddDays(1);
            }

            ViewBag.Dates = dateArray;
            ViewBag.Posts = postedArray;

            return View(vm);
        }
    }
}