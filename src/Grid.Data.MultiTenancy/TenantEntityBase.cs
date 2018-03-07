using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Grid.Data.MultiTenancy
{
    public class TenantEntityBase
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Created On")]
        public DateTime CreatedOn { get; set; }
    }
}
