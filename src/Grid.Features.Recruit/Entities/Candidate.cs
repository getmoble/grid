using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Features.Recruit.Entities
{
    public class Candidate: UserCreatedEntityBase
    {
        [Column(TypeName = "varchar")]
        [MaxLength(254)]
        public string Email { get; set; }

        public string Password { get; set; }

        public CandidatesSource Source { get; set; }

        public string Qualification { get; set; }

        [DisplayName("Total Experience")]
        public double? TotalExperience { get; set; }

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
        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        public int? DesignationId { get; set; }
        [ForeignKey("DesignationId")]
        public CandidateDesignation Designation { get; set; }

        [DisplayName("Recieved On")]
        [UIHint("Date")]
        [Required]
        public DateTime RecievedOn { get; set; }
    }
}
