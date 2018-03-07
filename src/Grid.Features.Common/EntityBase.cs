using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Grid.Features.Common
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public string Code { get; set; }


        [DisplayName("Created On")]
        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        public EntityBase()
        {
            CreatedOn = DateTime.UtcNow;
            Code = Guid.NewGuid().ToString("N");
        }
    }
}
