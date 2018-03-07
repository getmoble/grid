using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.IMS.Entities.Enums;

namespace Grid.Features.IMS.Entities
{
    public class AssetDocument: DocumentEntityBase
    {
        public int AssetId { get; set; }
        [ForeignKey("AssetId")]
        public Asset Asset { get; set; }

        public AssetDocumentType DocumentType { get; set; }
    }
}