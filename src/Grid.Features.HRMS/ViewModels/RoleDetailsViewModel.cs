using System.Collections.Generic;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.ViewModels
{
    public class RoleDetailsViewModel: ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<Features.HRMS.Entities.User> Users { get; set; }

        public RoleDetailsViewModel()
        {
            Permissions = new List<Permission>();
            Users = new List<Features.HRMS.Entities.User>();
        }

        public RoleDetailsViewModel(Role role): this()
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
            CreatedOn = role.CreatedOn;
        }
    }
}