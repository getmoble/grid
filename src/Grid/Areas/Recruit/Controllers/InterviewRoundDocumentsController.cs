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
    public class InterviewRoundDocumentsController : RecruitBaseController
    {
        private readonly IInterviewRoundDocumentRepository _interviewRoundDocumentRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;

        public InterviewRoundDocumentsController(IInterviewRoundDocumentRepository interviewRoundDocumentRepository,
                                            ISettingsService settingsService,
                                            IUnitOfWork unitOfWork)
        {
            _interviewRoundDocumentRepository = interviewRoundDocumentRepository;
            _unitOfWork = unitOfWork;
            _settingsService = settingsService;
        }

        public ActionResult Index()
        {
            var interviewRoundDocs = _interviewRoundDocumentRepository.GetAll();
            return View(interviewRoundDocs.ToList());
        }

        public ActionResult Details(int id)
        {
            var interviewRoundDocument = _interviewRoundDocumentRepository.Get(id);
            return CheckForNullAndExecute(interviewRoundDocument, () => View(interviewRoundDocument));
        }

        public ActionResult Create(int interviewRoundId)
        {
            var vm = new InterviewRoundDocumentViewModel
            {
                InterviewRoundId = interviewRoundId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InterviewRoundDocumentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newDocument = new InterviewRoundDocument()
                {
                    InterviewRoundId = vm.InterviewRoundId,
                    DocumentType = vm.DocumentType,
                    FileSize = 0.0
                };

                if (vm.Document != null)
                {
                    var siteSettings = _settingsService.GetSiteSettings();
                    var blobUploadService = new BlobUploadService(siteSettings.BlobSettings);
                    var blobPath = blobUploadService.UploadInterviewRoundDocument(vm.Document);
                    newDocument.DocumentPath = blobPath;
                    newDocument.FileName = vm.Document.FileName;
                }

                _interviewRoundDocumentRepository.Create(newDocument);
                _unitOfWork.Commit();

                return RedirectToAction("Details", "InterviewRounds", new { id = vm.InterviewRoundId });
            }

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var interviewRoundDocument = _interviewRoundDocumentRepository.Get(id);
            return CheckForNullAndExecute(interviewRoundDocument, () => View(interviewRoundDocument));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _interviewRoundDocumentRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}