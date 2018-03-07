using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.KBS.DAL.Interfaces;
using Grid.Features.KBS.Entities;
using Grid.Features.KBS.Entities.Enums;
using Grid.Features.KBS.ViewModels;
using Grid.Filters;
using Grid.Infrastructure.Filters;
using Newtonsoft.Json;

namespace Grid.Areas.KBS.Controllers
{
    [GridPermission(PermissionCode = 300)]
    public class ArticlesController : KnowledgeBaseController
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleAttachmentRepository _articleAttachmentRepository;
        private readonly IArticleVersionRepository _articleVersionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ArticlesController(IArticleRepository articleRepository,
                                  IArticleAttachmentRepository articleAttachmentRepository,
                                  IArticleVersionRepository articleVersionRepository,
                                  ICategoryRepository categoryRepository,
                                  IUnitOfWork unitOfWork)
        {
            _articleRepository = articleRepository;
            _articleAttachmentRepository = articleAttachmentRepository;
            _articleVersionRepository = articleVersionRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        // Exposed to Everyone in KBS
        [SelectList("ArticleCategory", "CategoryId")]
        public ActionResult Index(ArticleSearchViewModel vm)
        {
            Func<IQueryable<Article>, IQueryable<Article>> articleFilter = q =>
            {
                q = q.Include(a => a.Category)
                             .Include(a => a.CreatedByUser)
                             .Include(a => a.UpdatedByUser).Where(a => a.State == ArticleState.Published);

                if (vm.CategoryId.HasValue)
                {
                    q = q.Where(r => r.CategoryId == vm.CategoryId.Value);
                }

                if (!string.IsNullOrEmpty(vm.Title))
                {
                    q = q.Where(r => r.Title.Contains(vm.Title));
                }

                return q;
            };

            vm.Articles = _articleRepository.SearchPage(articleFilter, o => o.OrderByDescending(c => c.UpdatedOn), vm.GetPageNo(), vm.PageSize);

            return View(vm);
        }

        public ActionResult Manage()
        {
            if (WebUser.IsAdmin || WebUser.Permissions.Contains(700))
            {
                var articles = _articleRepository.GetAll("Category,CreatedByUser,UpdatedByUser");
                return View(articles.ToList());
            }
            else
            {
                var articles = _articleRepository.GetAllBy(a => a.CreatedByUserId == WebUser.Id, "Category,CreatedByUser,UpdatedByUser");
                return View(articles.ToList());
            }
        }

        public ActionResult Details(int id)
        {
            var article = _articleRepository.GetBy(a => a.Id == id, "Category,CreatedByUser.Person,UpdatedByUser.Person");

            // Update Hit Count 
            article.Hits = article.Hits + 1;
            _articleRepository.Update(article);
            _unitOfWork.Commit();

            return CheckForNullAndExecute(article, () => View(article));
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_categoryRepository.GetAll(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewArticleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newArticle = new Article
                {
                    CategoryId = vm.CategoryId,
                    IsPublic = vm.IsPublic,
                    Title = vm.Title,
                    ArticleVersion = vm.ArticleVersion,
                    Summary = vm.Summary,
                    Content = vm.Content,
                    KeyWords = vm.KeyWords,
                    IsFeatured = vm.IsFeatured,
                    CreatedByUserId = WebUser.Id,
                    UpdatedByUserId = WebUser.Id,
                    Version = 1
                };

                _articleRepository.Create(newArticle);
                _unitOfWork.Commit();

                return RedirectToAction("Manage");
            }

            ViewBag.CategoryId = new SelectList(_categoryRepository.GetAll(), "Id", "Title", vm.CategoryId);
            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var article = _articleRepository.Get(id);

            var attachments = _articleAttachmentRepository.GetAllBy(a => a.ArticleId == article.Id).ToList();
            var versions = _articleVersionRepository.GetAllBy(a => a.ArticleId == article.Id, o => o.OrderByDescending(a => a.CreatedOn)).ToList();

            var vm = new EditArticleViewModel(article)
            {
                Attachments = attachments,
                Versions = versions
            };

            ViewBag.CategoryId = new SelectList(_categoryRepository.GetAll(), "Id", "Title", article.CategoryId);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditArticleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var article = _articleRepository.Get(vm.Id);

                //Create a version 
                var newVersion = new ArticleVersion
                {
                    ArticleId = article.Id,
                    Version = article.Version,
                    CreatedByUserId = WebUser.Id,
                    Content = JsonConvert.SerializeObject(article)
                };

                _articleVersionRepository.Create(newVersion);
                _unitOfWork.Commit();

                article.Title = vm.Title;
                article.ArticleVersion = vm.ArticleVersion;
                article.IsPublic = vm.IsPublic;
                article.Summary = vm.Summary;
                article.Content = vm.Content;
                article.KeyWords = vm.KeyWords;
                article.IsFeatured = vm.IsFeatured;
                article.State = vm.State;
                article.CategoryId = vm.CategoryId;
                article.Version = article.Version + 1;
                article.UpdatedByUserId = WebUser.Id;

                _articleRepository.Update(article);
                _unitOfWork.Commit();

                return RedirectToAction("Manage");
            }

            // Not using SelectList as we are getting an exception for the Html Content like potentially dangerours
            ViewBag.CategoryId = new SelectList(_categoryRepository.GetAll(), "Id", "Title", vm.CategoryId);
            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var article = _articleRepository.GetBy(a => a.Id == id, "Category,CreatedByUser.Person,UpdatedByUser.Person");
            return CheckForNullAndExecute(article, () => View(article));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _articleRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
