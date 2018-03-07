using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;

namespace Grid.Features.Recruit.Entities
{
    public class CandidateActivity : UserCreatedEntityBase
    {
        public int CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public virtual Candidate Candidate { get; set; }

        public string Title { get; set; }

        [DisplayName("New Activity")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}