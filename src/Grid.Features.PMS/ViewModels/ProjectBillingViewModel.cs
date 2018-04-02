using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Grid.Features.Common;

namespace Grid.Features.PMS.ViewModels
{
    public class ProjectBillingViewModel: ViewModelBase
    {
        public int ProjectId { get; set; }
        public Entities.Project Project { get; set; }

        public decimal Amount { get; set; }

        [DisplayName("Billing Date")]
        public DateTime BillingDate { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Comments { get; set; }
        public string DocumentPath { get; set; }
        public HttpPostedFileBase Document { get; set; }
        public decimal BillingHours { get; set; }
    }
}