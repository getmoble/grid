using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Grid.Data.MultiTenancy.Entities
{
    public class EmailTemplate: TenantEntityBase
    {
        public string Name { get; set; }

        [DisplayName("Template")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }
    }
}
