using System.Collections.Generic;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.ViewModels
{
    public class PermissionDetailsViewModel: ViewModelBase
    {
        public string Title { get; set; }
        public int PermissionCode { get; set; }
        public List<Role> Roles { get; set; }
        public List<Features.HRMS.Entities.User> Users { get; set; }

        public PermissionDetailsViewModel()
        {
            Roles = new List<Role>();
            Users = new List<Features.HRMS.Entities.User>();
        }

        public PermissionDetailsViewModel(Permission permission)
        {
            Id = permission.Id;
            Title = permission.Title;
            PermissionCode = permission.PermissionCode;
            CreatedOn = permission.CreatedOn;
        }
    }
}