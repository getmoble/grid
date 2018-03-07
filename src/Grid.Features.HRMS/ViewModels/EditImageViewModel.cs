using System.Web;
using Grid.Features.Common;

namespace Grid.Features.HRMS.ViewModels
{
    public class EditImageViewModel: ViewModelBase
    {
        public int UserId { get; set; }
   
        public string PhotoPath { get; set; }

        public HttpPostedFileBase Photo { get; set; }

        public string FileName { get; set; }

    }
}