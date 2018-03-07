using Grid.Features.HRMS.Entities;
using System.Collections.Generic;

namespace Grid.Infrastructure
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Permissions { get; set; }

        public static UserInfo GetInstance(User employee, List<int> permissions)
        {
            var info = new UserInfo
            {
                Id = employee.Id,
                Name = employee.Person.Name,
                Permissions = permissions
            };

            return info;
        }
    }
}
