using System.Collections.Generic;

namespace Grid.Infrastructure
{
    public class PermissionChecker
    {
        public static bool CheckPermission(IList<int> userPermissions, int permissionCode)
        {
            return userPermissions != null && userPermissions.Contains(permissionCode);
        }
    }
}
