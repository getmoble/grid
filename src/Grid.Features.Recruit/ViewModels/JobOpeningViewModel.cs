using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.ViewModels
{
    public class JobOpeningViewModel : ViewModelBase
    {
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }

        [DisplayName("No Of Vacancies")]
        public int NoOfVacancies { get; set; }

        public JobOpeningStatus OpeningStatus { get; set; }

        [DisplayName("Job Description File Path")]
        public string JobDescriptionFilePath { get; set; }
    }
}