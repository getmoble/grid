using System;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Auth.DAL.Interfaces;
using Grid.Features.Auth.Entities;
using Grid.Features.Auth.Entities.Enums;
using Grid.Features.Common;
using Grid.Features.Common.Models;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;

namespace Grid.Api.Controllers
{
    public class AccountController : GridApiBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUserRepository userRepository,
                                 ITokenRepository tokenRepository,
                                 IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.Email))
            {
                return Json(new AuthResult
                {
                    Status = false,
                    Message = "Email is Empty"
                });
            }


            if (string.IsNullOrEmpty(vm.Password))
            {
                return Json(new AuthResult
                {
                    Status = false,
                    Message = "Password is Empty"
                });
            }

            var user = _userRepository.GetBy(u => u.Username == vm.Email, "AccessRule,Person");

            if (user == null)
            {
                return Json(new AuthResult
                {
                    Status = false,
                    Message = "Employee not found"
                });
            }

            if (user.AccessRule == null)
            {
                return Json(new AuthResult
                {
                    Status = false,
                    Message = "Employee Access is not enabled"
                });
            }

            if (!user.AccessRule.IsApproved)
            {
                return Json(new AuthResult
                {
                    Status = false,
                    Message = "Employee is not approved"
                });
            }

            var hashedPassword = user.Password;
            var verificationSucceeded = hashedPassword != null && HashHelper.CheckHash(vm.Password, hashedPassword);

            if (verificationSucceeded)
            {
                // Update Last Login Time
                if (user.AccessRule != null)
                {
                    user.AccessRule.LastLoginDate = DateTime.UtcNow;
                    user.AccessRule.LastActivityDate = DateTime.UtcNow;
                    user.AccessRule.PasswordFailuresSinceLastSuccess = 0;
                    user.AccessRule.LastLoginDate = user.AccessRule.LastActivityDate = DateTime.UtcNow;

                    _userRepository.Update(user);
                    _unitOfWork.Commit();
                }

                // Check whether a valid token already exists ?
                var validExistingToken = _tokenRepository.GetBy(t => t.AllocatedToUserId == user.Id &&
                                                                     t.DeviceType == DeviceType.Mobile &&
                                                                     t.ExpiresOn >= DateTime.UtcNow);
                if (validExistingToken != null)
                {
                    return Json(new AuthResult
                    {
                        Status = true,
                        Message = "Success",
                        Token = validExistingToken.Key
                    });
                }
                
                // Create a new Token
                var newToken = new Token
                {
                    Key = Guid.NewGuid().ToString("N"),
                    ExpiresOn = DateTime.UtcNow.AddDays(30),
                    DeviceType = DeviceType.Mobile,
                    AllocatedToUserId = user.Id
                };

                _tokenRepository.Create(newToken);
                _unitOfWork.Commit();

                return Json(new AuthResult
                {
                    Status = true,
                    Message = "Success",
                    Token = newToken.Key
                });
            }
            
            // Validation Failed, So let's log the incorrect attempts
            var totalFailures = user.AccessRule.PasswordFailuresSinceLastSuccess;
            if (totalFailures < 5)
            {
                user.AccessRule.PasswordFailuresSinceLastSuccess += 1;
                user.AccessRule.LastPasswordFailureDate = user.AccessRule.LastActivityDate = DateTime.UtcNow;
            }
            else if (totalFailures >= 5)
            {
                user.AccessRule.LastPasswordFailureDate = user.AccessRule.LastLockoutDate = user.AccessRule.LastActivityDate = DateTime.UtcNow;
                user.AccessRule.IsLockedOut = true;
            }

            _userRepository.Update(user);
            _unitOfWork.Commit();

            return Json(new AuthResult
            {
                Status = false,
                Message = "Authentication Failed"
            });
        }
    }
}