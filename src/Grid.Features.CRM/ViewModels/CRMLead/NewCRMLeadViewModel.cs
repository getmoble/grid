using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.CRM.ViewModels.CRMLead
{
    public class NewCRMLeadViewModel: ViewModelBase
    {
        public int? CategoryId { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int[] TechnologyIds { get; set; }

        [DataType(DataType.MultilineText)]
        public string Expertise { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Received On")]
        public DateTime? RecievedOn { get; set; }

        public int LeadSourceId { get; set; }
        public int LeadStatusId { get; set; }
        public int AssignedToUserId { get; set; }
        public int? LeadSourceUserId { get; set; }//id of user if lead source is user/staff.
    }
}