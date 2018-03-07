using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;

namespace Grid.Features.IMS.Entities
{
    public class AssetCategory: EntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}