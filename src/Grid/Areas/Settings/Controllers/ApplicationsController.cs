using System;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Settings.DAL.Interfaces;
using Grid.Features.Settings.Entities;
using Grid.Infrastructure;
using Grid.Providers.Plugin;

namespace Grid.Areas.Settings.Controllers
{
    public class ApplicationsController : GridBaseController
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationsController(IApplicationRepository applicationRepository,
                                      IUnitOfWork unitOfWork)
        {
            _applicationRepository = applicationRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var apps = _applicationRepository.GetAll();
            return View(apps);
        }

        public ActionResult Sync()
        {
            return View();
        }

        [HttpPost, ActionName("Sync")]
        public ActionResult SyncConfirmed()
        {
            // Remove Existing
            var existingApps = _applicationRepository.GetAll();
            foreach (var app in existingApps)
            {
                _applicationRepository.Delete(app);
            }
            _unitOfWork.Commit();

            // Refresh

            var type = typeof(IGridApplication);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && type.IsAssignableFrom(p));
            foreach (var selectedType in types)
            {
                var instance = (IGridApplication)Activator.CreateInstance(selectedType);
                var appInfo = instance.GetApplicationInfo();
                var newApp = new Application
                {
                    Category = appInfo.Category,
                    Title = appInfo.Title,
                    Description = appInfo.Description,
                    Icon = appInfo.Icon,
                    Url = appInfo.Url
                };

                _applicationRepository.Create(newApp);
            }
            _unitOfWork.Commit();

            return RedirectToAction("Index");
        }
    }
}