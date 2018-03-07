using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.HRMS.Controllers
{
    [GridPermission(PermissionCode = 210)]
    public class UserCertificationsController : GridBaseController
    {
        private readonly IUserCertificationRepository _userCertificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserCertificationsController(IUserCertificationRepository userCertificationRepository,
                                            IUnitOfWork unitOfWork)
        {
            _userCertificationRepository = userCertificationRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Delete(int id)
        {
            var userCertification = _userCertificationRepository.Get(id, "User.Person");
            return CheckForNullAndExecute(userCertification, () => View(userCertification));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userCertification = _userCertificationRepository.Get(id, "User.Person");
            _userCertificationRepository.Delete(userCertification);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Users", new { id = userCertification.UserId });
        }
    }
}
