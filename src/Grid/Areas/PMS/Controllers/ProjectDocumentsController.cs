using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.ViewModels;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Providers.Blob;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.PMS.Controllers
{
    [GridPermission(PermissionCode = 300)]
    public class ProjectDocumentsController : ProjectsBaseController
    {
        private readonly IProjectDocumentRepository _projectDocumentRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;

        public ProjectDocumentsController(IProjectDocumentRepository projectDocumentRepository,
                                          IProjectRepository projectRepository,
                                          ISettingsService settingsService,
                                          IUnitOfWork unitOfWork)
        {
            _projectDocumentRepository = projectDocumentRepository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _settingsService = settingsService;
        }

        public ActionResult Create(int projectId)
        {
            var vm = new ProjectDocumentViewModel
            {
                ProjectId = projectId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectDocumentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newDocument = new ProjectDocument
                {
                    ProjectId = vm.ProjectId,
                    DocumentType = vm.DocumentType,
                    FileSize = 0.0
                };

                if (vm.Document != null)
                {
                    var siteSettings = _settingsService.GetSiteSettings();
                    var blobUploadService = new BlobUploadService(siteSettings.BlobSettings);
                    var blobPath = blobUploadService.UploadProjectDocument(vm.Document);
                    newDocument.DocumentPath = blobPath;
                    newDocument.FileName = vm.Document.FileName;
                }

                _projectDocumentRepository.Create(newDocument);
                _unitOfWork.Commit();

                return RedirectToAction("Details", "Projects", new { Id = vm.ProjectId });
            }

            ViewBag.ProjectId = new SelectList(_projectRepository.GetAll(), "Id", "Title", vm.ProjectId);
            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var projectDocument = _projectDocumentRepository.Get(id);
            if (projectDocument == null)
            {
                return HttpNotFound();
            }
            return View(projectDocument);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var projectDocument = _projectDocumentRepository.Get(id);
            _projectDocumentRepository.Delete(projectDocument);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Projects", new { Id = projectDocument.ProjectId });
        }
    }
}
