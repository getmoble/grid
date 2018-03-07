using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Data.MultiTenancy;
using Grid.Data.MultiTenancy.Entities;
using Grid.TenantManagement.Models;
using Grid.TenantManagement.ViewModels;
using Hangfire;
using Newtonsoft.Json;

namespace Grid.TenantManagement.Controllers
{
    public class TenantScheduledJobsController : Controller
    {
        private TenantDataContext db = new TenantDataContext();

        public ActionResult Index()
        {
            var tenantScheduledJobs = db.TenantScheduledJobs.Include(t => t.Tenant);
            return View(tenantScheduledJobs.ToList());
        }

        public ActionResult Details(int id)
        {
            TenantScheduledJob tenantScheduledJob = db.TenantScheduledJobs.Find(id);
            if (tenantScheduledJob == null)
            {
                return HttpNotFound();
            }
            return View(tenantScheduledJob);
        }

        public ActionResult Create(int tenantId)
        {
            var vm = new TenantScheduledJobViewModel
            {
                TenantId = tenantId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TenantScheduledJobViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var jobInfo = new JobInfoModel
                {
                    JobType = "Scheduled",
                    CronExpression = vm.CronExpression,
                    Url = vm.Url
                };

                var newScheduledJob = new TenantScheduledJob
                {
                    TenantId = vm.TenantId,
                    Name = vm.Name,
                    JobInfo = JsonConvert.SerializeObject(jobInfo)
                };

                db.TenantScheduledJobs.Add(newScheduledJob);
                db.SaveChanges();


                // Create Hangfire Job
                var selectedTenant = db.Tenants.FirstOrDefault(t => t.Id == vm.TenantId);
                if (selectedTenant != null)
                {
                    RecurringJob.AddOrUpdate(vm.Name, ()=> WebClientHelper.InvokeUrl(vm.Url), vm.CronExpression);
                }
                
                return RedirectToAction("Details", "Tenants", new {id = vm.TenantId});
            }

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            TenantScheduledJob tenantScheduledJob = db.TenantScheduledJobs.Find(id);
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
            TenantScheduledJob tenantScheduledJob = db.TenantScheduledJobs.Find(id);

            if (tenantScheduledJob != null)
            {
                // Delete Hangfire Jobb
                RecurringJob.RemoveIfExists(tenantScheduledJob.Name);
            }
            
            // Remove Job
            db.TenantScheduledJobs.Remove(tenantScheduledJob);
            db.SaveChanges();

            return RedirectToAction("Details", "Tenants", new { id = tenantScheduledJob.TenantId });
        }
    }
}
