using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Auth.Entities.Enums;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.Auth.Entities
{
    public class Token : EntityBase
    {
        public string Key { get; set; }

        [UIHint("Date")]
        [DataType(DataType.Date)]
        public DateTime ExpiresOn { get; set; }

        public DeviceType DeviceType { get; set; }

        public int AllocatedToUserId { get; set; }
        [ForeignKey("AllocatedToUserId")]
        public virtual User AllocatedToUser { get; set; }
    }
}