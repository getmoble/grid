using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Settings.DAL.Interfaces;
using Grid.Features.Settings.Entities;
using Grid.Infrastructure;

namespace Grid.Areas.Settings.Controllers
{
    public class GridUpdatesController : GridBaseController
    {
        private readonly IGridUpdateRepository _gridUpdateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GridUpdatesController(IGridUpdateRepository gridUpdateRepository,
                                     IUnitOfWork unitOfWork)
        {
            _gridUpdateRepository = gridUpdateRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var gridUpdates = _gridUpdateRepository.GetAllBy(o => o.OrderByDescending(u => u.Date));
            return View(gridUpdates);
        }

        public ActionResult Details(int id)
        {
            var gridUpdate = _gridUpdateRepository.Get(id);
            return CheckForNullAndExecute(gridUpdate, () => View(gridUpdate));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GridUpdate gridUpdate)
        {
            if (ModelState.IsValid)
            {
                _gridUpdateRepository.Create(gridUpdate);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(gridUpdate);
        }

        public ActionResult Edit(int id)
        {
            var gridUpdate = _gridUpdateRepository.Get(id);
            return CheckForNullAndExecute(gridUpdate, () => View(gridUpdate));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GridUpdate gridUpdate)
        {
            if (ModelState.IsValid)
            {
                _gridUpdateRepository.Update(gridUpdate);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(gridUpdate);
        }

        public ActionResult Delete(int id)
        {
            var gridUpdate = _gridUpdateRepository.Get(id);
            return CheckForNullAndExecute(gridUpdate, () => View(gridUpdate));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _gridUpdateRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
