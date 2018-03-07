using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.ViewModels.Candidate
{
    public class NewCandidateViewModel: ViewModelBase
    {
        public CandidatesSource Source { get; set; }

        public string Qualification { get; set; }

        [DisplayName("Total Experience")]
        public double? TotalExperience { get; set; }

        public HttpPostedFileBase Resume { get; set; }
        public string ResumePath { get; set; }
        public string PhotoPath { get; set; }

        public CandidateStatus Status { get; set; }

        [DisplayName("Comments")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        [DisplayName("Current CTC")]
        public double? CurrentCTC { get; set; }
        [DisplayName("Expected CTC")]
        public double? ExpectedCTC { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int CandidateDesignationId { get; set; }
        public CandidateDesignation CandidateDesignation { get; set; }

        [DisplayName("Recieved On")]
        [UIHint("Date")]
        [Required]
        public DateTime RecievedOn { get; set; }

        public int[] TechnologyIds { get; set; }
    }
}