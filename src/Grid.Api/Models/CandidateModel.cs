using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Api.Models
{
    public class CandidateModel
    {
        public int CreatedByUserId { get; set; }
        public CandidatesSource Source { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; }
        public string Comments { get; set; }
    }
}