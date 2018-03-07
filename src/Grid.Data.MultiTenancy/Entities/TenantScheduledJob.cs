using System.ComponentModel.DataAnnotations.Schema;

namespace Grid.Data.MultiTenancy.Entities
{
    public class TenantScheduledJob: TenantEntityBase
    {
        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        public string Name { get; set; }

        public string JobInfo { get; set; }
    }
}
