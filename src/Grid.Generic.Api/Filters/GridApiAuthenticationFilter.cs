using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Script.Serialization;
using System.Web.Security;
using Autofac.Integration.WebApi;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;

namespace Grid.Generic.Api.Filters
{
    public class GridApiAuthenticationFilter: IAutofacAuthorizationFilter
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleMemberRepository _roleMemberRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public GridApiAuthenticationFilter(IUserRepository userRepository,
                                      IRoleMemberRepository roleMemberRepository,
                                      IRolePermissionRepository rolePermissionRepository)
        {
            _userRepository = userRepository;
            _roleMemberRepository = roleMemberRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var headers = actionContext.Request.Headers;
            var authCookie = headers
                .GetCookies()
                .Select(c => c[FormsAuthentication.FormsCookieName])
                .FirstOrDefault();

            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var serializer = new JavaScriptSerializer();
                if (authTicket != null)
                {
                    var sm = serializer.Deserialize<PrincipalModel>(authTicket.UserData);
                    
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

                        var userInfo = UserInfo.GetInstance(selectedUser, permissions);

                        var newUser = new Principal(userInfo.Name, sm.Key)
                        {
                            Name = userInfo.Name,
                            Id = userInfo.Id,
                            Permissions = permissions
                        };

                        Thread.CurrentPrincipal = newUser;
                        HttpContext.Current.User = newUser;
                    }
                }
            }

            return Task.FromResult(0);
        }
    }
}
