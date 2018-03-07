using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Grid.Features.Common;

namespace Grid.Features.Attendance.ViewModels
{
    public class UploadAccessLogFileViewModel: ViewModelBase
    {
        [DisplayName("From Date")]
        [UIHint("Date")]
        public DateTime FromDate { get; set; }

        [DisplayName("To Date")]
        [UIHint("Date")]
        public DateTime ToDate { get; set; }

        public HttpPostedFileBase Log { get; set; }
    }
}