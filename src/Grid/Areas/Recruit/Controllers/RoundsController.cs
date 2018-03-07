using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Areas.Recruit.Controllers
{
    public class RoundsController : RecruitBaseController
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoundsController(IRoundRepository roundRepository,
                                IUnitOfWork unitOfWork)
        {
            _roundRepository = roundRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var rounds = _roundRepository.GetAll();
            return View(rounds);
        }

        public ActionResult Details(int id)
        {
            var round = _roundRepository.Get(id);
            return CheckForNullAndExecute(round, () => View(round));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Round round)
        {
            if (ModelState.IsValid)
            {
                _roundRepository.Create(round);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(round);
        }

        public ActionResult Edit(int id)
        {
            var round = _roundRepository.Get(id);
            return CheckForNullAndExecute(round, () => View(round));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Round round)
        {
            if (ModelState.IsValid)
            {
                _roundRepository.Update(round);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(round);
        }

        public ActionResult Delete(int id)
        {
            var round = _roundRepository.Get(id);
            return CheckForNullAndExecute(round, () => View(round));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _roundRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
