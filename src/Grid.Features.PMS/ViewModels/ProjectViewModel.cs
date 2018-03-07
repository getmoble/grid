using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Grid.Features.Common;
using Grid.Features.CRM.Entities;
using Grid.Features.HRMS.Entities;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.Entities.Enums;

namespace Grid.Features.PMS.ViewModels
{
    public class ProjectViewModel: ViewModelBase
    {
        [DisplayName("Client")]
        public long ClientId { get; set; }
        public CRMContact Client { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Status")]
        public ProjectStatus Status { get; set; }

        [DisplayName("Project Type")]
        public ProjectType? ProjectType { get; set; }

        [DisplayName("Billing")]
        public Billing Billing { get; set; }

        [DisplayName("Expected Billing Amount")]
        public decimal ExpectedBillingAmount { get; set; }

        public decimal ActualBillingAmount
        {
            get
            {
                return ProjectBillings.Sum(b => b.Amount);
            }
        }

        public long? ParentId { get; set; }
        [DisplayName("Sub Project Of")]
        public Entities.Project ParentProject { get; set; }

        public List<ProjectBilling> ProjectBillings { get; set; }
        public List<ProjectMember> ProjectMembers { get; set; }
        public List<ProjectMileStone> ProjectMileStonesStones { get; set; }
        public List<Task> Tasks { get; set; }
        public List<ProjectDocument> ProjectDocuments { get; set; }
        public List<ProjectContribution> Contributions { get; set; }

        public List<Technology> Technologies { get; set; }

        public ProjectSettings Settings { get; set; }

        public ProjectViewModel()
        {
            ProjectBillings = new List<ProjectBilling>();
            ProjectMembers = new List<ProjectMember>();
            ProjectMileStonesStones = new List<ProjectMileStone>();
            Tasks = new List<Task>();
            ProjectDocuments = new List<ProjectDocument>();
            Contributions = new List<ProjectContribution>();
            Technologies = new List<Technology>();
        }
    }
}