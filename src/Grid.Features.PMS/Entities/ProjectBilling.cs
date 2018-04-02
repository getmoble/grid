using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Grid.Features.HRMS;

namespace Grid.Features.PMS.Entities
{
    public class ProjectBilling : UserCreatedEntityBase
    {
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public decimal Amount { get; set; }

        [DisplayName("Billing Date")]
        public DateTime BillingDate { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Comments { get; set; }
        public string DocumentPath { get; set; }
        public decimal BillingHours { get; set; }
    }
}
