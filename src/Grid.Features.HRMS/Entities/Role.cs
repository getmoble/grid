using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;

namespace Grid.Features.HRMS.Entities
{
    public class Role : EntityBase
    {
        [Column(TypeName = "varchar")]
        [MaxLength(30)]
        [Index(IsUnique = true)]
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(250)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}