using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.KBS.DAL.Interfaces;
using Grid.Features.KBS.Entities;
using Grid.Features.KBS.ViewModels;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Providers.Blob;

namespace Grid.Areas.KBS.Controllers
{
    public class ArticleAttachmentsController : KnowledgeBaseController
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleAttachmentRepository _articleAttachmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;

        public ArticleAttachmentsController(IArticleAttachmentRepository articleAttachmentRepository,
                                            IArticleRepository articleRepository,
                                            ISettingsService settingsService,
                                            IUnitOfWork unitOfWork)
        {
            _articleAttachmentRepository = articleAttachmentRepository;
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
            _settingsService = settingsService;
        }

        public ActionResult Index()
        {
            var articleAttachments = _articleAttachmentRepository.GetAll("Article");
            return View(articleAttachments.ToList());
        }

        public ActionResult Create(int articleId)
        {
            var vm = new ArticleAttachmentViewModel { ArticleId = articleId };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleAttachmentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newAttachment = new ArticleAttachment
                {
                    ArticleId = vm.ArticleId,
                    AttachmentType = vm.AttachmentType,
                    FileSize = 0.0
                };

                if (vm.Document != null)
                {
                    var siteSettings = _settingsService.GetSiteSettings();
                    var blobUploadService = new BlobUploadService(siteSettings.BlobSettings);
                    var blobPath = blobUploadService.UploadArticleAttachment(vm.Document);
                    newAttachment.DocumentPath = blobPath;
                    newAttachment.FileName = vm.Document.FileName;
                }

                _articleAttachmentRepository.Create(newAttachment);
                _unitOfWork.Commit();

                return RedirectToAction("Edit", "Articles", new { id = vm.ArticleId });
            }

            ViewBag.ArticleId = new SelectList(_articleRepository.GetAll(), "Id", "Title", vm.ArticleId);

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var articleAttachment = _articleAttachmentRepository.Get(id);
            return CheckForNullAndExecute(articleAttachment, () => View(articleAttachment));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _articleAttachmentRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
