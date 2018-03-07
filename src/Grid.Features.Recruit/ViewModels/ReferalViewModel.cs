using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Grid.Features.Common;

namespace Grid.Features.Recruit.ViewModels
{
    public class ReferalViewModel: ViewModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Qualification { get; set; }
        
        [DisplayName("Total Experience")]
        public double TotalExperience { get; set; }

        [DisplayName("Comments")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
        public string ResumePath { get; set; }
        public HttpPostedFileBase Resume { get; set; }
        public int JobOpeningId { get; set; }
    }
}