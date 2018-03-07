using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.KBS.DAL.Interfaces;
using Grid.Features.KBS.Entities;

namespace Grid.Areas.KBS.Controllers
{
    public class KeywordsController : KnowledgeBaseController
    {
        private readonly IKeywordRepository _keywordRepository;
        private readonly IUnitOfWork _unitOfWork;

        public KeywordsController(IKeywordRepository awardRepository,
                                  IUnitOfWork unitOfWork)
        {
            _keywordRepository = awardRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var keywords = _keywordRepository.GetAll();
            return View(keywords);
        }

        public ActionResult Details(int id)
        {
            var keyword = _keywordRepository.Get(id);
            return CheckForNullAndExecute(keyword, () => View(keyword));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Keyword keyword)
        {
            if (ModelState.IsValid)
            {
                _keywordRepository.Create(keyword);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(keyword);
        }

        public ActionResult Edit(int id)
        {
            var keyword = _keywordRepository.Get(id);
            return CheckForNullAndExecute(keyword, () => View(keyword));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Keyword keyword)
        {
            if (ModelState.IsValid)
            {
                _keywordRepository.Update(keyword);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(keyword);
        }

        public ActionResult Delete(int id)
        {
            var keyword = _keywordRepository.Get(id);
            return CheckForNullAndExecute(keyword, () => View(keyword));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _keywordRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
