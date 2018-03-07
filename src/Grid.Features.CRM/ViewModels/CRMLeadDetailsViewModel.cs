using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.CRM.Entities;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.CRM.ViewModels
{
    public class CRMLeadDetailsViewModel: ViewModelBase
    {
        public int? CategoryId { get; set; }
        public CRMLeadCategory Category { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        [DataType(DataType.MultilineText)]
        public string Expertise { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Received On")]
        public DateTime? RecievedOn { get; set; }

        public int LeadSourceId { get; set; }
        public CRMLeadSource LeadSource { get; set; }

        public int LeadStatusId { get; set; }
        public CRMLeadStatus LeadStatus { get; set; }

        public int AssignedToUserId { get; set; }
        public Features.HRMS.Entities.User AssignedToUser { get; set; }

        public int? LeadSourceUserId { get; set; }//id of user if lead source is user/staff.
        public Features.HRMS.Entities.User LeadSourceUser { get; set; }

        public List<Technology> Technologies { get; set; }

        public CRMLeadDetailsViewModel()
        {
            Technologies = new List<Technology>();
        }

        public CRMLeadDetailsViewModel(Features.CRM.Entities.CRMLead lead): this()
        {
            Id = lead.Id;
            CategoryId = lead.CategoryId;
            Category = lead.Category;
            PersonId = lead.PersonId;
            Person = lead.Person;
            Expertise = lead.Expertise;
            Description = lead.Description;
            RecievedOn = lead.RecievedOn;
            LeadSourceId = lead.LeadSourceId;
            LeadSource = lead.LeadSource;
            LeadStatusId = lead.LeadStatusId;
            LeadStatus = lead.LeadStatus;
            AssignedToUserId = lead.AssignedToUserId;
            AssignedToUser = lead.AssignedToUser;
            LeadSourceUserId = lead.LeadSourceUserId;
            LeadSourceUser = lead.LeadSourceUser;
            CreatedOn = lead.CreatedOn;
        }
    }
}