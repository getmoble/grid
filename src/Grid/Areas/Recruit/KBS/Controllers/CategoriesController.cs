using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.KBS.DAL.Interfaces;
using Grid.Features.KBS.Entities;
using Grid.Filters;

namespace Grid.Areas.KBS.Controllers
{
    public class CategoriesController : KnowledgeBaseController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(ICategoryRepository categoryRepository,
                                  IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var categories = _categoryRepository.GetAll("CreatedByUser,ParentCategory,UpdatedByUser");
            return View(categories.ToList());
        }

        public ActionResult Details(int id)
        {
            var category = _categoryRepository.Get(id);
            return CheckForNullAndExecute(category, () => View(category));
        }

        [SelectList("ArticleCategory", "ParentCategoryId")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("ArticleCategory", "ParentCategoryId", SelectListState.Recreate)]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.CreatedByUserId = WebUser.Id;
                _categoryRepository.Create(category);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int id)
        {
            var category = _categoryRepository.Get(id);
            ViewBag.ParentCategoryId = new SelectList(_categoryRepository.GetAll(), "Id", "Title", category.ParentCategoryId);
            return CheckForNullAndExecute(category, () => View(category));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("ArticleCategory", "ParentCategoryId", SelectListState.Recreate)]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.UpdatedByUserId = WebUser.Id;

                _categoryRepository.Update(category);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Delete(int id)
        {
            var category = _categoryRepository.Get(id);
            return CheckForNullAndExecute(category, () => View(category));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _categoryRepository.Delete(id);
            _unitOfWork.Commit();

            return RedirectToAction("Index");
        }
    }
}
