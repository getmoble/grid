using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Entities.HRMS;
using Grid.Features.Common;

namespace Grid.Features.User
{
    public class UserCreatedEntityBase: EntityBase
    {
        [ScaffoldColumn(false)]
        public int CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public Entities.User CreatedByUser { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? UpdatedOn { get; set; }

        [ScaffoldColumn(false)]
        public int? UpdatedByUserId { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public Entities.User UpdatedByUser { get; set; }

        public UserCreatedEntityBase()
        {
            UpdatedOn = DateTime.UtcNow;
        }
    }
}