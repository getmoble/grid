using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Feedback.DAL.Interfaces;
using Grid.Features.Feedback.Entities;
using Grid.Features.Settings.ViewModels;
using Grid.Infrastructure;

namespace Grid.Areas.Settings.Controllers
{
    public class UserFeedbacksController : GridBaseController
    {
        private readonly IUserFeedbackRepository _userFeedbackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserFeedbacksController(IUserFeedbackRepository userFeedbackRepository,
                                       IUnitOfWork unitOfWork)
        {
            _userFeedbackRepository = userFeedbackRepository;
            _unitOfWork = unitOfWork;
        }

        #region Ajax Calls
        [HttpPost]
        public ActionResult Create(CreateUserFeedbackViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newFeedback = new UserFeedback
                {
                    Screenshot = vm.Screenshot,
                    Content = vm.Comment,
                    CreatedByUserId = WebUser.Id,
                };

                _userFeedbackRepository.Create(newFeedback);
                _unitOfWork.Commit();

                return Json(true);
            }

            return Json(false);
        }
        #endregion

        public ActionResult Index()
        {
            var userFeedbacks = _userFeedbackRepository.GetAllBy(o => o.OrderByDescending(f => f.CreatedOn), "CreatedByUser.Person,UpdatedByUser");
            return View(userFeedbacks.ToList());
        }

        public ActionResult Details(int id)
        {
            var userFeedback = _userFeedbackRepository.GetBy(f => f.Id == id, "CreatedByUser.Person");
            return CheckForNullAndExecute(userFeedback, () => View(userFeedback));
        }

        public ActionResult Delete(int id)
        {
            var userFeedback = _userFeedbackRepository.Get(id);
            return CheckForNullAndExecute(userFeedback, () => View(userFeedback));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _userFeedbackRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
