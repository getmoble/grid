using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Grid.Features.HRMS.Entities
{
    public class EmailTemplate: UserCreatedEntityBase
    {
        public string Name { get; set; }

        [DisplayName("Template")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }
    }
}