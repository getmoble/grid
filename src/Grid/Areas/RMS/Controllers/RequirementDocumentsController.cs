using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Entities;
using Grid.Features.RMS.ViewModels;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Infrastructure;
using Grid.Providers.Blob;

namespace Grid.Areas.RMS.Controllers
{
    public class RequirementDocumentsController : GridBaseController
    {
        private readonly IRequirementDocumentRepository _requirementDocumentRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;

        public RequirementDocumentsController(IRequirementDocumentRepository requirementDocumentRepository,
                                              ISettingsService settingsService,
                                              IUnitOfWork unitOfWork)
        {
            _requirementDocumentRepository = requirementDocumentRepository;
            _unitOfWork = unitOfWork;
            _settingsService = settingsService;
        }

        public ActionResult Index()
        {
            var requirementDocuments = _requirementDocumentRepository.GetAll("Requirement");
            return View(requirementDocuments.ToList());
        }

        public ActionResult Details(int id)
        {
            var requirementDocument = _requirementDocumentRepository.Get(id);
            return CheckForNullAndExecute(requirementDocument, () => View(requirementDocument));
        }

        public ActionResult Create(int requirementId)
        {
            var vm = new RequirementDocumentViewModel
            {
                RequirementId = requirementId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequirementDocumentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newDocument = new RequirementDocument
                {
                    RequirementId = vm.RequirementId,
                    DocumentType = vm.DocumentType,
                    FileSize = 0.0
                };

                if (vm.Document != null)
                {
                    var siteSettings = _settingsService.GetSiteSettings();
                    var blobUploadService = new BlobUploadService(siteSettings.BlobSettings);
                    var blobPath = blobUploadService.UploadRMSDocument(vm.Document);
                    newDocument.DocumentPath = blobPath;
                    newDocument.FileName = vm.Document.FileName;
                }

                _requirementDocumentRepository.Create(newDocument);
                _unitOfWork.Commit();
                return RedirectToAction("Details", "Requirements", new { id = vm.RequirementId });
            }

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var requirementDocument = _requirementDocumentRepository.Get(id);
            return CheckForNullAndExecute(requirementDocument, () => View(requirementDocument));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var requirementDocument = _requirementDocumentRepository.Get(id);
            _requirementDocumentRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Requirements", new { id = requirementDocument.RequirementId });
        }
    }
}
