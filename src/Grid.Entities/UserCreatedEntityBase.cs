using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;

namespace Grid.Entities
{
    public class UserCreatedEntityBase: Grid.Features.Common.EntityBase
    {
        [ScaffoldColumn(false)]
        public int CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public User CreatedByUser { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? UpdatedOn { get; set; }

        [ScaffoldColumn(false)]
        public int? UpdatedByUserId { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public User UpdatedByUser { get; set; }

        public UserCreatedEntityBase()
        {
            UpdatedOn = DateTime.UtcNow;
        }
    }
}