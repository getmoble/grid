using System.Linq;
using System.Web.Mvc;
using Grid.Features.KBS.DAL.Interfaces;
using Grid.Features.KBS.Entities.Enums;
using Grid.Infrastructure.Filters;

namespace Grid.Api.Controllers
{
    public class ArticleController: GridApiBaseController
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult Index()
        {
            var articles = _articleRepository.GetAllBy(a => a.State == ArticleState.Published).ToList();
            return Json(articles, JsonRequestBehavior.AllowGet);
        }
    }
}
