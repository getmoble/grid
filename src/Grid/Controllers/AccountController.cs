using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Grid.Data;
using Grid.Features.Common;
using Grid.Features.Common.Models;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRoleMemberRepository _roleMemberRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GridDataContext _dataContext;
        public AccountController(GridDataContext dataContext,
                                 IUserRepository userRepository, 
                                 IRoleMemberRepository roleMemberRepository,
                                 IRolePermissionRepository rolePermissionRepository,
                                 IUnitOfWork unitOfWork)
        {
            _dataContext = dataContext;
            _userRepository = userRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _roleMemberRepository = roleMemberRepository;
            _unitOfWork = unitOfWork;
        }

        private const string FallbackReturnUrl = "/Home/Index";

        private ActionResult RedirectToPasswordChangeScreen()
        {
            return RedirectToAction("ChangePassword", "Profile", new { expired = true });
        }

        private ActionResult RedirectToLastPage(string returnUrl)
        {
            // Redirect to the Previous page if required
            var decodedUrl = string.Empty;
            if (!string.IsNullOrEmpty(returnUrl))
                decodedUrl = Server.UrlDecode(returnUrl);

            if (!string.IsNullOrEmpty(decodedUrl) && Url.IsLocalUrl(decodedUrl))
                return Redirect(returnUrl);

            return Redirect(FallbackReturnUrl);
        }

        private void RecordLoginSucceeded(User user)
        {
            var userRoles = _roleMemberRepository.GetAllBy(m => m.UserId == user.Id).Select(r => r.RoleId).ToList();
            var permissions = _rolePermissionRepository.GetAllBy(r => userRoles.Contains(r.RoleId), "Permission").Select(p => p.Permission.PermissionCode).ToList();

            var userInfo = UserInfo.GetInstance(user, permissions);
            HttpSessionWrapper.SetUserInfo(user.Code, userInfo);

            var serializeModel = new PrincipalModel(user.Code);
            var serializer = new JavaScriptSerializer();
            var userData = serializer.Serialize(serializeModel);
            var authTicket = new FormsAuthenticationTicket(1, user.Code, DateTime.Now, DateTime.Now.AddHours(2), false, userData);
            var eticket = FormsAuthentication.Encrypt(authTicket);
            var fcookie = new HttpCookie(FormsAuthentication.FormsCookieName, eticket);
            Response.Cookies.Add(fcookie);

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
        }

        private void RecordPasswordFailure(User user)
        {
            // Validation Failed, So let's log the incorrect attempts
            var totalFailures = user.AccessRule.PasswordFailuresSinceLastSuccess;
            if (totalFailures < 5)
            {
                user.AccessRule.PasswordFailuresSinceLastSuccess += 1;
                user.AccessRule.LastPasswordFailureDate = user.AccessRule.LastActivityDate = DateTime.UtcNow;
            }
            else if (totalFailures >= 5)
            {
                user.AccessRule.LastPasswordFailureDate =
                    user.AccessRule.LastLockoutDate = user.AccessRule.LastActivityDate = DateTime.UtcNow;
                user.AccessRule.IsLockedOut = true;
            }

            _userRepository.Update(user);
            _unitOfWork.Commit();
        }

        public static GoogleClientSecrets GetClientConfiguration()
        {
            var path = HostingEnvironment.MapPath("~/client_secret.json");
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return GoogleClientSecrets.Load(stream);
            }
        }

        //[HttpGet]
        //public ActionResult AutoUpdate()
        //{
        //    //Check whether the database is upto date
        //    var migrationConfiguration = new Data.Migrations.Configuration
        //    {
        //        TargetDatabase =
        //            new DbConnectionInfo(_dataContext.Database.Connection.ConnectionString, "System.Data.SqlClient")
        //    };
        //    var migrator = new DbMigrator(migrationConfiguration);
        //    if (migrator.GetPendingMigrations().Any())
        //    {
        //        migrator.Update();
        //    }

        //    return RedirectToAction("SignIn");
        //}

        [HttpGet]
        public ActionResult SignIn(string returnUrl)
        {
            var vm = new SignInViewModel();

            //So that the user can be referred back to where they were when they click logon
            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
            {
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);
            }

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                vm.ReturnUrl = returnUrl;
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(vm.Email))
                        throw new Exception("Email is Empty");

                    if (string.IsNullOrEmpty(vm.Password))
                        throw new Exception("Password is Empty");

                    var user = _userRepository.GetBy(u => u.Username == vm.Email, "AccessRule,Person");

                    if (user == null)
                        throw new Exception("User not found");

                    if (user.AccessRule == null)
                        throw new Exception("User AccessRule not found");

                    if (!user.AccessRule.IsApproved)
                        throw new Exception("User is not approved");

                    if(user.EmployeeStatus.HasValue && user.EmployeeStatus == EmployeeStatus.Ex)
                        throw new Exception("Your no longer have access to the System");

                    var hashedPassword = user.Password;

                    var verificationSucceeded = hashedPassword != null && HashHelper.CheckHash(vm.Password, hashedPassword);

                    if (verificationSucceeded)
                    {
                        RecordLoginSucceeded(user);

                        // Check on the Last Password Change and direct
                        if (user.AccessRule.LastPasswordChangedDate.HasValue)
                        {
                            return DateTime.Now.Subtract(user.AccessRule.LastPasswordChangedDate.Value).Days > 30 ? RedirectToPasswordChangeScreen() : RedirectToLastPage(vm.ReturnUrl);
                        }

                        return RedirectToPasswordChangeScreen();
                    }

                    RecordPasswordFailure(user);
                }

                ModelState.AddModelError(string.Empty, "Invalid Credentials");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View(vm);
        }

        [HttpGet]
        public virtual ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn", "Account");
        }

        [HttpPost]
        public ActionResult SignInWithGoogle(string code)
        {
            // Use the code exchange flow to get an access and refresh token.
            IAuthorizationCodeFlow flow =
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = GetClientConfiguration().Secrets,
                Scopes = new[] { Oauth2Service.Scope.PlusLogin, Oauth2Service.Scope.UserinfoProfile, Oauth2Service.Scope.UserinfoEmail }
            });

            var response = flow.ExchangeCodeForTokenAsync("", code, "postmessage", CancellationToken.None).Result; // response.accessToken now should be populated

            var credential = new UserCredential(flow, "me", response);

            var oauthSerivce = new Oauth2Service(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Grid Intranet"
            });

            var userInfo = oauthSerivce.Userinfo.Get().Execute();

            var user = _userRepository.GetBy(u => u.Username == userInfo.Email, "AccessRule,Person");

            if (user == null)
                return Json(false);

            if (user.AccessRule == null)
                return Json(false);

            if (!user.AccessRule.IsApproved)
                return Json(false);

            // Everything is fine.
            RecordLoginSucceeded(user);

            return Json(true);
        }
    }
}