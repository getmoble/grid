using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS;

namespace Grid.Features.Recruit.Entities
{
    public class InterviewRoundActivity: UserCreatedEntityBase
    {
        public int InterviewRoundId { get; set; }
        [ForeignKey("InterviewRoundId")]
        public virtual InterviewRound InterviewRound { get; set; }

        public string Title { get; set; }

        [DisplayName("New Activity")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}
