using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.HRMS.Controllers
{
    [GridPermission(PermissionCode = 210)]
    public class UserSkillsController : GridBaseController
    {
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserSkillsController(IUserSkillRepository userSkillRepository,
                                    IUnitOfWork unitOfWork)
        {
            _userSkillRepository = userSkillRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Delete(int id)
        {
            var userSkill = _userSkillRepository.Get(id, "User.Person");
            return CheckForNullAndExecute(userSkill, () => View(userSkill));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userSkill = _userSkillRepository.Get(id);
            _userSkillRepository.Delete(userSkill);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Users", new { id = userSkill.UserId});
        }
    }
}
