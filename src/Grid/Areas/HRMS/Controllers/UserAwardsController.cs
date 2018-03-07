using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities;
using Grid.Filters;

namespace Grid.Areas.HRMS.Controllers
{
    public class UserAwardsController : GridBaseController
    {
        private readonly IUserAwardRepository _userAwardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserAwardsController(IUserAwardRepository userAwardRepository,
                                    IUnitOfWork unitOfWork)
        {
            _userAwardRepository = userAwardRepository;
            _unitOfWork = unitOfWork;
        }

        [SelectList("Award", "AwardId")]
        public ActionResult Create(int userId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectList("Award", "AwardId", SelectListState.Recreate)]
        public ActionResult Create(UserAward userAward)
        {
            if (ModelState.IsValid)
            {
                _userAwardRepository.Create(userAward);
                _unitOfWork.Commit();
                return RedirectToAction("Details", "Users", new { id = userAward.UserId });
            }
            return View(userAward);
        }

        public ActionResult Delete(int id)
        {
            var award = _userAwardRepository.Get(id);
            return CheckForNullAndExecute(award, () => View(award));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userAward = _userAwardRepository.Get(id);
            _userAwardRepository.Delete(userAward);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Users", new { id = userAward.UserId });
        }
    }
}
