using System;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;
using Grid.Features.IMS.Entities.Enums;

namespace Grid.Features.IMS.Entities
{
    public class AssetAllocation : EntityBase
    {
        public AssetState State { get; set; }
        public int AssetId { get; set; }
        [ForeignKey("AssetId")]
        public virtual Asset Asset { get; set; }

        public int? AllocatedEmployeeId { get; set; }
        [ForeignKey("AllocatedEmployeeId")]
        public Employee AllocatedEmployee { get; set; }      
        public DateTime AllocatedOn { get; set; }
        public int? AllocatedByEmployeeId { get; set; }
        [ForeignKey("AllocatedByEmployeeId")]
        public Employee AllocatedByEmployee { get; set; }

    }
}