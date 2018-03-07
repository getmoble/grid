using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.Recruit.Entities
{
    public class JobOffer: EntityBase
    {
        public int CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public Candidate Candidate { get; set; }

        public int DesignationId { get; set; }
        [ForeignKey("DesignationId")]
        public Designation Designation { get; set; }

        public double CTC { get; set; }

        [DisplayName("Joining Date")]
        public DateTime JoiningDate { get; set; }
    }
}
