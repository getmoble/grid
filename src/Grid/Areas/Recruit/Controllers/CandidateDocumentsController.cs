using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.ViewModels;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Providers.Blob;

namespace Grid.Areas.Recruit.Controllers
{
    public class CandidateDocumentsController : RecruitBaseController
    {
        private readonly ICandidateDocumentRepository _candidateDocumentRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;

        public CandidateDocumentsController(ICandidateDocumentRepository candidateDocumentRepository,
                                            ISettingsService settingsService,
                                            IUnitOfWork unitOfWork)
        {
            _candidateDocumentRepository = candidateDocumentRepository;
            _unitOfWork = unitOfWork;
            _settingsService = settingsService;
        }

        public ActionResult Index()
        {
            var candidateDocuments = _candidateDocumentRepository.GetAll("Candidate");
            return View(candidateDocuments.ToList());
        }

        public ActionResult Details(int id)
        {
            var candidateDocument = _candidateDocumentRepository.Get(id);
            return CheckForNullAndExecute(candidateDocument, () => View(candidateDocument));
        }

        public ActionResult Create(int candidateId)
        {
            var vm = new CandidateDocumentViewModel
            {
                CandidateId = candidateId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CandidateDocumentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newDocument = new CandidateDocument
                {
                    CandidateId = vm.CandidateId,
                    DocumentType = vm.DocumentType,
                    FileSize = 0.0
                };

                if (vm.Document != null)
                {
                    var siteSettings = _settingsService.GetSiteSettings();
                    var blobUploadService = new BlobUploadService(siteSettings.BlobSettings);
                    var blobPath = blobUploadService.UploadRecruitDocument(vm.Document);
                    newDocument.DocumentPath = blobPath;
                    newDocument.FileName = vm.Document.FileName;
                }

                _candidateDocumentRepository.Create(newDocument);
                _unitOfWork.Commit();

                return RedirectToAction("Details", "Candidates", new { id = vm.CandidateId });
            }

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var candidateDocument = _candidateDocumentRepository.Get(id);
            return CheckForNullAndExecute(candidateDocument, () => View(candidateDocument));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _candidateDocumentRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
