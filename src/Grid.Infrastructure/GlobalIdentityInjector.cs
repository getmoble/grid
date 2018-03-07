using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Grid.Features.HRMS.DAL.Interfaces;

namespace Grid.Infrastructure
{
    public class GlobalIdentityInjector : ActionFilterAttribute, IAuthorizationFilter
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleMemberRepository _roleMemberRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public GlobalIdentityInjector(IUserRepository userRepository,
                                      IRoleMemberRepository roleMemberRepository,
                                      IRolePermissionRepository rolePermissionRepository)
        {
            _userRepository = userRepository;
            _roleMemberRepository = roleMemberRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
         
            var authCookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var serializer = new JavaScriptSerializer();
                if (authTicket != null)
                {
                    var sm = serializer.Deserialize<PrincipalModel>(authTicket.UserData);
                    var userInfo = HttpSessionWrapper.GetUserInfo(sm.Key);

                    if (userInfo == null)
                    {
                        // Couldn't get it from Session, get it from database & cache it
                        var selectedUser = _userRepository.GetBy(u => u.Code == sm.Key, "Person");
                        if (selectedUser != null)
                        {
                            var userRoles = _roleMemberRepository.GetAllBy(m => m.UserId == selectedUser.Id)
                                .Select(r => r.RoleId)
                                .ToList();
                            var permissions = _rolePermissionRepository.GetAllBy(r => userRoles.Contains(r.RoleId), "Permission")
                                .Select(p => p.Permission.PermissionCode)
                                .ToList();

                            userInfo = UserInfo.GetInstance(selectedUser, permissions);
                            HttpSessionWrapper.SetInSession(selectedUser.Code, userInfo);

                            var newUser = new Principal(userInfo.Name, sm.Key)
                            {
                                Name = userInfo.Name,
                                Id = userInfo.Id,
                                Permissions = permissions
                            };

                            filterContext.HttpContext.User = newUser;
                        }
                    }
                    else
                    {
                        var newUser = new Principal(userInfo.Name, sm.Key)
                        {
                            Name = userInfo.Name,
                            Id = userInfo.Id,
                            Permissions = userInfo.Permissions
                        };

                        HttpContext.Current.User = newUser;
                    }
                }
            }
        }
    }
}