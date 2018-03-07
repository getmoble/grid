using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.CRM.Entities;
using Grid.Features.HRMS.Entities;
using Grid.Features.RMS.Entities;
using Grid.Features.RMS.Entities.Enums;

namespace Grid.Features.RMS.ViewModels
{
    public class RequirementDetailsViewModel : ViewModelBase
    {
        public int AssignedToUserId { get; set; }
        public Features.HRMS.Entities.User AssignedToUser { get; set; }


        public int? ContactId { get; set; }
        public CRMContact Contact { get; set; }

        public int SourceId { get; set; }
        public CRMLeadSource Source { get; set; }

        public int CategoryId { get; set; }
        public RequirementCategory Category { get; set; }

        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }


        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        public string Url { get; set; }

        [DisplayName("Billing Type")]
        public BillingType? BillingType { get; set; }
        public decimal? Budget { get; set; }
        public int StatusId { get; set; }
        public RequirementStatus RequirementStatus { get; set; }

        [DisplayName("Requirement Posted On")]
        public DateTime PostedOn { get; set; }

        [DisplayName("Responsed On")]
        public DateTime? RespondedOn { get; set; }

        public List<RequirementActivity> Activities { get; set; }

        [DisplayName("New Note/Activity")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string NewComment { get; set; }

        public Dictionary<DateTime, string> TimeLine { get; set; }

        public List<RequirementDocument> RequirementDocuments { get; set; }

        public List<Technology> Technologies { get; set; }

        public int CreatedByUserId { get; set; }
        public Features.HRMS.Entities.User CreatedByUser { get; set; }

        public RequirementDetailsViewModel()
        {
            TimeLine = new Dictionary<DateTime, string>();
            Activities = new List<RequirementActivity>();
            RequirementDocuments = new List<RequirementDocument>();
            Technologies = new List<Technology>();
        }

        public RequirementDetailsViewModel(Requirement requirement):this()
        {
            Id = requirement.Id;
            AssignedToUserId = requirement.AssignedToUserId;
            AssignedToUser = requirement.AssignedToUser;
            ContactId = requirement.ContactId;
            Contact = requirement.Contact;
            SourceId = requirement.SourceId;
            Source = requirement.Source;
            CategoryId = requirement.CategoryId;
            Category = requirement.Category;
            Title = requirement.Title;
            Description = requirement.Description;
            Url = requirement.Url;
            BillingType = requirement.BillingType;
            Budget = requirement.Budget;
            RequirementStatus = requirement.RequirementStatus;
            PostedOn = requirement.PostedOn;
            RespondedOn = requirement.RespondedOn;
            CreatedOn = requirement.CreatedOn;
            CreatedByUserId = requirement.CreatedByUserId;
            CreatedByUser = requirement.CreatedByUser;
        }
    }
}