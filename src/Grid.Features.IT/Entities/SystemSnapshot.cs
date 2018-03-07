using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;
using Grid.Features.IMS.Entities;

namespace Grid.Features.IT.Entities
{
    public class SystemSnapshot : EntityBase
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int AssetId { get; set; }
        [ForeignKey("AssetId")]
        public Asset Asset { get; set; }

        public string Softwares { get; set; }
        public string Hardwares { get; set; }

        [DisplayName("Last Updated on")]
        public DateTime RanOn { get; set; }

        public SystemSnapshot()
        {
            RanOn = DateTime.Now;
        }
    }
}