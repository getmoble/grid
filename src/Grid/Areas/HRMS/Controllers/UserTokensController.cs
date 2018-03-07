using System;
using System.Web.Mvc;
using Grid.Features.Auth.DAL.Interfaces;
using Grid.Features.Auth.Entities;
using Grid.Features.Common;
using Grid.Infrastructure;

namespace Grid.Areas.HRMS.Controllers
{
    public class UserTokensController : GridBaseController
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserTokensController(ITokenRepository tokenRepository,
                                    IUnitOfWork unitOfWork)
        {
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Create(int userId)
        {
            var token = new Token
            {
                AllocatedToUserId = userId,
                ExpiresOn = DateTime.UtcNow.AddDays(30),
                Key = Guid.NewGuid().ToString("N")
            };

            return View(token);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Token token)
        {
            if (ModelState.IsValid)
            {
                _tokenRepository.Create(token);
                _unitOfWork.Commit();
                return RedirectToAction("Details", "Users", new { id = token.AllocatedToUserId });
            }

            return View(token);
        }

        public ActionResult Delete(int id)
        {
            var token = _tokenRepository.Get(id);
            return CheckForNullAndExecute(token, () => View(token));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var token = _tokenRepository.Get(id);
            _tokenRepository.Delete(token);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Users", new { id = token.AllocatedToUserId });
        }
    }
}
