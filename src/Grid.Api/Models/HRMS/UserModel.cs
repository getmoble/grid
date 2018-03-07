using System;
using Grid.Features.HRMS.Entities;

namespace Grid.Api.Models.HRMS
{
    public class UserModel: ApiModelBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public DateTime? LastActivity { get; set; }

        public UserModel(User user)
        {
            Id = user.Id;
            Code = user.Code;

            if (user.Person != null)
            {
                Name = user.Person.Name;
            }

            if (user.Location != null)
            {
                Location = user.Location.Title;
            }

            if (user.Department != null)
            {
                Department = user.Department.Title;
            }

            if (user.Designation != null)
            {
                Designation = user.Designation.Title;
            }

            if (user.AccessRule != null)
            {
                LastActivity = user.AccessRule.LastActivityDate;
            }
        }
    }
}
