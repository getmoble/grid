using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;
using Grid.Features.IMS.ViewModels;
using Grid.Providers.Blob;
using Grid.Features.Settings.Services.Interfaces;

namespace Grid.Areas.IMS.Controllers
{
    public class AssetDocumentsController : InventoryBaseController
    {
        private readonly IAssetDocumentRepository _assetDocumentRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;

        public AssetDocumentsController(IAssetDocumentRepository assetDocumentRepository,
                                        ISettingsService settingsService,
                                        IUnitOfWork unitOfWork)
        {
            _assetDocumentRepository = assetDocumentRepository;
            _unitOfWork = unitOfWork;
            _settingsService = settingsService;
        }

        public ActionResult Index()
        {
            var assetDocuments = _assetDocumentRepository.GetAll("Asset");
            return View(assetDocuments.ToList());
        }

        public ActionResult Details(int id)
        {
            var assetDocument = _assetDocumentRepository.Get(id);
            return CheckForNullAndExecute(assetDocument, () => View(assetDocument));
        }

        public ActionResult Create(int assetId)
        {
            var vm = new AssetDocumentViewModel
            {
                AssetId = assetId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AssetDocumentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newDocument = new AssetDocument
                {
                    AssetId = vm.AssetId,
                    DocumentType = vm.DocumentType,
                    FileSize = 0.0
                };

                if (vm.Document != null)
                {
                    var siteSettings = _settingsService.GetSiteSettings();
                    var blobUploadService = new BlobUploadService(siteSettings.BlobSettings);
                    var blobPath = blobUploadService.UploadITDocument(vm.Document);
                    newDocument.DocumentPath = blobPath;
                    newDocument.FileName = vm.Document.FileName;
                }

                _assetDocumentRepository.Create(newDocument);
                _unitOfWork.Commit();

                return RedirectToAction("Details", "Assets", new { id = vm.AssetId });
            }

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var assetDocument = _assetDocumentRepository.Get(id);
            return CheckForNullAndExecute(assetDocument, () => View(assetDocument));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var assetDocument = _assetDocumentRepository.Get(id);
            _assetDocumentRepository.Delete(assetDocument);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Assets", new { id = assetDocument.AssetId });
        }
    }
}
