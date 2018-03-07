using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.IT.Entities.Enums;

namespace Grid.Features.IT.Entities
{
    public class Software: EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Version { get; set; }

        [Description("Latest Version")]
        public decimal LatestVersion { get; set; }

        [Description("Recommended Version")]
        public decimal RecommendedVersion { get; set; }

        [Description("Status")]
        public SoftwareStatus Status { get; set; }
        [Description("License Type")]
        public LicenseType LicenseType { get; set; }

        [Description("Licenses Allowed")]
        public int LicensesAllowed { get; set; }
        public int SoftwareCategoryId { get; set; }
        [ForeignKey("SoftwareCategoryId")]
        public virtual SoftwareCategory Category { get; set; }
    }
}