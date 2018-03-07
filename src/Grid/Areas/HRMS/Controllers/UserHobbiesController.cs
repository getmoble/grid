using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.HRMS.Controllers
{
    [GridPermission(PermissionCode = 210)]
    public class UserHobbiesController : GridBaseController
    {
        private readonly IUserHobbyRepository _userHobbyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserHobbiesController(IUserHobbyRepository userHobbyRepository,
                                     IUnitOfWork unitOfWork)
        {
            _userHobbyRepository = userHobbyRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Delete(int id)
        {
            var userHobby = _userHobbyRepository.Get(id);
            return CheckForNullAndExecute(userHobby, () => View(userHobby));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userHobby = _userHobbyRepository.Get(id);
            _userHobbyRepository.Delete(userHobby);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Users", new { id = userHobby.UserId });
        }
    }
}
