using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.Recruit.Entities.Enums;
using PagedList;

namespace Grid.Features.Recruit.ViewModels
{
    public class CandidateSearchViewModel : PagedViewModelBase
    {
        public int? CandidateDesignationId { get; set; }

        [DisplayName("Minimum Experience")]
        public int? MinExperience { get; set; }

        [DisplayName("Maximum Experience")]
        public int? MaxExperience { get; set; }

        public CandidatesSource? Source { get; set; }
        public CandidateStatus? Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [DisplayName("Start Date")]
        [UIHint("Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [UIHint("Date")]
        public DateTime? EndDate { get; set; }

        public IPagedList<Entities.Candidate> Candidates { get; set; }

        public int Total { get; set; }
    }
}