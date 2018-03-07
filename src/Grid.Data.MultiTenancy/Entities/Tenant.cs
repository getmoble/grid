using System.ComponentModel;

namespace Grid.Data.MultiTenancy.Entities
{
    public class Tenant: TenantEntityBase
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        [DisplayName("Contact Person Name")]
        public string ContactPersonName { get; set; }

        [DisplayName("Contact Person Number")]
        public string ContactPersonNumber { get; set; }

        [DisplayName("Connection String")]
        public string ConnectionString { get; set; }

        [DisplayName("Domain Name")]
        public string DomainName { get; set; }

        [DisplayName("Sub Domain")]
        public string SubDomain { get; set; }

        public bool Status { get; set; }
    }
}
