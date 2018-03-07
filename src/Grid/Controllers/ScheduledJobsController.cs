using System.Linq;
using System.Web.Mvc;
using Grid.Data;
using Grid.Features.Common;
using Grid.Infrastructure;
using Grid.Models;
using Grid.ViewModels;
using Hangfire;
using Newtonsoft.Json;
using System;

namespace Grid.Controllers
{
    public class ScheduledJobsController : GridBaseController
    {
        private readonly GridDataContext _db;

        public ScheduledJobsController(GridDataContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            var tenantScheduledJobs = _db.ScheduledJobs.ToList();
            return View(tenantScheduledJobs.ToList());
        }

        public ActionResult Details(int id)
        {
            ScheduledJob tenantScheduledJob = _db.ScheduledJobs.Find(id);
            if (tenantScheduledJob == null)
            {
                return HttpNotFound();
            }
            return View(tenantScheduledJob);
        }

        public ActionResult Create()
        {
            var vm = new ScheduledJobViewModel
            {
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScheduledJobViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var jobInfo = new JobInfoModel
                {
                    JobType = "Scheduled",
                    CronExpression = vm.CronExpression,
                    Url = vm.Url
                };

                var newScheduledJob = new ScheduledJob
                {
                    Name = vm.Name,
                    JobInfo = JsonConvert.SerializeObject(jobInfo)
                };

                _db.ScheduledJobs.Add(newScheduledJob);
                _db.SaveChanges();

                try
                {
                    // Create Hangfire Job
                    RecurringJob.AddOrUpdate(vm.Name, () => WebClientHelper.InvokeUrl(vm.Url), vm.CronExpression);
                    return RedirectToAction("Index");
                }
                catch(Exception ex) {
                    ViewBag.jobException = "Invalid cron expression" ;
                }
            }

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            ScheduledJob tenantScheduledJob = _db.ScheduledJobs.Find(id);
            if (tenantScheduledJob == null)
            {
                return HttpNotFound();
            }

            return View(tenantScheduledJob);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScheduledJob tenantScheduledJob = _db.ScheduledJobs.Find(id);

            if (tenantScheduledJob != null)
            {
                // Delete Hangfire Jobb
                RecurringJob.RemoveIfExists(tenantScheduledJob.Name);
            }

            // Remove Job
            _db.ScheduledJobs.Remove(tenantScheduledJob);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}