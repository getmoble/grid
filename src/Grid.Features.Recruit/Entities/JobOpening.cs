using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.Entities
{
    public class JobOpening: EntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        [Required]
        [AllowHtml]
        public string Description { get; set; }

        [DisplayName("No Of Vacancies")]
        [Required]
        public int NoOfVacancies { get; set; }

        public JobOpeningStatus OpeningStatus { get; set; }

        public string JobDescriptionPath { get; set; }
    }
}
