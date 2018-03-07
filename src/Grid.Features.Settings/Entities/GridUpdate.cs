using System;
using System.ComponentModel;
using Grid.Features.Common;

namespace Grid.Features.Settings.Entities
{
    public class GridUpdate: EntityBase
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        [DisplayName("Permission Code")]
        public int PermissionCode { get; set; }
    }
}