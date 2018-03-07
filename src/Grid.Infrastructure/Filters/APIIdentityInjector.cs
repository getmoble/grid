using System;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Auth.DAL.Interfaces;
using Grid.Features.HRMS.DAL.Interfaces;

namespace Grid.Infrastructure.Filters
{
    public class APIIdentityInjector : ActionFilterAttribute, IAuthorizationFilter
    {
        public ITokenRepository TokenRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IRoleMemberRepository RoleMemberRepository { get; set; }
        public IRolePermissionRepository RolePermissionRepository { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var authHeader = filterContext.HttpContext.Request.Headers["Auth"];

            if (!string.IsNullOrEmpty(authHeader))
            {
                var selectedToken = TokenRepository.GetBy(t => t.Key == authHeader);
                if (selectedToken != null)
                {
                    if (DateTime.UtcNow <= selectedToken.ExpiresOn)
                    {
                        var selectedUser = UserRepository.GetBy(u => u.Id == selectedToken.AllocatedToUserId, "Person");
                        if (selectedUser != null)
                        {
                            var userRoles = RoleMemberRepository.GetAllBy(m => m.UserId == selectedUser.Id)
                                .Select(r => r.RoleId)
                                .ToList();

                            var permissions = RolePermissionRepository.GetAllBy(r => userRoles.Contains(r.RoleId), "Permission")
                                .Select(p => p.Permission.PermissionCode)
                                .ToList();

                            var userInfo = UserInfo.GetInstance(selectedUser, permissions);

                            var newUser = new Principal(userInfo.Name, selectedUser.Code)
                            {
                                Name = userInfo.Name,
                                Id = userInfo.Id,
                                Permissions = permissions
                            };

                            filterContext.HttpContext.User = newUser;
                        }
                    }
                }
            }
        }
    }
}