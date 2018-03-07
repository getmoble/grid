using System;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.KBS.DAL.Interfaces;
using Grid.Features.KBS.Entities;
using Newtonsoft.Json;

namespace Grid.Api.Controllers
{
    public class ArticlesController: GridApiBaseController
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ArticlesController(IArticleRepository articleRepository,
                                   IUnitOfWork unitOfWork)
        {
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
        }
    
        public ContentResult Index()
        {
            var apiResult = TryExecute(() => _articleRepository.GetAll(), "Articles Fetched sucessfully");

            var list = JsonConvert.SerializeObject(apiResult,
            Formatting.Indented,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Content(list, "application/json");
        }
      
        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _articleRepository.Get(id), "Article fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Article vm)
        {
            ApiResult<Article> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var article = _articleRepository.Get(vm.Id);
                        article.CategoryId = vm.CategoryId;
                        article.IsPublic = vm.IsPublic;
                        article.Title = article.Title;
                        article.Summary = article.Summary;
                        article.Content = article.Content;
                        article.ArticleVersion = article.ArticleVersion;
                        article.KeyWords = article.KeyWords;
                        article.IsFeatured = article.IsFeatured;
                        article.UpdatedByUserId = WebUser.Id;
                        article.UpdatedOn = DateTime.UtcNow;

                        _articleRepository.Update(article);
                        _unitOfWork.Commit();
                        return article;
                    }, "Article updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        vm.CreatedByUserId = WebUser.Id;
                        _articleRepository.Create(vm);
                        _unitOfWork.Commit();
                        return vm;
                    }, "Article created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Article>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _articleRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Article deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
