using System.Collections.Generic;

namespace Grid.Api.Models.Company
{
    public class RoleModel: ApiModelBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<int> PermissionIds { get; set; }
    }
}
