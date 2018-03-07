using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Providers.Blob;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.ViewModels;
using Grid.Features.Settings.Services.Interfaces;

namespace Grid.Areas.HRMS.Controllers
{
    public class UserDocumentsController : GridBaseController
    {
        private readonly IUserDocumentRepository _userDocumentRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;
        public UserDocumentsController(IUserDocumentRepository userDocumentRepository,
                                       IUnitOfWork unitOfWork,
                                       ISettingsService settingsService)
        {
            _userDocumentRepository = userDocumentRepository;
            _unitOfWork = unitOfWork;
            _settingsService = settingsService;
        }

        public ActionResult Index()
        {
            var userDocuments = _userDocumentRepository.GetAll("User").ToList();
            return View(userDocuments);
        }

        public ActionResult Details(int id)
        {
            var userDocument = _userDocumentRepository.Get(id);
            return CheckForNullAndExecute(userDocument, () => View(userDocument));
        }

        public ActionResult Create(int userId)
        {
            var vm = new UserDocumentViewModel
            {
                UserId = userId
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserDocumentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newDocument = new UserDocument
                {
                    UserId = vm.UserId,
                    DocumentType = vm.DocumentType,
                    FileSize = 0.0
                };

                if (vm.Document != null)
                {
                    var siteSettings = _settingsService.GetSiteSettings();
                    var blobUploadService = new BlobUploadService(siteSettings.BlobSettings);
                    var blobPath = blobUploadService.UploadHRDocument(vm.Document);
                    newDocument.DocumentPath = blobPath;
                    newDocument.FileName = vm.Document.FileName;
                }

                _userDocumentRepository.Create(newDocument);
                _unitOfWork.Commit();

                return RedirectToAction("Details", "Users", new { id = vm.UserId });
            }

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var userDocument = _userDocumentRepository.Get(id);
            return CheckForNullAndExecute(userDocument, () => View(userDocument));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userDocument = _userDocumentRepository.Get(id);
            _userDocumentRepository.Delete(userDocument);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Users", new { id = userDocument.UserId });
        }
    }
}
